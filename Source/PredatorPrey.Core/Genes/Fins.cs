namespace PredatorPrey;

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

    protected override Gene Create(byte value, TraitType traitType)
    {
        return new Fins
        {
            Value = value,
            TraitType = traitType
        };
    }
}
