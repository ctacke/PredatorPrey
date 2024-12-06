using System.Drawing;

namespace PredatorPrey;

public class World
{
    public Size Dimensions { get; } = new Size(512, 256);
    public Region[,] Regions { get; set; }

    public World()
    {
        Build();
    }

    private void Build()
    {
        var generator = new TerrainGenerator(Dimensions.Width, Dimensions.Height);

        Regions = generator.Generate();
    }

    public void RunCycle()
    {
        foreach (var region in Regions)
        {
            GrowFood(region);
        }
    }

    private double _foodGenerationRate = 0.05;

    private void GrowFood(Region region)
    {
        if (region.AvailableFood > 0)
        {
            return;
        }

        if (_foodGenerationRate < Random.Shared.NextDouble())
        {
            region.AvailableFood += 1;
        }
    }
}
