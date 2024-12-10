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

    private const int SquareSize = 5;
    private const int Padding = 0;

    private Rectangle[,] _gridRectangles;

    private World _world;

    private ViewOptions _viewOptions;

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

    private Color GetRegionColor(Region region)
    {
        if (_world.Population.GetOrganismsAtLocation(region.Location).Count() > 0)
        {
            return Color.DarkRed;
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

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Handle mouse input
        MouseState mouseState = Mouse.GetState();

        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            for (int row = 0; row < _world.Dimensions.Height; row++)
            {
                for (int col = 0; col < _world.Dimensions.Width; col++)
                {
                    // Check if mouse is over a square
                    if (_gridRectangles[col, row].Contains(mouseState.Position))
                    {
                        // TODO: REGION CLICKED
                        break;
                    }
                }
            }
        }

        _world.RunCycle();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw colored squares
        Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
        pixel.SetData(new[] { Color.White });

        for (int row = 0; row < _world.Dimensions.Height; row++)
        {
            for (int col = 0; col < _world.Dimensions.Width; col++)
            {
                _spriteBatch.Draw(
                    pixel,
                    _gridRectangles[col, row],
                    GetRegionColor(_world.Regions[col, row])
                );
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
