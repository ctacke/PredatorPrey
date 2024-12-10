namespace PredatorPrey;

public class Biome(TerrainType terrainType)
{
    public TerrainType TerrainType { get; set; } = terrainType;

    public double GrowFood()
    {
        double baseResult = 0;

        switch (TerrainType)
        {
            case TerrainType.Sea:
                baseResult = 0.005;
                break;
            case TerrainType.Littoral:
                baseResult = 0.007;
                break;
            case TerrainType.Beach:
                baseResult = 0.0025;
                break;
            case TerrainType.Grass:
                baseResult = 0.006;
                break;
            case TerrainType.Forest:
                baseResult = 0.004;
                break;
            case TerrainType.Mountain:
                baseResult = 0.001;
                break;
        }

        baseResult *= Random.Shared.NextDouble();

        return baseResult;
    }
}
