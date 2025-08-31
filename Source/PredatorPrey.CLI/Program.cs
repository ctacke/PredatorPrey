using PredatorPrey;

namespace PredatorPrey.CLI;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Predator-Prey Simulation CLI");
        Console.WriteLine("============================");
        Console.WriteLine("Press Ctrl+C to exit");
        Console.WriteLine();

        var world = new World();
        int cycleCount = 0;
        var startTime = DateTime.Now;

        Console.WriteLine($"Initial population: {world.Population.Population}");
        Console.WriteLine($"World dimensions: {world.Dimensions.Width}x{world.Dimensions.Height}");
        Console.WriteLine();

        // Set up Ctrl+C handler for graceful exit
        Console.CancelKeyPress += (sender, e) =>
        {
            var elapsed = DateTime.Now - startTime;
            Console.WriteLine();
            Console.WriteLine("=== Final Statistics ===");
            Console.WriteLine($"Total cycles: {cycleCount}");
            Console.WriteLine($"Total time: {elapsed.TotalSeconds:F1} seconds");
            Console.WriteLine($"Cycles per second: {cycleCount / elapsed.TotalSeconds:F1}");
            Console.WriteLine($"Final generation: {world.WorldAge}");
            Console.WriteLine($"Final population: {world.Population.Population}");
            Console.WriteLine($"Deaths from age: {world.DeathsFromAge}");
            Console.WriteLine($"Deaths from starvation: {world.DeathsFromStarvation}");
            Console.WriteLine($"Deaths from overpopulation: {world.DeathsFromOverpopulation}");
            Environment.Exit(0);
        };

        // Run simulation loop
        while (true)
        {
            world.RunCycle();
            cycleCount++;

            // Output statistics every 100 cycles
            if (cycleCount % 100 == 0)
            {
                var elapsed = DateTime.Now - startTime;
                var cyclesPerSec = cycleCount / elapsed.TotalSeconds;
                
                Console.WriteLine($"Cycle {world.WorldAge,6} | Pop: {world.Population.Population,6} | " +
                                $"Deaths: Age({world.DeathsFromAge,4}) Starv({world.DeathsFromStarvation,4}) Over({world.DeathsFromOverpopulation,4}) | " +
                                $"Rate: {cyclesPerSec:F1} c/s");

                // Check for extinction
                if (world.Population.Population == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("=== EXTINCTION EVENT ===");
                    Console.WriteLine($"All organisms died after {world.WorldAge} generations");
                    Console.WriteLine($"Primary death causes:");
                    Console.WriteLine($"  Age: {world.DeathsFromAge}");
                    Console.WriteLine($"  Starvation: {world.DeathsFromStarvation}");
                    Console.WriteLine($"  Overpopulation: {world.DeathsFromOverpopulation}");
                    break;
                }

                // Check for population explosion
                if (world.Population.Population > 50000)
                {
                    Console.WriteLine();
                    Console.WriteLine("=== POPULATION EXPLOSION ===");
                    Console.WriteLine($"Population exceeded 50,000 organisms");
                    Console.WriteLine("Simulation parameters may need adjustment");
                    break;
                }
            }

            // Brief pause to prevent CPU overload while still running fast
            if (cycleCount % 1000 == 0)
            {
                Thread.Sleep(1);
            }
        }

        Console.WriteLine();
        Console.WriteLine("Simulation ended. Press any key to exit...");
        Console.ReadKey();
    }
}