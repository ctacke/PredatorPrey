using System.Diagnostics;
using System.Drawing;

namespace PredatorPrey;

public class World
{
    public const int InitialPopulationSize = 200;
    public const double InitialFoodDistribution = 0.05;

    public Size Dimensions { get; } = new Size(256, 128);
    //public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }
    public PopulationMap Population { get; set; } = new();

    public long WorldAge { get; private set; } = 0;
    public int DeathsFromAge { get; private set; } = 0;
    public int DeathsFromStarvation { get; private set; } = 0;

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

        if (Population.Population == 0)
        {
            Console.WriteLine("Everything is dead!");
            Console.WriteLine($" Age of the world: {WorldAge} cycles");
            Console.WriteLine($" Deaths from old age: {DeathsFromAge}");
            Console.WriteLine($" Deaths from starvation: {DeathsFromStarvation}");

            Debugger.Break();
        }

        int oldest = 0;

        foreach (var organism in Population)
        {
            var location = Population.GetOrganismLocation(organism);
            if (location != null)
            {
                var region = Regions[location.Value.X, location.Value.Y];

                var motion = _motionController.Move(organism, Population, this.Dimensions.Width, this.Dimensions.Height);
                organism.Metabolize(region, motion);

                organism.TryEat(region);

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

            var newOrganisms = parents
                .SelectMany((o1, i) => parents.Skip(i + 1)
                .Select(o2 => _organismGenerator.Reproduce(o1, o2)))
                .Where(c => c != null)
                .ToList();

            if (newOrganisms.Count > 0)
            {
                Population.AddRange(newOrganisms, this.Dimensions.Width, this.Dimensions.Height);
                Debug.WriteLine($"Population: {Population.Population}");
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
