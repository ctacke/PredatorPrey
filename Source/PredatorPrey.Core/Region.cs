using System.Drawing;

namespace PredatorPrey;

public class Region
{
    private double _availableFood;

    public Biome Biome { get; set; }
    public Point Location { get; set; }
    public double AvailableFood
    {
        get => _availableFood;
        set
        {
            var max = Biome.GetMaxFoodCapacity();
            _availableFood = (value > max) ? max : value;
        }
    }
}
