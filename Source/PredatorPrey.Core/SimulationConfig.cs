namespace PredatorPrey;

/// <summary>
/// Static configuration class containing all tunable simulation parameters.
/// This centralizes all configurable values to enable future UI configuration.
/// </summary>
public static class SimulationConfig
{
    #region World Parameters
    
    /// <summary>
    /// Initial number of organisms when world is created
    /// </summary>
    public static int InitialPopulationSize { get; set; } = 800;
    
    /// <summary>
    /// Initial percentage of regions that contain food (0.0-1.0)
    /// </summary>
    public static double InitialFoodDistribution { get; set; } = 0.05;
    
    /// <summary>
    /// Population threshold that triggers debugging breakpoint
    /// </summary>
    public static int PopulationAlertThreshold { get; set; } = 10000;
    
    /// <summary>
    /// Multiplier for reproduction population limits (max capacity * this value)
    /// </summary>
    public static double ReproductionPopulationMultiplier { get; set; } = 1.2;
    
    #endregion

    #region Organism Lifecycle Parameters
    
    /// <summary>
    /// Maximum age an organism can reach before dying of old age
    /// </summary>
    public static int MaxAge { get; set; } = 500;
    
    /// <summary>
    /// Minimum age before organism can reproduce
    /// </summary>
    public static int MinReproductionAge { get; set; } = 80;
    
    /// <summary>
    /// Base fertility rate for reproduction attempts (0.0-1.0)
    /// </summary>
    public static float BaseFertility { get; set; } = 0.60f;
    
    /// <summary>
    /// Base metabolic rate for energy consumption
    /// </summary>
    public static float BaseMetabolicRate { get; set; } = 1.0f;
    
    /// <summary>
    /// Amount of food an organism provides when it dies
    /// </summary>
    public static float ValueAsFood { get; set; } = 0.5f;
    
    /// <summary>
    /// Multiplier for health loss when starving
    /// </summary>
    public static short StarvationFactor { get; set; } = 1;
    
    /// <summary>
    /// Health gained per unit of food consumed
    /// </summary>
    public static short HealthPerFood { get; set; } = 8;
    
    #endregion

    #region Movement and Metabolism Parameters
    
    /// <summary>
    /// Base terrain factor for metabolism (neutral terrain)
    /// </summary>
    public static float BaseTerrainFactor { get; set; } = 1.0f;
    
    /// <summary>
    /// Terrain factor for land animals in water
    /// </summary>
    public static float WaterTerrainFactor { get; set; } = 2.0f;
    
    /// <summary>
    /// Terrain factor for aquatic animals on grass
    /// </summary>
    public static float GrassTerrainFactor { get; set; } = 2.0f;
    
    /// <summary>
    /// Terrain factor for aquatic animals in forest
    /// </summary>
    public static float ForestTerrainFactor { get; set; } = 2.5f;
    
    /// <summary>
    /// Terrain factor for aquatic animals on mountains
    /// </summary>
    public static float MountainTerrainFactor { get; set; } = 3.0f;
    
    /// <summary>
    /// Movement metabolism multiplier (reduces energy cost)
    /// </summary>
    public static float MovementMetabolismMultiplier { get; set; } = 0.5f;
    
    /// <summary>
    /// Idle metabolism multiplier for stationary organisms
    /// </summary>
    public static float IdleMetabolismMultiplier { get; set; } = 0.5f;
    
    #endregion

    #region Base Movement Speeds by Terrain
    
    public static short SeaBaseMovement { get; set; } = 3;
    public static short LittoralBaseMovement { get; set; } = 2;
    public static short BeachBaseMovement { get; set; } = 2;
    public static short GrassBaseMovement { get; set; } = 3;
    public static short ForestBaseMovement { get; set; } = 2;
    public static short MountainBaseMovement { get; set; } = 1;
    
    #endregion

    #region Food Growth Rates by Terrain
    
    public static float SeaFoodGrowthRate { get; set; } = 0.15f;
    public static float LittoralFoodGrowthRate { get; set; } = 0.18f;
    public static float BeachFoodGrowthRate { get; set; } = 0.09f;
    public static float GrassFoodGrowthRate { get; set; } = 0.18f;
    public static float ForestFoodGrowthRate { get; set; } = 0.12f;
    public static float MountainFoodGrowthRate { get; set; } = 0.06f;
    
    #endregion

    #region Population Capacity by Terrain
    
    public static int SeaMaxPopulation { get; set; } = 6;
    public static int LittoralMaxPopulation { get; set; } = 8;
    public static int BeachMaxPopulation { get; set; } = 4;
    public static int GrassMaxPopulation { get; set; } = 12;
    public static int ForestMaxPopulation { get; set; } = 5;
    public static int MountainMaxPopulation { get; set; } = 3;
    public static int DefaultMaxPopulation { get; set; } = 2;
    
    #endregion

    #region Food Capacity by Terrain
    
    public static float SeaMaxFood { get; set; } = 6f;
    public static float LittoralMaxFood { get; set; } = 8f;
    public static float BeachMaxFood { get; set; } = 4f;
    public static float GrassMaxFood { get; set; } = 12f;
    public static float ForestMaxFood { get; set; } = 5f;
    public static float MountainMaxFood { get; set; } = 3f;
    public static float DefaultMaxFood { get; set; } = 2f;
    
    #endregion

    #region Overpopulation Penalties
    
    /// <summary>
    /// Multiplier for severe overpopulation health reduction
    /// </summary>
    public static int SevereOverpopulationHealthDivisor { get; set; } = 4;
    
    /// <summary>
    /// Multiplier threshold for moderate overpopulation (2x carrying capacity)
    /// </summary>
    public static int ModerateOverpopulationMultiplier { get; set; } = 2;
    
    /// <summary>
    /// Multiplier threshold for severe overpopulation (3x carrying capacity)
    /// </summary>
    public static int SevereOverpopulationMultiplier { get; set; } = 3;
    
    /// <summary>
    /// Multiplier threshold for extreme overpopulation (4x carrying capacity)
    /// </summary>
    public static int ExtremeOverpopulationMultiplier { get; set; } = 4;
    
    #endregion

    #region Genetic Parameters
    
    /// <summary>
    /// Chance of genetic mutation during reproduction (0.0-1.0)
    /// </summary>
    public static float MutationRate { get; set; } = 0.005f; // 0.5%
    
    /// <summary>
    /// Initial health value for newborn organisms
    /// </summary>
    public static short InitialHealth { get; set; } = 0xff; // 255
    
    #endregion

    /// <summary>
    /// Reset all parameters to their default values
    /// </summary>
    public static void ResetToDefaults()
    {
        InitialPopulationSize = 800;
        InitialFoodDistribution = 0.05;
        PopulationAlertThreshold = 10000;
        ReproductionPopulationMultiplier = 1.2;
        
        MaxAge = 500;
        MinReproductionAge = 80;
        BaseFertility = 0.60f;
        BaseMetabolicRate = 1.0f;
        ValueAsFood = 0.5f;
        StarvationFactor = 1;
        HealthPerFood = 8;
        
        BaseTerrainFactor = 1.0f;
        WaterTerrainFactor = 2.0f;
        GrassTerrainFactor = 2.0f;
        ForestTerrainFactor = 2.5f;
        MountainTerrainFactor = 3.0f;
        MovementMetabolismMultiplier = 0.5f;
        IdleMetabolismMultiplier = 0.5f;
        
        SeaBaseMovement = 3;
        LittoralBaseMovement = 2;
        BeachBaseMovement = 2;
        GrassBaseMovement = 3;
        ForestBaseMovement = 2;
        MountainBaseMovement = 1;
        
        SeaFoodGrowthRate = 0.15f;
        LittoralFoodGrowthRate = 0.18f;
        BeachFoodGrowthRate = 0.09f;
        GrassFoodGrowthRate = 0.18f;
        ForestFoodGrowthRate = 0.12f;
        MountainFoodGrowthRate = 0.06f;
        
        SeaMaxPopulation = 6;
        LittoralMaxPopulation = 8;
        BeachMaxPopulation = 4;
        GrassMaxPopulation = 12;
        ForestMaxPopulation = 5;
        MountainMaxPopulation = 3;
        DefaultMaxPopulation = 2;
        
        SeaMaxFood = 6f;
        LittoralMaxFood = 8f;
        BeachMaxFood = 4f;
        GrassMaxFood = 12f;
        ForestMaxFood = 5f;
        MountainMaxFood = 3f;
        DefaultMaxFood = 2f;
        
        SevereOverpopulationHealthDivisor = 4;
        ModerateOverpopulationMultiplier = 2;
        SevereOverpopulationMultiplier = 3;
        ExtremeOverpopulationMultiplier = 4;
        
        MutationRate = 0.005f;
        InitialHealth = 0xff;
    }
}