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
                Age = Random.Shared.Next(0, 1000)
            };

            var xPosition = 0;
            var yPosition = 0;

            bool add = false;
            do
            {

                do
                {
                    xPosition = Random.Shared.Next(0, world.Dimensions.Width);
                    yPosition = Random.Shared.Next(0, world.Dimensions.Height);
                } while (world.Population.GetOrganismsAtLocation(xPosition, yPosition).Count() != 0);

                switch (world.Regions[xPosition, yPosition].Biome.TerrainType)
                {
                    case TerrainType.Sea:
                        if (organism.Has<Fins>())
                        {
                            add = true;
                        }
                        break;
                    case TerrainType.Grass:
                    case TerrainType.Forest:
                        if (organism.Has<Legs>())
                        {
                            add = true;
                        }
                        break;
                }

                if (add)
                {
                    world.Population.Add(organism, xPosition, yPosition);
                }
            } while (!add);
        }
    }

    public Organism? Reproduce(Organism parentA, Organism parentB, float adaptiveFertility)
    {
        if (parentA.CanReproduce && parentB.CanReproduce)
        {
            // Use adaptive fertility instead of organism fertility for reproduction chance
            if (Random.Shared.NextSingle() < (adaptiveFertility * adaptiveFertility))
            {
                return new Organism(parentA, parentB);
            }
        }

        return null;
    }
    
    // Keep backward compatibility with fixed fertility
    public Organism? Reproduce(Organism parentA, Organism parentB)
    {
        return Reproduce(parentA, parentB, SimulationConfig.BaseFertility);
    }
}
