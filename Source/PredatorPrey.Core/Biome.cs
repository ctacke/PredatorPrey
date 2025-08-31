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
                baseResult = SimulationConfig.SeaFoodGrowthRate;
                break;
            case TerrainType.Littoral:
                baseResult = SimulationConfig.LittoralFoodGrowthRate;
                break;
            case TerrainType.Beach:
                baseResult = SimulationConfig.BeachFoodGrowthRate;
                break;
            case TerrainType.Grass:
                baseResult = SimulationConfig.GrassFoodGrowthRate;
                break;
            case TerrainType.Forest:
                baseResult = SimulationConfig.ForestFoodGrowthRate;
                break;
            case TerrainType.Mountain:
                baseResult = SimulationConfig.MountainFoodGrowthRate;
                break;
        }

        baseResult *= Random.Shared.NextSingle();

        return baseResult;
    }

    public int GetMaxPopulationCapacity()
    {
        switch (TerrainType)
        {
            case TerrainType.Sea:
                return SimulationConfig.SeaMaxPopulation;
            case TerrainType.Littoral:
                return SimulationConfig.LittoralMaxPopulation;
            case TerrainType.Beach:
                return SimulationConfig.BeachMaxPopulation;
            case TerrainType.Grass:
                return SimulationConfig.GrassMaxPopulation;
            case TerrainType.Forest:
                return SimulationConfig.ForestMaxPopulation;
            case TerrainType.Mountain:
                return SimulationConfig.MountainMaxPopulation;
            default:
                return SimulationConfig.DefaultMaxPopulation;
        }
    }

    public float GetMaxFoodCapacity()
    {
        switch (TerrainType)
        {
            case TerrainType.Sea:
                return SimulationConfig.SeaMaxFood;
            case TerrainType.Littoral:
                return SimulationConfig.LittoralMaxFood;
            case TerrainType.Beach:
                return SimulationConfig.BeachMaxFood;
            case TerrainType.Grass:
                return SimulationConfig.GrassMaxFood;
            case TerrainType.Forest:
                return SimulationConfig.ForestMaxFood;
            case TerrainType.Mountain:
                return SimulationConfig.MountainMaxFood;
            default:
                return SimulationConfig.DefaultMaxFood;
        }
    }
}
