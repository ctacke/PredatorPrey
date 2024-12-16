namespace PredatorPrey;

public interface IMovementGene
{
    short ModifyMovement(Organism organism, TerrainType terrain, short baseMovement);
}
