namespace PredatorPrey;

public class Biome(TerrainType terrainType)
{
    public TerrainType TerrainType { get; set; } = terrainType;

    public float GrowFood()
    {
        float baseResult = 0;

        switch (TerrainType)
        {
            case TerrainType.Sea:
                baseResult = 0.05f;
                break;
            case TerrainType.Littoral:
                baseResult = 0.007f;
                break;
            case TerrainType.Beach:
                baseResult = 0.0025f;
                break;
            case TerrainType.Grass:
                baseResult = 0.006f;
                break;
            case TerrainType.Forest:
                baseResult = 0.004f;
                break;
            case TerrainType.Mountain:
                baseResult = 0.002f;
                break;
        }

        baseResult *= Random.Shared.NextSingle();

        return baseResult;
    }

    public float GetMaxFoodCapacity()
    {
        switch (TerrainType)
        {
            case TerrainType.Sea:
                return 4;
            case TerrainType.Littoral:
                return 5;
            case TerrainType.Beach:
                return 3;
            case TerrainType.Grass:
                return 8;
            case TerrainType.Forest:
                return 3;
            case TerrainType.Mountain:
                return 2;
            default:
                return 1;
        }
    }
}
