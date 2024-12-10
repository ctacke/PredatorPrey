namespace PredatorPrey;

public class MotionController
{
    public void Move(Organism organism, PopulationMap populationMap, int worldWidth, int worldHeight)
    {
        var direction = Random.Shared.Next(0, 8);
        short speed = (short)Random.Shared.Next(0, 3);

        if (speed > 0)
        {
            int newX = organism.Region.Location.X;
            int newY = organism.Region.Location.Y;

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

        organism.Health -= speed;
    }
}

public class Organism
{
    private short _health = 0xff;

    public Guid ID { get; set; }
    public Guid ParentA { get; set; }
    public Guid ParentB { get; set; }

    public Chromosome ChromosomeA { get; set; }
    public Chromosome ChromosomeB { get; set; }

    public bool IsDead => Health == 0;

    public short Health
    {
        get => _health;
        set
        {
            if (_health == 0) return;
            if (value < 0) value = 0;
            _health = value;
        }
    }

    public Region Region { get; set; }
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
