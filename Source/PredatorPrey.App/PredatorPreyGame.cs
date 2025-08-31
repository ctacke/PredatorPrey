using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace PredatorPrey.App;

public class ViewOptions
{
    public bool ShowFood { get; set; }
}

public class PredatorPreyGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _pixelTexture;

    private const int SquareSize = 5;
    private const int Padding = 0;
    private const int TargetFPS = 30; // Limit simulation updates to 30 FPS

    private Rectangle[,] _gridRectangles;
    private Color[,] _regionColors;
    private bool _needsColorUpdate = true;

    private World _world;
    private ViewOptions _viewOptions;
    
    private double _lastUpdateTime = 0;
    private double _updateInterval = 1000.0 / TargetFPS; // milliseconds

    public PredatorPreyGame()
    {
        _viewOptions = new ViewOptions
        {
            ShowFood = false
        };

        _world = new World();

        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    private Color GetOrganismColor(Organism organism)
    {
        if (organism.IsAmphibian()) return Color.PaleGreen;
        if (organism.IsAcquatic()) return Color.White;
        return Color.DarkRed;
    }

    private void UpdateRegionColors()
    {
        if (!_needsColorUpdate) return;

        for (int row = 0; row < _world.Dimensions.Height; row++)
        {
            for (int col = 0; col < _world.Dimensions.Width; col++)
            {
                _regionColors[col, row] = GetRegionColor(_world.Regions[col, row]);
            }
        }
        _needsColorUpdate = false;
    }

    private Color GetRegionColor(Region region)
    {
        var regionOrganisms = _world.Population.GetOrganismsAtLocation(region.Location);

        if (regionOrganisms.Count() > 0)
        {
            return GetOrganismColor(regionOrganisms.First());
        }

        if (_viewOptions.ShowFood && region.AvailableFood >= 1)
        {
            return Color.Cyan;
        }

        return region.Biome.TerrainType switch
        {
            TerrainType.Sea => Color.MediumBlue,
            TerrainType.Littoral => Color.CornflowerBlue,
            TerrainType.Beach => Color.Bisque,
            TerrainType.Grass => Color.LimeGreen,
            TerrainType.Forest => Color.ForestGreen,
            TerrainType.Mountain => Color.SaddleBrown,
            _ => Color.Magenta
        };
    }

    protected override void Initialize()
    {
        // Set window size
        _graphics.PreferredBackBufferWidth = (SquareSize + Padding) * _world.Dimensions.Width;
        _graphics.PreferredBackBufferHeight = (SquareSize + Padding) * _world.Dimensions.Height;
        _graphics.ApplyChanges();

        // Initialize color and rectangle arrays
        _gridRectangles = new Rectangle[_world.Dimensions.Width, _world.Dimensions.Height];
        _regionColors = new Color[_world.Dimensions.Width, _world.Dimensions.Height];

        // Populate with random colors and positions
        Random random = new Random();
        for (int row = 0; row < _world.Dimensions.Height; row++)
        {
            for (int col = 0; col < _world.Dimensions.Width; col++)
            {
                _gridRectangles[col, row] = new Rectangle(
                    col * (SquareSize + Padding),
                    row * (SquareSize + Padding),
                    SquareSize,
                    SquareSize
                );
            }
        }

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Create reusable pixel texture
        _pixelTexture = new Texture2D(GraphicsDevice, 1, 1);
        _pixelTexture.SetData(new[] { Color.White });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Throttle simulation updates to improve performance
        if (gameTime.TotalGameTime.TotalMilliseconds - _lastUpdateTime >= _updateInterval)
        {
            _world.RunCycle();
            _needsColorUpdate = true;
            _lastUpdateTime = gameTime.TotalGameTime.TotalMilliseconds;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // Update colors only when simulation has changed
        UpdateRegionColors();

        _spriteBatch.Begin();

        // Draw cached colored squares
        for (int row = 0; row < _world.Dimensions.Height; row++)
        {
            for (int col = 0; col < _world.Dimensions.Width; col++)
            {
                _spriteBatch.Draw(
                    _pixelTexture,
                    _gridRectangles[col, row],
                    _regionColors[col, row]
                );
            }
        }

        _spriteBatch.End();

        // Update window title with statistics (less frequently)
        if (_needsColorUpdate || gameTime.TotalGameTime.TotalMilliseconds % 500 < 16) // Update title every ~500ms
        {
            Window.Title = $"Predator-Prey Simulation - Gen: {_world.WorldAge} | Pop: {_world.Population.Population} | Deaths: Age({_world.DeathsFromAge}) Starv({_world.DeathsFromStarvation}) Over({_world.DeathsFromOverpopulation})";
        }

        base.Draw(gameTime);
    }
}
