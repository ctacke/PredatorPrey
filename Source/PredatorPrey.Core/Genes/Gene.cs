namespace PredatorPrey;

public abstract class Gene
{
    public byte Value { get; set; }
    public TraitType TraitType { get; set; }

    protected abstract Gene Create(byte value, TraitType traitType);

    public virtual Gene Crossover(Gene other)
    {
        var newValue = (byte)Math.Ceiling((this.Value + other.Value) / 2f);

        // add some randomization
        if (Random.Shared.NextSingle() < 0.005)
        {
            if (newValue > 0) newValue = 0;
            else newValue = 1;
        }

        return Create(newValue, this.TraitType);
    }
}
