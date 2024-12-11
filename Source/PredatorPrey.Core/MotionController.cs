namespace PredatorPrey;

public class MotionController
{
    public short Move(Organism organism, PopulationMap populationMap, int worldWidth, int worldHeight)
    {
        var direction = Random.Shared.Next(0, 8);
        short speed = (short)Random.Shared.Next(0, 3);

        if (speed > 0)
        {
            var location = populationMap.GetOrganismLocation(organism);
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
            if (newX > worldWidth - 1) newX = worldWidth - 1;
            if (newY > worldHeight - 1) newY = worldHeight - 1;

            populationMap.Add(organism, newX, newY);
        }

        return speed;
    }
}
