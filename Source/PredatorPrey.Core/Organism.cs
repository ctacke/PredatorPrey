namespace PredatorPrey;

public enum DeathReason
{
    Starvation,
    Age,
    Predation,
    Disease
}

public class Organism
{
    private short _health = 0xff;
    private int _age;
    private DeathReason? _deathReason;

    public Guid ID { get; set; }
    public Guid ParentA { get; set; }
    public Guid ParentB { get; set; }

    public Chromosome ChromosomeA { get; set; }
    public Chromosome ChromosomeB { get; set; }

    public int MaxAge { get; set; } = 1000;
    public int MinReproductionAge { get; set; } = 100;
    /// <summary>
    /// The likelihood that the organism will reproduce when interacting with another (percentile)
    /// </summary>
    public float Fertility { get; set; } = 0.16f;

    /// <summary>
    /// rate at which food is consumed (and, perhaps, things like movement rate)
    /// </summary>
    public float MetabolicRate { get; set; } = 1.0f;

    /// <summary>
    /// How much "food" is this organism worth on death (as compost to the biome or as food to another organism)
    /// </summary>
    public float ValueAsFood { get; set; } = 0.5f;

    public bool IsDead => Health == 0;

    public Organism()
    {
        ChromosomeA = new Chromosome
        {
            Genes =
             [
                new Legs { Value = 0 },
                new Fins { Value = 1 }
            ]
        };
    }

    public bool CanReproduce
    {
        get
        {
            return Age >= MinReproductionAge;
        }
    }

    public DeathReason? DeathReason
    {
        get => _deathReason;
        private set
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

        if (region.AvailableFood >= 1)
        {
            // TODO: make organism health rate increase variable
            Health += 20;
            region.AvailableFood -= MetabolicRate;
        }
    }

    public void Metabolize(Region region, short movementAmount)
    {
        if (IsDead) return;

        // TODO: metabolize differently based on biome/terrain?
        // TODO: add a gene for base metabolic burn
        if (movementAmount == 0)
        { // even no movement uses energy
            Health--;
        }
        else
        {
            Health -= movementAmount;
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
                baseMotion = 3;
                break;
            case TerrainType.Littoral:
                baseMotion = 2;
                break;
            case TerrainType.Beach:
                baseMotion = 2;
                break;
            case TerrainType.Grass:
                baseMotion = 3;
                break;
            case TerrainType.Forest:
                baseMotion = 2;
                break;
            case TerrainType.Mountain:
                baseMotion = 1;
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

public interface IMovementGene
{
    short ModifyMovement(Organism organism, TerrainType terrain, short baseMovement);
}

public class Fins : Gene, IMovementGene
{
    // 0 or 1 (missing or present)
    public Fins()
    {
        TraitType = TraitType.Dominant;
        Value = 0;
    }

    public short ModifyMovement(Organism organism, TerrainType terrain, short baseMovement)
    {
        switch (terrain)
        {
            case TerrainType.Sea:
            case TerrainType.Littoral:
                if (Value > 0) return baseMovement;
                break;
        }

        return 0;
    }
}

public class Legs : Gene, IMovementGene
{
    // 0 or 1 (missing or present)
    public Legs()
    {
        TraitType = TraitType.Dominant;
        Value = 0;
    }

    public short ModifyMovement(Organism organism, TerrainType terrain, short baseMovement)
    {
        switch (terrain)
        {
            case TerrainType.Beach:
            case TerrainType.Grass:
            case TerrainType.Forest:
            case TerrainType.Mountain:
                if (Value > 0) return baseMovement;
                break;
        }

        return 0;
    }
}

public class Chromosome
{
    public Gene[] Genes { get; set; }
}

public enum TraitType
{
    Dominant,
    Recessive
}

public class Gene
{
    public byte Value { get; set; }
    public TraitType TraitType { get; set; }
}
