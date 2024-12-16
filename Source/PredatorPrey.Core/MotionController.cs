namespace PredatorPrey;

public class MotionController
{
    public (int newX, int newY, short speed) Move(Organism organism, PopulationMap populationMap, World world)
    {
        var direction = Random.Shared.Next(0, 8);

        var location = populationMap.GetOrganismLocation(organism);

        if (location == null) return (0, 0, 0);

        short speed = organism.GetMovementSpeed(world.Regions[location.Value.X, location.Value.Y].Biome.TerrainType);

        int newX = location.Value.X;
        int newY = location.Value.Y;

        // 7 0 1
        // 6 * 2
        // 5 4 3
        switch (direction)
        {
            case 0:
                newY -= speed;
                break;
            case 1:
                newY -= speed;
                newX += speed;
                break;
            case 2:
                newX += speed;
                break;
            case 3:
                newX += speed;
                newY += speed;
                break;
            case 4:
                newY += speed;
                break;
            case 5:
                newX -= speed;
                newY += speed;
                break;
            case 6:
                newX -= speed;
                break;
            case 7:
                newX -= speed;
                newY -= speed;
                break;
        }

        if (newX < 0) newX = 0;
        if (newY < 0) newY = 0;
        if (newX > world.Dimensions.Width - 1) newX = world.Dimensions.Width - 1;
        if (newY > world.Dimensions.Height - 1) newY = world.Dimensions.Height - 1;

        // can the target location support the population?
        var existing = world.Population.GetOrganismsAtLocation(newX, newY).Count();
        var max = world.Regions[newX, newY].Biome.GetMaxPopulationCapacity();
        if (existing < max)
        {
            populationMap.Add(organism, newX, newY);
            return (newX, newY, speed);
        }

        // unable to move due to population density
        return (location.Value.X, location.Value.Y, 0);
    }
}
