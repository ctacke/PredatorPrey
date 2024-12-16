namespace PredatorPrey;

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

    protected override Gene Create(byte value, TraitType traitType)
    {
        return new Legs
        {
            Value = value,
            TraitType = traitType
        };
    }
}
