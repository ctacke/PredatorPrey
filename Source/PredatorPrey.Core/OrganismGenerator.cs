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
            } while (world.Population.GetOrganismsAtLocation(xPosition, yPosition).Count() != 0);

            world.Population.Add(organism, xPosition, yPosition);
            organism.Region = world.Regions[xPosition, yPosition];
        }
    }
}
