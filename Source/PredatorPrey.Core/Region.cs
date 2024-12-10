using System.Drawing;

namespace PredatorPrey;

public class Region
{
    public Biome Biome { get; set; }
    public Point Location { get; set; }
    public double AvailableFood { get; set; }
}
