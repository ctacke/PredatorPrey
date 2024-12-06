
internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new PredatorPrey.App.PredatorPreyGame();
        game.Run();
    }
}