namespace PredatorPrey;

public enum DeathReason
{
    Starvation,
    Age,
    Overpopulation,
    Predation,
    Disease
}

public class Organism
{
    private short _health = SimulationConfig.InitialHealth;
    private int _age;
    private DeathReason? _deathReason;

    public Guid ID { get; set; }
    public Guid ParentA { get; set; }
    public Guid ParentB { get; set; }

    public Chromosome ChromosomeA { get; set; }
    public Chromosome ChromosomeB { get; set; }

    public int MaxAge { get; set; } = SimulationConfig.MaxAge;
    public int MinReproductionAge { get; set; } = SimulationConfig.MinReproductionAge;

    /// <summary>
    /// The likelihood that the organism will reproduce when interacting with another (percentile)
    /// </summary>
    public float Fertility { get; set; } = SimulationConfig.BaseFertility;

    /// <summary>
    /// rate at which food is consumed (and, perhaps, things like movement rate)
    /// </summary>
    public float MetabolicRate { get; set; } = SimulationConfig.BaseMetabolicRate;

    /// <summary>
    /// How much "food" is this organism worth on death (as compost to the biome or as food to another organism)
    /// </summary>
    public float ValueAsFood { get; set; } = SimulationConfig.ValueAsFood;

    /// <summary>
    /// When a region can't provide food, the organism's health falls by this * metabolic rate
    /// </summary>
    public short StarvationFactor { get; set; } = SimulationConfig.StarvationFactor;

    /// <summary>
    /// How much health does the organism get per metabolic-rate unit of food
    /// </summary>
    public short HealthPerFood { get; set; } = SimulationConfig.HealthPerFood;

    public bool IsDead => Health == 0;

    public Organism(Organism parentA, Organism parentB)
    {
        ID = new Guid();

        var geneCount = parentA.ChromosomeA.Genes.Length;

        this.ChromosomeA = new Chromosome()
        {
            Genes = new Gene[geneCount]
        };

        for (var i = 0; i < geneCount; i++)
        {
            this.ChromosomeA.Genes[i] = parentA.ChromosomeA.Genes[i].Crossover(parentB.ChromosomeA.Genes[i]);
        }
    }

    public Organism()
    {
        ID = new Guid();

        var isLandAnimal = Random.Shared.Next(0, 2) == 1;

        ChromosomeA = new Chromosome
        {
            Genes =
             [
                new Legs { Value = (byte)(isLandAnimal ? 1 : 0) },
                new Fins { Value = (byte)(isLandAnimal ? 0 : 1) }
            ]
        };
    }

    public bool CanReproduce
    {
        get
        {
            if (IsDead) return false;
            return Age >= MinReproductionAge;
        }
    }

    public DeathReason? DeathReason
    {
        get => _deathReason;
        set
        {
            if (_deathReason == null) _deathReason = value;
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            _age = value;
            // TODO: make this a distribution, not a hard line
            if (_age > MaxAge)
            {
                Health = 0;
                DeathReason = PredatorPrey.DeathReason.Age;
            }
        }
    }

    public short Health
    {
        get => _health;
        set
        {
            if (_health == 0) return;
            if (value <= 0) value = 0;
            _health = value;
        }
    }

    public void TryEat(Region region)
    {
        if (IsDead) return;

        if (region.AvailableFood <= MetabolicRate)
        {
            Health = (short)(Health - (short)(MetabolicRate * StarvationFactor)); // starvation
            region.AvailableFood = 0;

            if (IsDead)
            {
                DeathReason = PredatorPrey.DeathReason.Starvation;
            }
        }
        else
        {
            // TODO: make organism health rate increase variable
            Health += HealthPerFood;
            region.AvailableFood -= MetabolicRate;
        }
    }

    public void Metabolize(Region region, short movementAmount)
    {
        if (IsDead) return;

        var terrainFactor = SimulationConfig.BaseTerrainFactor;

        switch (region.Biome.TerrainType)
        {
            case TerrainType.Sea:
                if (this.Has<Legs>())
                {
                    terrainFactor = SimulationConfig.WaterTerrainFactor;
                }
                break;
            case TerrainType.Littoral:
            case TerrainType.Beach:
                break;
            case TerrainType.Grass:
                if (this.Has<Fins>())
                {
                    terrainFactor = SimulationConfig.GrassTerrainFactor;
                }
                break;
            case TerrainType.Forest:
                if (this.Has<Fins>())
                {
                    terrainFactor = SimulationConfig.ForestTerrainFactor;
                }
                break;
            case TerrainType.Mountain:
                if (this.Has<Fins>())
                {
                    terrainFactor = SimulationConfig.MountainTerrainFactor;
                }
                break;
        }

        // TODO: metabolize differently based on biome/terrain?
        // TODO: add a gene for base metabolic burn
        if (movementAmount == 0)
        { // even no movement uses energy
            Health -= (short)(SimulationConfig.IdleMetabolismMultiplier * terrainFactor);
        }
        else
        {
            Health -= (short)(movementAmount * terrainFactor * SimulationConfig.MovementMetabolismMultiplier);
        }

        if (IsDead)
        {
            DeathReason = PredatorPrey.DeathReason.Starvation;
        }
    }

    public short GetMovementSpeed(TerrainType terrain)
    {
        short baseMotion = 0;

        switch (terrain)
        {
            case TerrainType.Sea:
                baseMotion = SimulationConfig.SeaBaseMovement;
                break;
            case TerrainType.Littoral:
                baseMotion = SimulationConfig.LittoralBaseMovement;
                break;
            case TerrainType.Beach:
                baseMotion = SimulationConfig.BeachBaseMovement;
                break;
            case TerrainType.Grass:
                baseMotion = SimulationConfig.GrassBaseMovement;
                break;
            case TerrainType.Forest:
                baseMotion = SimulationConfig.ForestBaseMovement;
                break;
            case TerrainType.Mountain:
                baseMotion = SimulationConfig.MountainBaseMovement;
                break;
        }

        baseMotion = (short)Random.Shared.Next(0, baseMotion + 1);

        short maxMotion = 0;

        foreach (IMovementGene m in ChromosomeA.Genes)
        {
            var modifier = m.ModifyMovement(this, terrain, baseMotion);
            if (modifier > maxMotion) { maxMotion = modifier; }
        }

        return maxMotion;
    }
}
