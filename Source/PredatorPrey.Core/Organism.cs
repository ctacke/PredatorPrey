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

    public bool IsDead => Health == 0;
    public DeathReason? DeathReason
    {
        get => _deathReason;
        private set
        {
            if (_deathReason == null) _deathReason = value;
        }
    }

    /// <summary>
    /// The likelihood that the organism will reproduce when interacting with another (percentile)
    /// </summary>
    public float Fertility { get; set; } = 0.46f;

    /// <summary>
    /// rate at which food is consumed (and, perhaps, things like movement rate)
    /// </summary>
    public float MetabolicRate { get; set; } = 1.0f;

    public int MaxAge { get; set; } = 1000;

    /// <summary>
    /// How much "food" is this organism worth on death (as compost to the biome or as food to another organism)
    /// </summary>
    public float ValueAsFood { get; set; } = 0.5f;

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
        Health -= movementAmount;
        if (IsDead)
        {
            DeathReason = PredatorPrey.DeathReason.Starvation;
        }
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
