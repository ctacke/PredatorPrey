namespace PredatorPrey;

public class OrganismGenerator()
{
    public void Populate(World world, int population)
    {
        for (var i = 0; i < population; i++)
        {
            var organism = new Organism
            {
                ID = Guid.NewGuid(),
                ParentA = Guid.Empty,
                ParentB = Guid.Empty,
            };

            var xPosition = 0;
            var yPosition = 0;

            do
            {
                xPosition = Random.Shared.Next(0, world.Dimensions.Width);
                yPosition = Random.Shared.Next(0, world.Dimensions.Height);
            } while (world.Regions[xPosition, yPosition].Organisms.Count != 0);

            world.Regions[xPosition, yPosition].Organisms.Add(organism);
            organism.Region = world.Regions[xPosition, yPosition];
        }
    }
}

public class MotionController
{
    public void Move(Organism organism, World world)
    {
        var direction = Random.Shared.Next(0, 8);
        short speed = (short)Random.Shared.Next(0, 3);

        if (speed > 0)
        {
            int newX = organism.Region.Location.X;
            int newY = organism.Region.Location.Y;

            world.Regions[newX, newY].Organisms.Remove(organism);

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

            world.Regions[newX, newY].Organisms.Add(organism);
            organism.Region = world.Regions[newX, newY];
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
