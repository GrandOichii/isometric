using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;
using Roy_T.AStar.Paths;
using System;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using Roy_T.AStar.Graphs;
using System.Reflection;
using System.Linq.Expressions;

using Isometric.Layout;
using Isometric.Entities;

using NLua;

// namespace Isometric {
    // enum TileType : int
    // {
    //     Floor = '.',
    //     Selected = '+',
    //     Wall = '*',
    //     Entrance = 'o',
    //     Player = '@',
    //     Enemy = 'Z',
    //     Detection = '!',
    // }

    // abstract class Entity
    // {
    //     private int x;
    //     private int y;

    //     protected TileType type;


    //     public int X { get => x; set => x = value; }
    //     public int Y { get => y; set => y = value; }
    //     public TileType Type { get => type; }

    //     protected Entity(int x, int y, TileType t)
    //     {
    //         this.x = x;
    //         this.y = y;
    //         this.type = t;
    //     }
    // }

    // class PlayerEntity : Entity
    // {
    //     public PlayerEntity(int x, int y) : base(x, y, TileType.Player) { }
    // }

    // class EnemyEntity : Entity
    // {
    //     private int detectionRadius;
    //     public int DetectionRadius {get => detectionRadius; }
    //     public EnemyEntity(int x, int y, int detectionRadius) : base(x, y, TileType.Enemy) { 
    //         this.detectionRadius = detectionRadius;
    //     }
    // }

    // class Tile1
    // {
    //     static readonly TileType[] highTiles = { TileType.Wall };
    //     static bool IsHighTile(TileType t)
    //     {
    //         return false;
    //         return highTiles.Contains(t);
    //     }

    //     public static int Height = 64;
    //     public static int Width = 64;
    //     private TileType type;

    //     private bool isHigh;
    //     public Tile1(TileType t)
    //     {
    //         Type = t;
    //         IsHigh = IsHighTile(t);
            
    //     }

    //     public bool IsHigh { get => isHigh; set => isHigh = value; }
    //     internal TileType Type { get => type; set => type = value; }
    //     public bool IntersectsMouse(int i, int ii, int xOffset, int yOffset)
    //     {
    //         int xLoc = (int)(ii * Width * 0.5 - i * Width * 0.5);
    //         int yLoc = (int)(ii * Height * 0.25 + i * Height * 0.25);
    //         var mState = Mouse.GetState();
    //         var mX = mState.X - xOffset;
    //         var mY = mState.Y - 20 - yOffset;
    //         return mState.LeftButton == ButtonState.Pressed && !isHigh && mX >= xLoc && mY >= yLoc && mX <= xLoc + Width && mY <= yLoc + Height;
    //     }

    // }

    // class Layout1
    // {
    //     //static Random rnd = new Random();
    //     static Random rnd = new Random(1);

    //     Tile1[][] tiles;

    //     // pathfinding
    //     Grid? grid;
    //     PathFinder pathFinder = new PathFinder();

    //     static int iOffset = -4;
    //     static int iiOffset = 6;


    //     private void CreatePathfinding()
    //     {
    //         var gridSize = new GridSize(tiles[0].Length, tiles.Length);
    //         var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
    //         var traversalVelocity = Velocity.FromKilometersPerHour(100);
    //         grid = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, traversalVelocity);

    //         for (int i = 0; i < tiles.Length; i++)
    //         {
    //             for (int ii = 0; ii < tiles[i].Length; ii++)
    //             {
    //                 if (tiles[i][ii].Type == TileType.Wall) grid.DisconnectNode(new GridPosition(ii, i));
    //             }

    //         }
    //     }

    //     public Layout1(char[][] map)
    //     {
    //         //new Roy_T.AStar.Paths.PathFinder()
    //         tiles = new Tile1[map.Length][];
    //         for (int i = 0; i < map.Length; i++)
    //         {
    //             tiles[i] = new Tile1[map[i].Length];
    //             for (int ii = 0; ii < map[i].Length; ii++)
    //             {
    //                 var type = (TileType)map[i][ii];
    //                 tiles[i][ii] = new Tile1(type);

    //             }
    //         }
    //         CreatePathfinding();
    //     }

    //     public Layout1(int height, int width, out EnemyEntity[] enemies)
    //     {
    //         // generate outer walls
    //         tiles = new Tile1[height][];
    //         for (int i = 0; i < height; i++)
    //         {
    //             tiles[i] = new Tile1[width];
    //             for (int ii = 0; ii < width; ii++)
    //             {

    //                 var type = i == 0 || ii == 0 || i == height - 1 || ii == width - 1 ? TileType.Wall : TileType.Floor;
    //                 tiles[i][ii] = new Tile1(type);

    //             }
    //         }

    //         // generate entrance room
    //         var eX = rnd.Next(1, width-1);
    //         var eY = rnd.Next(1, height-1);
    //         var startRoomH = 5;
    //         var startRoomW = 5;
    //         createRoom(eX - startRoomW / 2, eY - startRoomH / 2, startRoomW, startRoomH);

    //         // generate hallway
    //         int hallLength = rnd.Next(7, 20);
    //         // bool created = false;
    //         foreach (Direction dir in Enum.GetValues(typeof(Direction)))
    //         {
    //             if (createHallway(eX, eY, dir, hallLength, 3))
    //             {
    //                 // created = true;
    //                 break;
    //             }
    //         }
    //         putTile(eX, eY, TileType.Entrance);


    //         // generate enemies
    //         int enemyCount = rnd.Next(2, 5);
    //         enemies = new EnemyEntity[enemyCount];
    //         int t = width / enemyCount;
    //         for (int i = 0 ; i < enemyCount; i++) {
    //             int x = i * t + rnd.Next(t / 2);
    //             int y = rnd.Next(height / 2);
    //             enemies[i] = new(x, y, rnd.Next(4, 10));
    //         }

    //         //int roomSize
    //         //createHallway(eX-startRoomW/2, eY, Direction.West, 11, 3);
    //         //createHallway(eX, eY - startRoomH / 2, Direction.North, 11, 3);

    //         CreatePathfinding();
    //     }

    //     public Vector2 getEntrance()
    //     {
    //         for (int i = 0; i < tiles.Length; i++)
    //         {
    //             for (int ii = 0; ii < tiles[i].Length; ii++)
    //             {
    //                 if (tiles[i][ii].Type == TileType.Entrance) return new Vector2(ii, i);
    //             }
    //         }
    //         throw new Exception("Entrance not found");
    //     }

    //     private bool putTile(int x, int y, TileType t)
    //     {
    //         if (x < 0 || y < 0 || y >= tiles.Length || x >= tiles[0].Length) return false;
    //         tiles[y][x] = new Tile1(t);
    //         return true;
    //     }

    //     private bool createRoom(int x, int y, int width, int height)
    //     {
    //         if (x < 0 || y < 0 || x + width >= tiles[0].Length || y + height >= tiles.Length) return false;
    //         for (int i = 0; i < width; i++)
    //         {
    //             for (int ii = 0; ii < height; ii++)
    //             {
    //                 if (i != 0 && ii != 0 && i != width-1 && ii != height-1) continue;
    //                 putTile(x + i, y + ii, TileType.Wall);
    //             }
    //         }
    //         return true;
    //     }

    //     enum Direction : int
    //     {
    //         North = 0,
    //         South,
    //         West,
    //         East
    //     }

    //     private bool createHallway(int x, int y, Direction dir, int length, int width)
    //     {
    //         int hw = width / 2 + 1;
    //         int xDiff = 0;
    //         int yDiff = 0;
    //         switch (dir)
    //         {
    //             case Direction.North:
    //                 xDiff = 0;
    //                 yDiff = -1;
    //                 break;
    //             case Direction.South:
    //                 xDiff = 0;
    //                 yDiff = 1;
    //                 break;
    //             case Direction.West:
    //                 xDiff = -1;
    //                 yDiff = 0;
    //                 break;
    //             case Direction.East:
    //                 xDiff = 1;
    //                 yDiff = 0;
    //                 break;
    //         }
    //         if (x < 0 || y < 0 || x >= tiles[0].Length || y >= tiles.Length || x + xDiff * length >= tiles[0].Length || y + yDiff * length >= tiles.Length || x + xDiff * length < 0 || y + yDiff * length < 0) return false;
    //         for (int i = 0; i < length; i++)
    //         {
    //             for (int ii = 1; ii <= width/2+1; ii++)
    //             {

    //                 putTile(x + (hw - ii) * yDiff, y + (hw - ii) * xDiff, TileType.Floor); ;
    //                 putTile(x + (hw - ii) * -yDiff, y + (hw - ii) * -xDiff, TileType.Floor);
    //             }
    //             putTile(x + hw * yDiff, y + hw * xDiff, TileType.Wall);
    //             putTile(x + hw * -yDiff, y + hw * -xDiff, TileType.Wall);
    //             x += xDiff;
    //             y += yDiff;
    //         }
    //         return true;
    //     }

    //     public Roy_T.AStar.Paths.Path? Draw(SpriteBatch sb, Dictionary<TileType, Texture2D> tileMap, int xOffset, int yOffset, Entity[] entities)
    //     {
    //         int selectedI = -1;
    //         int selectedII = -1;
    //         for (int i = 0; i < tiles.Length; i++)
    //         {
    //             for (int ii = 0; ii < tiles[i].Length; ii++)
    //             {
    //                 if (!tiles[i][ii].IntersectsMouse(i + iOffset, ii + iiOffset, xOffset, yOffset)) continue;
    //                 selectedI = i;
    //                 selectedII = ii;
    //             }
    //         }
    //         List<Vector2> points = new();
    //         Roy_T.AStar.Paths.Path? result = null;
    //         var player = entities[0];
    //         if (selectedI != -1)
    //         {
    //             result = pathFinder.FindPath(new GridPosition(player.X, player.Y), new GridPosition(selectedII, selectedI), grid);
    //             foreach (var edge in result.Edges)
    //             {
    //                 int x = (int)edge.End.Position.X;
    //                 int y = (int)edge.End.Position.Y;
    //                 points.Add(new(x, y));
    //             }
    //             if (result.Edges.Count == 0) result = null;
    //         }
            
    //         // draw base layer
    //         for (int i = 0; i < tiles.Length; i++)
    //         {
    //             for (int ii = 0; ii < tiles[i].Length; ii++)
    //             {
    //                 var tex = points.Contains(new(ii, i)) ? tileMap[TileType.Selected] : tileMap[tiles[i][ii].Type];
    //                 DrawTile(sb, tex, i + iOffset, ii + iiOffset, xOffset, yOffset);
    //             }

    //         }
    //         // DrawDetectionCircles(sb, tileMap[TileType.Detection], entities, xOffset, yOffset);

    //         // draw entities
    //         foreach (Entity en in entities) {
    //             var x = en.X;
    //             var y = en.Y;
    //             DrawTile(sb, tileMap[en.Type], y + iOffset - 1, x + iiOffset - 1, xOffset, yOffset);
    //         }


    //         // draw wall layer
    //         for (int i = 0; i < tiles.Length; i++)
    //         {
    //             for (int ii = 0; ii < tiles[i].Length; ii++)
    //             {
    //                 if (!tiles[i][ii].IsHigh) continue;
    //                 var tex = points.Contains(new(ii, i)) ? tileMap[TileType.Selected] : tileMap[tiles[i][ii].Type];
    //                 DrawTile(sb, tex, i + iOffset - 1, ii + iiOffset - 1, xOffset, yOffset, true);
    //             }

    //         }

    //         return result;
    //     }
    //     private void DrawDetectionCircles(SpriteBatch sb, Texture2D tex, Entity[] entities, int xOffset, int yOffset) {
    //         foreach (var en in entities) {
    //             if (!(en is EnemyEntity)) continue;
    //             var e = (EnemyEntity)en;
    //             var dr = e.DetectionRadius;
    //             for (int i = 0; i < dr*2; i++) {
    //                 for (int ii = 0; ii < dr*2; ii++) {
    //                     var x = e.X - dr + ii;
    //                     var y = e.Y - dr + i;
    //                     if (y == e.Y && x == e.X) continue;
    //                     if (x < 0 || y < 0 || x >= tiles[i].Length || y >= tiles.Length) continue;
    //                     if (Math.Sqrt(Math.Pow(e.X - x, 2) + Math.Pow(e.Y - y, 2)) >= dr) continue;
    //                     DrawTile(sb, tex, y + iOffset - 1, x + iiOffset - 1, xOffset, yOffset, true);
    //                 }
    //             }
    //         }
    //     }

    //     private void DrawTile(SpriteBatch sb, Texture2D tex, int i, int ii, int xOffset, int yOffset, bool transparent=false) {
    //         int xLoc = (int)(ii * Tile1.Width * 0.5 - i * Tile1.Width * 0.5);
    //         int yLoc = (int)(ii * Tile1.Height * 0.25  + i * Tile1.Height * 0.25);
    //         var vec = new Vector2(xLoc + xOffset, yLoc + yOffset);
    //         sb.Draw(tex, vec, Color.White * (transparent ? .5f : 1));
    //     }
    // }


enum TilesetEnum {
    Floor,
    Wall,
    Glass,
}

public class SpriteCreator {
    private int _tileWidth;
    private int _tileHeight;
    GraphicsDevice _graphics;
    
    public SpriteCreator(int tileWidth, int tileHeight, GraphicsDevice graphics) {
        _tileWidth = tileWidth;
        _tileHeight = tileHeight;
        _graphics = graphics;

        ErrorTex = new Texture2D(_graphics, _tileWidth, _tileHeight * 3 / 2);
        Color[] data = new Color[_tileHeight * _tileWidth * 3 / 2];
        for (int i = 0; i < _tileHeight * _tileWidth * 3 / 2; i++)
            data[i] = Color.Red;
        ErrorTex.SetData(data);

        ShadowTex = new Texture2D(_graphics, _tileHeight, _tileWidth);
        Color[] sdata = new Color[_tileHeight * _tileWidth];
        DrawSurface(sdata, Color.Black * .3f);
        ShadowTex.SetData(sdata);
    }

    public void DrawSurface(Color[] data, Color c) {
        int centerX = _tileWidth / 2;
        int topCenterX = centerX;
        int rightX = _tileWidth - 1;
        int leftX = 0;

        int midY1 = _tileHeight / 4;
        int centerY = _tileHeight / 2;
        int midY2 = _tileHeight * 3 / 4;
        int lowY = _tileHeight - 1;

        DrawLine(data, leftX, midY2, centerX, lowY, c);
        DrawLine(data, centerX, lowY, rightX, midY2, c);
        DrawLine(data, rightX, midY2, centerX, centerY, c);
        DrawLine(data, centerX, centerY, leftX, midY2, c);
        Fill(data, c, c);
    }

    public void DrawPoint(Color[] data, int x, int y, Color c) {
        data[x * _tileWidth + y] = c;
    }

    public void DrawLine(Color[] data, int y1, int x1, int y2, int x2, Color c) {
        int x, y, dx, dy, dx1, dy1, px, py, xe, ye, i;

        // Calculate line deltas
        dx = x2 - x1;
        dy = y2 - y1;
        // Create a positive copy of deltas (makes iterating easier)
        dx1 = Math.Abs(dx);
        dy1 = Math.Abs(dy);
        // Calculate error intervals for both axis
        px = 2 * dy1 - dx1;
        py = 2 * dx1 - dy1;
        // The line is X-axis dominant
        if (dy1 <= dx1)
        {
            // Line is drawn left to right
            if (dx >= 0)
            {
                x = x1; y = y1; xe = x2;
            }
            else
            { // Line is drawn right to left (swap ends)
                x = x2; y = y2; xe = x1;
            }
            DrawPoint(data, x, y, c); // Draw first pixel
                                // Rasterize the line
            for (i = 0; x < xe; i++)
            {
                x = x + 1;
                // Deal with octants...
                if (px < 0)
                {
                    px = px + 2 * dy1;
                }
                else
                {
                    if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0))
                    {
                        y = y + 1;
                    }
                    else
                    {
                        y = y - 1;
                    }
                    px = px + 2 * (dy1 - dx1);
                }
                // Draw pixel from line span at
                // currently rasterized position
                DrawPoint(data, x, y, c);
            }
        }
        else
        { // The line is Y-axis dominant
            // Line is drawn bottom to top
            if (dy >= 0)
            {
                x = x1; y = y1; ye = y2;
            }
            else
            { // Line is drawn top to bottom
                x = x2; y = y2; ye = y1;
            }
            DrawPoint(data, x, y, c); // Draw first pixel
                                // Rasterize the line
            for (i = 0; y < ye; i++)
            {
                y = y + 1;
                // Deal with octants...
                if (py <= 0)
                {
                    py = py + 2 * dx1;
                }
                else
                {
                    if ((dx < 0 && dy < 0) || (dx > 0 && dy > 0))
                    {
                        x = x + 1;
                    }
                    else
                    {
                        x = x - 1;
                    }
                    py = py + 2 * (dx1 - dy1);
                }
                // Draw pixel from line span at
                // currently rasterized position
                DrawPoint(data, x, y, c);
            }
        }
    }

    public void Fill(Color[] data, Color outlineColor, Color fillColor) {
        for (int i = 0; i < _tileHeight; i++)
        {
            int c = 0;
            for (int ii = 0; ii < _tileWidth; ii++) {
                if (data[ii * _tileWidth + i] == outlineColor) c++;
            }
            if (c < 2) continue;
            c = 0;
            for (int ii = 0; ii < _tileWidth; ii++)
            {
                if (data[ii * _tileWidth + i] == outlineColor) {
                    ++c;
                    continue;
                }
                if (c == 1) DrawPoint(data, ii, i, fillColor);
            }
        }
    }

    public Texture2D CreateIsoCube(Color outlineColor, Color fillColor) {
        return CreateIsoCube(outlineColor, fillColor, fillColor, fillColor);
    }

    public Texture2D CreateIsoCube(Color outlineColor, Color topColor, Color leftColor, Color rightColor) {
        Color[] data1 = new Color[_tileWidth * _tileHeight];
        Color[] data2 = new Color[_tileWidth * _tileHeight];
        Color[] data3 = new Color[_tileWidth * _tileHeight];

        Texture2D result = new(_graphics, _tileWidth, _tileHeight);

        int centerX = _tileWidth / 2;
        int topCenterX = centerX;
        int rightX = _tileWidth - 1;
        int leftX = 0;

        int highY = 0;
        int midY1 = _tileHeight / 4;
        int centerY = _tileHeight / 2;
        int midY2 = _tileHeight * 3 / 4;
        int lowY = _tileHeight - 1;

        DrawLine(data1, topCenterX, highY, rightX, midY1, outlineColor);
        DrawLine(data1, leftX, midY1, topCenterX, highY, outlineColor);
        DrawLine(data1, centerX, centerY, leftX, midY1, outlineColor);
        DrawLine(data1, centerX, centerY, rightX, midY1, outlineColor);
        Fill(data1, outlineColor, topColor);

        DrawLine(data2, centerX, centerY, leftX, midY1, outlineColor);
        DrawLine(data2, leftX, midY2, leftX, midY1, outlineColor);
        DrawLine(data2, centerX, centerY, centerX, lowY, outlineColor);
        DrawLine(data2, centerX, lowY, leftX, midY2, outlineColor);
        Fill(data2, outlineColor, leftColor);

        DrawLine(data3, centerX, centerY, rightX, midY1, outlineColor);
        DrawLine(data3, rightX, midY1, rightX, midY2, outlineColor);
        DrawLine(data3, rightX, midY2, centerX, lowY, outlineColor);
        DrawLine(data3, centerX, centerY, centerX, lowY, outlineColor);
        Fill(data3, outlineColor, rightColor);

        SpliceOnto(data1, data2);
        SpliceOnto(data1, data3);

        result.SetData(data1);

        return result;
    }

    private void SpliceOnto(Color[] inData, Color[] add) {
        for (int i = 0; i < _tileHeight; i++) {
            for (int ii = 0; ii < _tileWidth; ii++) {
                if (add[i * _tileHeight + ii] == Color.Transparent) continue;
                inData[i * _tileHeight + ii] = add[i * _tileHeight + ii];
            }
        }
    }

    public Texture2D ErrorTex { get; }
    public Texture2D ShadowTex { get; }
}

public class IsoGame : Game {
    static int layoutHeight = 20;
    static int layoutWidth = 20;
    static int tileHeight = 64;
    static int tileWidth = 64;
    Lua _lState = new Lua();

    // camera movement
    int _x = 0;
    int _y = 0;
    int step = 10;

    // pathfinding
    // Roy_T.AStar.Paths.Path? currentPath = null;
    // int edgeI;
    // int si = 0;
    // int sMax = 10;

    // current layout
    private Layout _layout;
    private Dictionary<TilesetEnum, Tile> _tileset;

    // graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteCreator _spriteCreator;

    // entities
    private List<ControlledEntity> entities = new();

    public IsoGame(int wWidth, int wHeight) {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = wWidth;
        _graphics.PreferredBackBufferHeight = wHeight;
        _graphics.ApplyChanges();

        IsMouseVisible = true;


        // add lua methods
        var type = typeof(IsoGame);
        foreach (var method in type.GetMethods()) {
            if (method.GetCustomAttribute(typeof(LuaCommand)) is object) {
                _lState[method.Name] = method.CreateDelegate(Expression.GetDelegateType(
                (from parameter in method.GetParameters() select parameter.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray()), this);
            }
        }
    }

    protected override void Initialize() {
        base.Initialize();

        Window.Title = "IsoGame";
        
        Window.AllowUserResizing = true;
        // Window.ClientSizeChanged += Changed;
    }

    // protected void Changed(object? o, EventArgs e) {
    //     System.Console.WriteLine("Amogus");
    // }

    protected override void Update(GameTime gameTime) {
        var kState = Keyboard.GetState();
        if (kState.IsKeyDown(Keys.Escape)) Exit();

        if (kState.IsKeyDown(Keys.Left)) _x += step;
        if (kState.IsKeyDown(Keys.Right)) _x -= step;
        if (kState.IsKeyDown(Keys.Up)) _y += step;
        if (kState.IsKeyDown(Keys.Down)) _y -= step;

        foreach (var e in entities)
            e.Upd(gameTime);

        base.Update(gameTime);    
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);
        _spriteCreator = new SpriteCreator(tileWidth, tileHeight, _graphics.GraphicsDevice);
        StaticEntity en = new StaticEntity(_spriteCreator.ErrorTex, 2);
        PlayerEntity player = new(_spriteCreator.ErrorTex);
        MonitorEntity monitor = new(_lState, 0, 0);
        _tileset = new Dictionary<TilesetEnum, Tile>() {
            { TilesetEnum.Floor, new Tile("Floor", _spriteCreator.CreateIsoCube(Color.Black, Color.PaleGreen, Color.LightGreen, Color.Green), true, false) },
            { TilesetEnum.Wall, new Tile("Wall", _spriteCreator.CreateIsoCube(Color.Black, Color.LightGray, Color.DarkGray, Color.Gray), true, false)},
            { TilesetEnum.Glass, new Tile("Glass", _spriteCreator.CreateIsoCube(Color.Aqua, Color.Aqua * .5f, Color.Aqua * .5f, Color.Aqua * .5f), true, true) }
        };
        _layout = new LayoutBuilder<TilesetEnum>(layoutHeight, layoutWidth, _tileset)
        .AddLevel(TilesetEnum.Floor)
        .AddLevel()
        .Rect(TilesetEnum.Wall, 1, 0, 0, 7, 7, true)
        .Remove(1, 6, 3)
        .AddLevel()
        .Rect(TilesetEnum.Wall, 2, 0, 0, 7, 7, true)
        .Remove(2, 6, 3)
        .AddLevel()
        .AddLevel()
        .AddLevel()
        .AddLevel()
        .Rect(TilesetEnum.Wall, 3, 0, 0, 7, 7, true)
        .Rect(TilesetEnum.Wall, 2, 10, 10, 4, 1)
        .Rect(TilesetEnum.Wall, 6, 10, 10, 4, 1)
        .Remove(3, 6, 3)
        .Put(TilesetEnum.Glass, 1, 7, 7)
        // .PlaceEntity(en, 0, 16, 15)
        // .PlaceEntity(player, 0, 12, 12)
        // .Fill(TilesetEnum.Glass, 1, 4, 4, 10, 5)
        .Build();

        player.CurrentLayout = _layout;
        player.MoveTo(16, 15, 0);
        entities.Add(player);
        entities.Add(monitor);

    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        _graphics.GraphicsDevice.Clear(Color.White);
        _spriteBatch.Begin();

        DrawLayout();

        _spriteBatch.End();

    }

    protected void DrawLayout() {
        int height = _layout[0].GetLength(0);
        int width = _layout[0].GetLength(1);
        for (int i = 0; i < height; i++) {
            for (int ii = 0; ii < width; ii++) {
                int lastSolid = -1;
                for (int levelI = 0; levelI < _layout.LevelCount; levelI ++) {
                    var level = _layout[levelI];
                    var slot = level[i, ii];
                    DrawTile(slot, i-levelI, ii-levelI, levelI);

                    // draw shadow
                    if (levelI == 0) {
                        if (slot is object) lastSolid = levelI;
                        continue;
                    }
                    if (slot is null) continue;
                    if (lastSolid == -1) {
                        lastSolid = levelI;
                        continue;
                    }
                    if (lastSolid != levelI - 1)
                        DrawTexture(_spriteCreator.ShadowTex, i-1-lastSolid, ii-1-lastSolid, lastSolid);
                    // Console.ReadKey();
                    // Console.WriteLine(lastSolid);
                    lastSolid = levelI;
                    // Console.ReadKey();
                    
                }
            }
        }
        
    }

    protected void DrawTile(TileSlot? t, int i, int ii, int depth) {
        if (t is null) return;
        // private void DrawTile(SpriteBatch sb, Texture2D tex, int i, int ii, int xOffset, int yOffset, bool transparent=false) {
        int xLoc = (int)(ii * tileWidth * 0.5 - i * tileWidth * 0.5);
        int yLoc = (int)(ii * tileHeight * 0.25  + i * tileHeight * 0.25);
        var vec = new Vector2(xLoc + _x, yLoc + _y);
        DrawTexture(t.Tile.Texture, i, ii, depth);
        // _spriteBatch.Draw(t.Tile.Texture, vec, Color.Lerp(Color.White, Color.Black, (3-depth) * .08f));
        if (t.Entity is null) return;
        DrawTexture(t.Entity.GetTexture(), i-1, ii-1);
        // _spriteBatch.Draw(t.Entity.GetTexture(), new Vector2(xLoc + _x - tileWidth * 3 / 2, yLoc + _y - tileHeight*2), Color.White);
        // _spriteBatch.Draw(t.Tile.Texture, vec, Color.White * (t.Tile.Transparent ? .5f : 1));
        // _spriteBatch.Draw(t.Tile.Texture, new Rectangle(xLoc + _x, yLoc + _y), Color.Black * depth * .1f);
//     }
    }

    protected void DrawTexture(Texture2D tex, int i, int ii, int depth=0) {
        int xLoc = (int)(ii * tileWidth * 0.5 - i * tileWidth * 0.5);
        int yLoc = (int)(ii * tileHeight * 0.25  + i * tileHeight * 0.25);
        var vec = new Vector2(xLoc + _x - (tex.Width - tileWidth), yLoc + _y - (tex.Height - tileHeight));

        _spriteBatch.Draw(tex, vec, Color.Lerp(Color.White, Color.Black, (3-depth) * .08f));
    }

    // lua commands
    [LuaCommand]
    public bool PlaceTile(string tileName, int x, int y, int levelI) {
        var layer = _layout[levelI];
        if (x < 0 || y < 0 || x >= layer.GetLength(1) || y >= layer.GetLength(0)) return false;

        // check if enough space to place entity
        var tileT = Enum.Parse(typeof(TilesetEnum), tileName);
        if (tileT is null) {
            System.Console.WriteLine($"ERR: no tile with name {tileName}");
            return false;
        }
        var tile = _tileset[(TilesetEnum)tileT];
        if (layer[x, y] is null)
            layer[x, y] = new TileSlot(tile);
        else 
            layer[x, y].Tile = tile;
        return true;
    }

    [LuaCommand]
    public bool RemoveTile(int x, int y, int levelI) {
        var layer = _layout[levelI];
        if (x < 0 || y < 0 || x >= layer.GetLength(1) || y >= layer.GetLength(0)) return false;
        var slot = layer[x, y];
        if (slot is null || slot.Entity is object) return false;
        layer[x, y] = null;
        return true;
    }
}

class PlayerEntity : MovableEntity {
    private Texture2D _tex;
    public PlayerEntity(Texture2D tex) : base(2, 0) {
        _tex = tex;
    }

    public override Texture2D GetTexture()
    {
        return _tex;
    }
    public override void Update(GameTime gameTime)
    {
        var kState = Keyboard.GetState();
        if (kState.IsKeyDown(Keys.S)) {
            Move(1, 1, 0);
        }
        if (kState.IsKeyDown(Keys.W)) {
            Move(-1, -1, 0);
        }
        if (kState.IsKeyDown(Keys.D)) {
            Move(-1, 1, 0);
        }
        if (kState.IsKeyDown(Keys.A)) {
            Move(1, -1, 0);
        }
    }
}

class MonitorEntity : ControlledEntity
{
    private Lua _lState;
    public MonitorEntity(Lua lState, int height, int updateDelay) : base(height, updateDelay)
    {
        _lState = lState;
    }

    public override Texture2D GetTexture()
    {
        throw new NotImplementedException();
    }

    public override void Update(GameTime gameTime)
    {
        var kState = Keyboard.GetState();
        if (kState.IsKeyDown(Keys.P)) {
            Console.Write("Enter Lua command: ");
            var command = Console.ReadLine();
            var result = _lState.DoString(command);
        }
    }

    
}



//     public class IsoGame1 : Game {

//         // private PlayerEntity player;

//         // private GraphicsDeviceManager _graphics;
//         // private SpriteBatch _spriteBatch;
//         // private Dictionary<TileType, Texture2D> _tileMap;
//         protected override void Update(GameTime gameTime) {
//             var kState = Keyboard.GetState();
//             if (kState.IsKeyDown(Keys.Escape)) Exit();
//             if (kState.IsKeyDown(Keys.Left)) _x += step;
//             if (kState.IsKeyDown(Keys.Right)) _x -= step;
//             if (kState.IsKeyDown(Keys.Up)) _y += step;
//             if (kState.IsKeyDown(Keys.Down)) _y -= step;
            
//             if (currentPath != null)
//             {
//                 if (si == 0) { 
//                     var edge = currentPath.Edges[edgeI];
//                     player.X = (int)edge.End.Position.X;
//                     player.Y = (int)edge.End.Position.Y;
//                     edgeI++;
//                     if (edgeI == currentPath.Edges.Count) currentPath = null;
//                 }
//                 si = (si + 1) % sMax;
//             }

//             base.Update(gameTime);
//         }

//         protected override void LoadContent()
//         {
//             _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

//             // tile texture
//             _tileMap = new()
//             {
//                 { TileType.Wall, createTileTexture(Color.DarkGray) },
//                 { TileType.Floor, createTileTexture(Color.BurlyWood) },
//                 { TileType.Selected, createTileTexture(Color.Yellow) },
//                 { TileType.Player, createTileTexture(Color.BlueViolet) },
//                 { TileType.Entrance, createTileTexture(Color.Green) },
//                 { TileType.Enemy, createTileTexture(Color.Red) },
//                 { TileType.Detection, createTileTexture(Color.LightPink) },
//             };

//         }

//         private Entity[] getEntities() {
//             Entity[] result = new Entity[1 + enemies.Length];
//             result[0] = player;
//             for (int i = 1; i < enemies.Length + 1; i++)
//                 result[i] = enemies[i-1];
//             return result;
//         }

//         protected override void Draw(GameTime gameTime)
//         {
//             base.Draw(gameTime);

//             _graphics.GraphicsDevice.Clear(Color.Black);

//             _spriteBatch.Begin();

//             var path = layout.Draw(_spriteBatch, _tileMap, _x, _y, getEntities());
//             if (path != null)
//             {
//                 currentPath = path;
//                 edgeI = 0;
//             }

//             _spriteBatch.End();

//         }
//     }
// }
