namespace PredatorPrey;

public static class OrganismExtensions
{
    public static bool Has<T>(this Organism organism)
        where T : Gene
    {
        return organism.ChromosomeA.Genes.Any(g => g is T && g.Value > 0);
    }

    public static bool IsAcquatic(this Organism organism)
    {
        return organism.ChromosomeA.Genes.Any(g => g is Fins && g.Value > 0);
    }

    public static bool IsTerrestrial(this Organism organism)
    {
        return organism.ChromosomeA.Genes.Any(g => g is Legs && g.Value > 0);
    }

    public static bool IsAmphibian(this Organism organism)
    {
        return organism.IsAcquatic() && organism.IsTerrestrial();
    }
}
