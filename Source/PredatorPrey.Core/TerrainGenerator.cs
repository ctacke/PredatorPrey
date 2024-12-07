using System.Drawing;

namespace PredatorPrey;

public class TerrainGenerator
{
    private int _width;
    private int _height;

    // Biome seed points
    public TerrainGenerator(int width, int height)
    {
        _width = width;
        _height = height;
    }

    private TerrainType ModifyContinentTerrain(float distance, TerrainType existing)
    {
        // add noise
        distance += (Random.Shared.NextSingle() * 0.10f) - 0.05f;

        // Gradual terrain transition
        var modified = distance switch
        {
            < 0.02f => existing + 1,
            < 0.6f => existing,
            < 1.9f => existing - 1,
            _ => existing - 2
        };

        if (modified < 0) modified = 0;
        if (modified > TerrainType.Mountain) modified = TerrainType.Mountain;

        return modified;
    }

    private TerrainType GenerateContinentTerrain(float distance)
    {
        // add noise
        distance += (Random.Shared.NextSingle() * 0.06f) - 0.03f;

        // Gradual terrain transition
        return distance switch
        {
            < 0.1f => TerrainType.Mountain,
            < 0.3f => TerrainType.Forest,
            < 0.4f => TerrainType.Grass,
            < 0.6f => TerrainType.Beach,
            < 1.0f => TerrainType.Littoral,
            _ => TerrainType.Sea
        };
    }

    public Region[,] Generate(double foodDistribution)
    {
        var regions = new Region[_width, _height];

        // fill with water
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                regions[x, y] = new Region
                {
                    Location = new Point(x, y),
                    Biome = new Biome(TerrainType.Sea)
                };

                if (Random.Shared.NextSingle() <= foodDistribution)
                {
                    regions[x, y].AvailableFood = 1;
                }
            }
        }

        // Random starting point
        int startX = Random.Shared.Next(_width);
        int startY = Random.Shared.Next(_height);

        // Continent size and shape
        int continentSize = Random.Shared.Next(_width / 2, _width / 2);
        float shapeFactor = (float)Random.Shared.NextDouble() * 0.35f + 0.65f;

        for (var c = 0; c < 20; c++)
        {

            // Generate continent terrain
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    // Distance calculation with irregular shape
                    float distanceX = Math.Abs(x - startX) / (float)continentSize;
                    float distanceY = Math.Abs(y - startY) / (float)continentSize;
                    float distance = (float)Math.Sqrt(
                        Math.Pow(distanceX, 2) * shapeFactor +
                        Math.Pow(distanceY, 2) * (1 - shapeFactor)
                    );

                    // Continent generation with gradual terrain transitions
                    if (distance < 1)
                    {
                        if (c == 0)
                        {
                            regions[x, y].Biome.TerrainType = GenerateContinentTerrain(distance);
                        }
                        else
                        {
                            var current = regions[x, y].Biome.TerrainType;
                            regions[x, y].Biome.TerrainType = ModifyContinentTerrain(distance, current);
                        }

                    }
                }
            }

            startX += (int)((Random.Shared.NextSingle() * 0.05f * _width) - 0.025 * _width);
            startY += (int)((Random.Shared.NextSingle() * 0.05f * _height) - 0.025 * _height);
        }

        return regions;
    }
}
/*
public static class TerrainGenerator
{

    private static float GenerateSimpleNoise(int x, int y)
    {
        // Very basic noise generation
        return (float)(Math.Sin(x * 0.1) + Math.Cos(y * 0.1) + 2) / 4;
    }

    public static Region[,] Generate(int width, int height)
    {
        var regions = new Region[width, height];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                float noiseValue = GenerateSimpleNoise(x, y);

                Terrain terrain = noiseValue switch
                {
                    < 0.2f => new Sea(),
                    < 0.3f => new Littoral(),
                    < 0.4f => new Beach(),
                    < 0.6f => new Grass(),
                    < 0.8f => new Forest(),
                    _ => new Mountain()
                };

                regions[x, y] = new Region
                {
                    Location = new Point(x, y),
                    Terrain = terrain
                };
            }
        }

        return regions;
    }

    public static Region[,] Generate2(int width, int height)
    {
        var regions = new Region[width, height];

        // fill with water
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                regions[x, y] = new Region
                {
                    Location = new Point(x, y),
                    Terrain = new Sea()
                };
            }
        }

        // seed a mountain

        // create forests

        // create grasslands

        // create beach

        // create littoral

        return regions;

    }
}
*/