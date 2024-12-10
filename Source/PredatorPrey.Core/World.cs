using System.Diagnostics;
using System.Drawing;

namespace PredatorPrey;

public class World
{
    public const int InitialPopulationSize = 100;
    public const double InitialFoodDistribution = 0.05;
    public const double ReproductionRate = 0.50; // percent chance an interaction will yield reproduction

    public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }
    public PopulationMap Population { get; set; } = new();

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
        foreach (var region in Regions)
        {
            GrowFood(region);
        }

        if (Population.Population == 0)
        {
            Debugger.Break();
        }

        foreach (var organism in Population)
        {
            _motionController.Move(organism, Population, this.Dimensions.Width, this.Dimensions.Height);

            var location = Population.GetOrganismLocation(organism);
            if (location != null)
            {
                organism.TryEat(Regions[location.Value.X, location.Value.Y]);
            }

            if (organism.IsDead)
            {
                Population.Remove(organism);
            }
            else
            {
                organism.Age++;
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
    }
}
