using System.Drawing;

namespace PredatorPrey;

public class World
{
    public const int InitialPopulationSize = 1000;
    public const double InitialFoodDistribution = 0.05;
    public const int HealthPerFoodUnit = 10;

    public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }

    private MotionController motionController;

    public World()
    {
        Build();
    }

    private void Build()
    {
        motionController = new MotionController();

        var terrainGenerator = new TerrainGenerator(Dimensions.Width, Dimensions.Height);

        Regions = terrainGenerator.Generate(InitialFoodDistribution);

        var organismGenerator = new OrganismGenerator();
        organismGenerator.Populate(this, InitialPopulationSize);
    }

    public void RunCycle()
    {
        foreach (var region in Regions)
        {
            GrowFood(region);
        }

        foreach (var region in Regions)
        {
            foreach (var organism in region.Organisms.ToList())
            {
                // TODO: mate / eat
                motionController.Move(organism, this);

                if (region.AvailableFood >= 1)
                {
                    organism.Health += HealthPerFoodUnit;
                }

                if (organism.IsDead)
                {
                    region.Organisms.Remove(organism);
                }
            }
        }
    }

    private double _foodGenerationRate = 0.01;

    private void GrowFood(Region region)
    {
        var rate = _foodGenerationRate;

        if (region.AvailableFood > 1)
        {
            rate /= 2;
        }

        if (Random.Shared.NextDouble() < rate)
        {
            region.AvailableFood += 0.1;
        }
    }
}
