using System.Drawing;

namespace PredatorPrey;

public class World
{
    public const int InitialPopulationSize = 1000;
    public const double InitialFoodDistribution = 0.05;

    public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }
    public PopulationMap Population { get; set; } = new();

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

        foreach (var organism in Population)
        {
            // TODO: mate / eat
            motionController.Move(organism, Population, this.Dimensions.Width, this.Dimensions.Height);

            var location = Population.GetOrganismLocation(organism);
            if (location != null)
            {
                if (Regions[location.Value.X, location.Value.Y].AvailableFood >= 1)
                {
                    // TODO: make organism eat rate variable
                    organism.Health += 20;
                    Regions[location.Value.X, location.Value.Y].AvailableFood -= 1;
                }
            }

            if (organism.IsDead)
            {
                Population.Remove(organism);
            }
        }
    }

    private void GrowFood(Region region)
    {
        region.AvailableFood += region.Biome.GrowFood();
    }
}
