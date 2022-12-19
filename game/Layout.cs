using Microsoft.Xna.Framework.Graphics;

namespace Isometric {

    using Entities;

    namespace Layout {

        public class Tile {
            string displayName = "--err--";
            Texture2D tex;
            public Texture2D Texture { get => tex; }
            bool solid;
            public bool Solid { get => solid; }
            bool transparent;
            public bool Transparent { get => transparent; }

            public Tile(string displayName, Texture2D tex, bool solid, bool transparent) {
                this.displayName = displayName;
                this.tex = tex;
                this.solid = solid;
                this.transparent = transparent;
            }
        }

        public class TileSlot {
            private Tile tile;
            private Entity? entity;
            public Tile Tile { get => tile; set => tile = value; }
            public Entity? Entity { get => entity; set => entity = value; }
            public TileSlot(Tile tile) {
                this.tile = tile;
            }
        }

        public class Layout {
            private TileSlot?[][,] _tiles;

            // public TileSlot?[][,] Tiles { get => _tiles; }
            public int LevelCount { get => _tiles.Length; }
            public TileSlot?[,] this[int i] { get => _tiles[i]; }

            private Layout() {

            }

            public Layout(TileSlot?[][,] tiles) {
                _tiles = tiles;
            }

            // public static Layout Load(string path) {
            //     var result = new Layout();
            //     // TODO
            //     return result;
            // }
                
        }

        public class LayoutBuilder<TilesetEnum> where TilesetEnum : Enum {
            int height;
            int width;
            private Dictionary<TilesetEnum, Tile> tileset;
            private List<TileSlot?[,]> tiles;
            public LayoutBuilder(int height, int width, Dictionary<TilesetEnum, Tile> tileset) {
                this.height = height;
                this.width = width;
                this.tileset = tileset;
                this.tiles = new List<TileSlot?[,]>();
            }

            public LayoutBuilder<TilesetEnum> AddLevel() {
                var level = new TileSlot?[height, width];
                for (int i = 0; i < height; i++) {
                    // level[i] = new TileSlot[width];
                    for (int ii = 0; ii < width; ii++)
                        level[i, ii] = null;
                }
                this.tiles.Add(level);
                return this;
            }

            public LayoutBuilder<TilesetEnum> AddLevel(TilesetEnum tile) {
                var level = new TileSlot?[height, width];
                var t = tileset[tile];
                for (int i = 0; i < height; i++) {
                    // level[i] = new TileSlot[width];
                    for (int ii = 0; ii < width; ii++)
                        level[i, ii] = new TileSlot(t);
                }
                this.tiles.Add(level);
                return this;
            }

            public LayoutBuilder<TilesetEnum> Rect(TilesetEnum tile, int level, int x, int y, int width, int height, bool fill=false) {
                var t = tileset[tile];
                var l = tiles[level];
                for (int i = y; i < y + height; i++)
                    for (int ii = x; ii < x + width; ii++) 
                        if (!fill || !(i > y && ii > x && i < y + height - 1 && ii < x + width - 1)) 
                            l[i, ii] = new TileSlot(t);
                return this;
            }

            public LayoutBuilder<TilesetEnum> Put(TilesetEnum tile, int level, int x, int y) {
                var t = tileset[tile];
                var l = tiles[level];
                l[y, x] = new TileSlot(t);
                return this;
            }

            public LayoutBuilder<TilesetEnum> Remove(int level, int x, int y) {
                var l = tiles[level];
                l[y, x] = null;
                return this;
            } 
 
            public LayoutBuilder<TilesetEnum> PlaceEntity(Entity entity, int level, int x, int y) {
                var l = tiles[level];
                l[y, x].Entity = entity;
                if (entity is MovableEntity) {
                    var ce = (MovableEntity)entity;
                    ce.MoveTo(x, y, level);
                }
                return this;
            }

            public Layout Build() {
                var result = new Layout(tiles.ToArray());
                return result;
            }
        }
    }   
}
