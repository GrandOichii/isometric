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

namespace Isometric {
    enum TileType : int
    {
        Floor = '.',
        Selected = '+',
        Wall = '*',
        Entrance = 'o',
        Player = '@',
        Enemy = 'Z',
    }

    abstract class Entity
    {
        private int x;
        private int y;

        protected TileType type;


        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public TileType Type { get => type; }

        protected Entity(int x, int y, TileType t)
        {
            this.x = x;
            this.y = y;
            this.type = t;
        }
    }

    class PlayerEntity : Entity
    {
        public PlayerEntity(int x, int y) : base(x, y, TileType.Player) { }
    }

    class EnemyEntity : Entity
    {
        public EnemyEntity(int x, int y) : base(x, y, TileType.Enemy) { }
    }


    class Tile
    {
        static readonly TileType[] highTiles = { TileType.Wall };
        static bool IsHighTile(TileType t)
        {
            //return false;
            return highTiles.Contains(t);
        }

        public static int Height = 64;
        public static int Width = 64;
        private TileType type;

        private bool isHigh;
        public Tile(TileType t)
        {
            Type = t;
            IsHigh = IsHighTile(t);
            
        }

        public bool IsHigh { get => isHigh; set => isHigh = value; }
        internal TileType Type { get => type; set => type = value; }

        public void Draw(SpriteBatch sb, Texture2D tex, int i, int ii, int xOffset, int yOffset, bool transparent=false)
        {
            int xLoc = (int)(ii * Width * 0.5 - i * Width * 0.5);
            int yLoc = (int)(ii * Height * 0.25  + i * Height * 0.25);
            var vec = new Vector2(xLoc + xOffset, yLoc + yOffset);
            sb.Draw(tex, vec, Color.White * (transparent ? .5f : 1));
        }

        public bool IntersectsMouse(int i, int ii, int xOffset, int yOffset)
        {
            int xLoc = (int)(ii * Width * 0.5 - i * Width * 0.5);
            int yLoc = (int)(ii * Height * 0.25 + i * Height * 0.25);
            var mState = Mouse.GetState();
            var mX = mState.X - xOffset;
            var mY = mState.Y - 20 - yOffset;
            return mState.LeftButton == ButtonState.Pressed && !isHigh && mX >= xLoc && mY >= yLoc && mX <= xLoc + Width && mY <= yLoc + Height;
        }

    }

    class Layout
    {
        //static Random rnd = new Random();
        static Random rnd = new Random(1);

        Tile[][] tiles;

        // pathfinding
        Grid grid;
        PathFinder pathFinder = new PathFinder();

        static int iOffset = -4;
        static int iiOffset = 6;


        private void CreatePathfinding()
        {
            var gridSize = new GridSize(tiles[0].Length, tiles.Length);
            var cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
            var traversalVelocity = Velocity.FromKilometersPerHour(100);
            grid = Grid.CreateGridWithLateralAndDiagonalConnections(gridSize, cellSize, traversalVelocity);

            for (int i = 0; i < tiles.Length; i++)
            {
                for (int ii = 0; ii < tiles[i].Length; ii++)
                {
                    if (tiles[i][ii].Type == TileType.Wall) grid.DisconnectNode(new GridPosition(ii, i));
                }

            }
        }

        public Layout(char[][] map)
        {
            //new Roy_T.AStar.Paths.PathFinder()
            tiles = new Tile[map.Length][];
            for (int i = 0; i < map.Length; i++)
            {
                tiles[i] = new Tile[map[i].Length];
                for (int ii = 0; ii < map[i].Length; ii++)
                {
                    var type = (TileType)map[i][ii];
                    tiles[i][ii] = new Tile(type);

                }
            }
            CreatePathfinding();
        }

        public Layout(int height, int width)
        {
            // generate outer walls
            tiles = new Tile[height][];
            for (int i = 0; i < height; i++)
            {
                tiles[i] = new Tile[width];
                for (int ii = 0; ii < width; ii++)
                {

                    var type = i == 0 || ii == 0 || i == height - 1 || ii == width - 1 ? TileType.Wall : TileType.Floor;
                    tiles[i][ii] = new Tile(type);

                }
            }

            // generate entrance room
            var eX = rnd.Next(1, width-1);
            var eY = rnd.Next(1, height-1);
            var startRoomH = 5;
            var startRoomW = 5;
            createRoom(eX - startRoomW / 2, eY - startRoomH / 2, startRoomW, startRoomH);

            int hallLength = rnd.Next(7, 20);
            bool created = false;
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                if (createHallway(eX, eY, dir, hallLength, 3))
                {
                    created = true;
                    break;
                }
            }
            putTile(eX, eY, TileType.Entrance);


            //int roomSize
            //createHallway(eX-startRoomW/2, eY, Direction.West, 11, 3);
            //createHallway(eX, eY - startRoomH / 2, Direction.North, 11, 3);

            CreatePathfinding();
        }

        public Vector2 getEntrance()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int ii = 0; ii < tiles[i].Length; ii++)
                {
                    if (tiles[i][ii].Type == TileType.Entrance) return new Vector2(ii, i);
                }
            }
            throw new Exception("Entrance not found");
        }

        private bool putTile(int x, int y, TileType t)
        {
            if (x < 0 || y < 0 || y >= tiles.Length || x >= tiles[0].Length) return false;
            tiles[y][x] = new Tile(t);
            return true;
        }

        private bool createRoom(int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || x + width >= tiles[0].Length || y + height >= tiles.Length) return false;
            for (int i = 0; i < width; i++)
            {
                for (int ii = 0; ii < height; ii++)
                {
                    if (i != 0 && ii != 0 && i != width-1 && ii != height-1) continue;
                    putTile(x + i, y + ii, TileType.Wall);
                }
            }
            return true;
        }

        enum Direction : int
        {
            North = 0,
            South,
            West,
            East
        }

        private bool createHallway(int x, int y, Direction dir, int length, int width)
        {
            int hw = width / 2 + 1;
            int xDiff = 0;
            int yDiff = 0;
            switch (dir)
            {
                case Direction.North:
                    xDiff = 0;
                    yDiff = -1;
                    break;
                case Direction.South:
                    xDiff = 0;
                    yDiff = 1;
                    break;
                case Direction.West:
                    xDiff = -1;
                    yDiff = 0;
                    break;
                case Direction.East:
                    xDiff = 1;
                    yDiff = 0;
                    break;
            }
            if (x < 0 || y < 0 || x >= tiles[0].Length || y >= tiles.Length || x + xDiff * length >= tiles[0].Length || y + yDiff * length >= tiles.Length || x + xDiff * length < 0 || y + yDiff * length < 0) return false;
            for (int i = 0; i < length; i++)
            {
                for (int ii = 1; ii <= width/2+1; ii++)
                {

                    putTile(x + (hw - ii) * yDiff, y + (hw - ii) * xDiff, TileType.Floor); ;
                    putTile(x + (hw - ii) * -yDiff, y + (hw - ii) * -xDiff, TileType.Floor);
                }
                putTile(x + hw * yDiff, y + hw * xDiff, TileType.Wall);
                putTile(x + hw * -yDiff, y + hw * -xDiff, TileType.Wall);
                x += xDiff;
                y += yDiff;
            }
            return true;
        }

        public Roy_T.AStar.Paths.Path Draw(SpriteBatch sb, Dictionary<TileType, Texture2D> tileMap, int xOffset, int yOffset, Entity[] entities)
        {
            int selectedI = -1;
            int selectedII = -1;
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int ii = 0; ii < tiles[i].Length; ii++)
                {
                    if (!tiles[i][ii].IntersectsMouse(i + iOffset, ii + iiOffset, xOffset, yOffset)) continue;
                    selectedI = i;
                    selectedII = ii;
                }
            }
            List<Vector2> points = new();
            Roy_T.AStar.Paths.Path result = null;
            var player = entities[0];
            if (selectedI != -1)
            {
                result = pathFinder.FindPath(new GridPosition(player.X, player.Y), new GridPosition(selectedII, selectedI), grid);
                foreach (var edge in result.Edges)
                {
                    int x = (int)edge.End.Position.X;
                    int y = (int)edge.End.Position.Y;
                    points.Add(new(x, y));
                }
                if (result.Edges.Count == 0) result = null;
            }
            
            // draw base layer
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int ii = 0; ii < tiles[i].Length; ii++)
                {
                    var tex = points.Contains(new(ii, i)) ? tileMap[TileType.Selected] : tileMap[tiles[i][ii].Type];
                    tiles[i][ii].Draw(sb, tex, i + iOffset, ii + iiOffset, xOffset, yOffset);
                    //if (i == playerY && ii == playerX)
                    //    tiles[i][ii].Draw(sb, tileMap[TileType.Character], i + iOffset-1, ii + iiOffset-1, xOffset, yOffset);
                }

            }
            // draw entities
            foreach (Entity en in entities) {
                var x = en.X;
                var y = en.Y;
                tiles[y][x].Draw(sb, tileMap[en.Type], y + iOffset, x + iiOffset, xOffset, yOffset);

            }

            // draw wall layer
            for (int i = 0; i < tiles.Length; i++)
            {
                for (int ii = 0; ii < tiles[i].Length; ii++)
                {
                    if (!tiles[i][ii].IsHigh) continue;
                    var tex = points.Contains(new(ii, i)) ? tileMap[TileType.Selected] : tileMap[tiles[i][ii].Type];
                    tiles[i][ii].Draw(sb, tex, i + iOffset - 1, ii + iiOffset - 1, xOffset, yOffset, true);
                    //if (i == playerY && ii == playerX)
                    //    tiles[i][ii].Draw(sb, tileMap[TileType.Character], i + iOffset-1, ii + iiOffset-1, xOffset, yOffset);
                }

            }


            return result;
        }
    }

    public class IsoGame : Game {
        private Layout layout = new Layout(50, 50);


        int _x = 0;
        int _y = 0;
        int step = 10;

        Roy_T.AStar.Paths.Path currentPath = null;
        int edgeI;
        int si = 0;
        int sMax = 10;

        private PlayerEntity player;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Dictionary<TileType, Texture2D> _tileMap;

        public IsoGame() {
            var v = layout.getEntrance();
            var playerX = (int)v.X;
            var playerY = (int)v.Y;
            player = new PlayerEntity(playerX, playerY);

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        protected override void Initialize() {
            base.Initialize();

            Window.Title = "Amogus";
        }

        protected override void Update(GameTime gameTime) {
            var kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Escape)) Exit();
            if (kState.IsKeyDown(Keys.Left)) _x += step;
            if (kState.IsKeyDown(Keys.Right)) _x -= step;
            if (kState.IsKeyDown(Keys.Up)) _y += step;
            if (kState.IsKeyDown(Keys.Down)) _y -= step;
            
            if (currentPath != null)
            {
                if (si == 0) { 
                    var edge = currentPath.Edges[edgeI];
                    player.X = (int)edge.End.Position.X;
                    player.Y = (int)edge.End.Position.Y;
                    edgeI++;
                    if (edgeI == currentPath.Edges.Count) currentPath = null;
                }
                si = (si + 1) % sMax;
            }

            base.Update(gameTime);
        }

        private Texture2D createTileTexture(Color color, bool half = false)
        {
            int pointSize = 1;
            Color outlineColor = Color.Black;
            Texture2D result = new(_graphics.GraphicsDevice, Tile.Width, Tile.Height);
            Color[] data = new Color[Tile.Width * Tile.Height];

            var drawPoint = delegate (int x, int y, Color c)
            {
                var half = pointSize / 2;
                for (int i = 0; i < pointSize; i++)
                {
                    for (int ii = 0; ii < pointSize; ii++)
                    {
                        int loc = (x + ii - half) * Tile.Width + y + i - half;
                        if (x + ii - half < 0 || y + i - half < 0 || y + i - half >= Tile.Height || x + ii - half >= Tile.Width) continue;
                        if (loc < 0 || loc >= data.Length) continue;
                        data[loc] = c;
                    }
                }
            };

            var drawLine = delegate (int y1, int x1, int y2, int x2, Color c)
            {
                // Iterators, counters required by algorithm
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
                    drawPoint(x, y, c); // Draw first pixel
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
                        drawPoint(x, y, c);
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
                    drawPoint(x, y, c); // Draw first pixel
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
                        drawPoint(x, y, c);
                    }
                }
            };

            int centerX = Tile.Width / 2;
            int rightX = Tile.Width - 1;
            int leftX = 0;

            int highY = 0;
            int midY1 = Tile.Height / 4;
            int centerY = Tile.Height / 2;
            int midY2 = Tile.Height * 3 / 4;
            int lowY = Tile.Height - 1;


            drawLine(centerX, highY, rightX, midY1, outlineColor);
            drawLine(rightX, midY1, rightX, midY2, outlineColor);
            drawLine(rightX, midY2, centerX, lowY, outlineColor);
            drawLine(centerX, lowY, leftX, midY2, outlineColor);
            drawLine(leftX, midY2, leftX, midY1, outlineColor);
            drawLine(leftX, midY1, centerX, highY, outlineColor);
            for (int i = 0; i < Tile.Height; i++)
            {
                int c = 0;
                for (int ii = 0; ii < Tile.Width; ii++)
                {
                    if (data[ii * Tile.Width + i] == outlineColor)
                        ++c;
                    if (c == 1) drawPoint(ii, i, color);
                }
            }
            drawLine(centerX, centerY, centerX, lowY, outlineColor);
            drawLine(centerX, centerY, leftX, midY1, outlineColor);
            drawLine(centerX, centerY, rightX, midY1, outlineColor);
            drawLine(leftX, midY2, leftX, midY1, outlineColor);
            drawLine(leftX, midY1, centerX, highY, outlineColor);
            drawLine(centerX, highY, rightX, midY1, outlineColor);


            result.SetData(data);

            return result;
        } 

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);


            // tile texture
            _tileMap = new()
            {
                { TileType.Wall, createTileTexture(Color.DarkGray) },
                { TileType.Floor, createTileTexture(Color.BurlyWood) },
                { TileType.Selected, createTileTexture(Color.Yellow) },
                { TileType.Player, createTileTexture(Color.BlueViolet) },
                { TileType.Entrance, createTileTexture(Color.Green) },
            };

        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _graphics.GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            var path = layout.Draw(_spriteBatch, _tileMap, _x, _y, new Entity[] { player });
            if (path != null)
            {
                currentPath = path;
                edgeI = 0;
            }

            _spriteBatch.End();

        }
    }
}