using System.Diagnostics;
using System.Drawing;

namespace PredatorPrey;

public class World
{
    public static int InitialPopulationSize => SimulationConfig.InitialPopulationSize;
    public static double InitialFoodDistribution => SimulationConfig.InitialFoodDistribution;

    public Size Dimensions { get; } = new Size(256, 128);
    //public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }
    public PopulationMap Population { get; set; } = new();

    public long WorldAge { get; private set; } = 0;
    public int DeathsFromAge { get; private set; } = 0;
    public int DeathsFromStarvation { get; private set; } = 0;
    public int DeathsFromOverpopulation { get; private set; } = 0;

    private MotionController _motionController;
    private OrganismGenerator _organismGenerator;

    public World()
    {
        Build();
    }

    private void Build()
    {
        _motionController = new MotionController();

        var terrainGenerator = new TerrainGenerator(Dimensions.Width, Dimensions.Height);

        Regions = terrainGenerator.Generate(InitialFoodDistribution);

        _organismGenerator = new OrganismGenerator();
        _organismGenerator.Populate(this, InitialPopulationSize);
    }

    public void RunCycle()
    {
        WorldAge++;

        foreach (var region in Regions)
        {
            GrowFood(region);
        }

        if (Population.Population > SimulationConfig.PopulationAlertThreshold)
        {
            //Debugger.Break();
        }

        if (Population.Population == 0)
        {
            Debug.WriteLine("Everything is dead!");
            Debug.WriteLine($" Age of the world: {WorldAge} cycles");
            Debug.WriteLine($" Deaths from old age: {DeathsFromAge}");
            Debug.WriteLine($" Deaths from starvation: {DeathsFromStarvation}");
            Debug.WriteLine($" Deaths from over-population: {DeathsFromOverpopulation}");

            Debugger.Break();
        }

        int oldest = 0;

        foreach (var organism in Population)
        {
            var location = Population.GetOrganismLocation(organism);
            if (location != null)
            {
                var region = Regions[location.Value.X, location.Value.Y];

                var motion = _motionController.Move(organism, Population, this);

                var currentpop = Population.GetOrganismsAtLocation(motion.newX, motion.newY).Count();
                var max = region.Biome.GetMaxPopulationCapacity();

                if (currentpop > max)
                {
                    // region is overpopulated
                    organism.Health -= (short)(currentpop - max);
                    if (organism.IsDead)
                    {
                        organism.DeathReason = DeathReason.Overpopulation;
                        DeathsFromOverpopulation++;
                    }

                    // region is superpopulated.  Reduce food to zero
                    if (currentpop > SimulationConfig.ModerateOverpopulationMultiplier * max)
                    {
                        region.AvailableFood = 0;
                    }

                    if (currentpop > SimulationConfig.SevereOverpopulationMultiplier * max)
                    {
                        organism.Health /= (short)SimulationConfig.SevereOverpopulationHealthDivisor;
                    }

                    if (currentpop > SimulationConfig.ExtremeOverpopulationMultiplier * max)
                    {
                        organism.Health = 0;
                    }
                }

                if (!organism.IsDead)
                {
                    organism.Metabolize(region, motion.speed);
                    organism.TryEat(region);
                }

                if (organism.IsDead)
                {
                    // fertilize the biome
                    region.AvailableFood += organism.ValueAsFood;

                    if (organism.DeathReason != null)
                    {
                        switch (organism.DeathReason)
                        {
                            case DeathReason.Age:
                                DeathsFromAge++;
                                break;
                            case DeathReason.Starvation:
                                DeathsFromStarvation++;
                                break;
                        }
                    }

                    Population.Remove(organism);
                }
                else
                {
                    organism.Age++;
                    if (organism.Age > oldest)
                    {
                        oldest = organism.Age;
                    }
                }
            }
        }

        // reproduction cycle
        foreach (var overlap in Population.GetPointsWithMultipleOrganisms().ToArray())
        {
            var parents = Population.GetOrganismsAtLocation(overlap);

            var region = Regions[overlap.X, overlap.Y];
            var max = region.Biome.GetMaxPopulationCapacity();

            // Only reproduce if population is well below carrying capacity
            if (parents.Count() < (max * SimulationConfig.ReproductionPopulationMultiplier))
            {
                // Limit reproduction attempts - only allow one pair to reproduce per region per cycle
                var reproductiveParents = parents.Take(2).ToArray();
                if (reproductiveParents.Length == 2)
                {
                    var newOrganism = _organismGenerator.Reproduce(reproductiveParents[0], reproductiveParents[1]);
                    if (newOrganism != null)
                    {
                        Population.Add(newOrganism, overlap.X, overlap.Y);
                        Debug.WriteLine($"Population: {Population.Population}");
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Sterile land");
            }
        }
    }

    private void GrowFood(Region region)
    {
        region.AvailableFood += region.Biome.GrowFood();
        var max = region.Biome.GetMaxFoodCapacity();
        if (region.AvailableFood > max) { region.AvailableFood = max; }
    }
}
