using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Isometric {

    using Layout;

    namespace Entities {
        public abstract class Entity {
            protected readonly int _height;
            public abstract Texture2D GetTexture();
            public Entity(int height) {
                _height = height;
            }
        }

        public class StaticEntity : Entity {

            private Texture2D _texture;
            public StaticEntity(Texture2D texture, int height) : base(height) {
                _texture = texture;
            }

            public override Texture2D GetTexture()
            {
                return _texture;
            }
        }

        public abstract class ControlledEntity : Entity {

            private int _uC = 0;
            private int _uM;
            public void Upd(GameTime gameTime) {
                if (_uC == 0) Update(gameTime);
                if (_uM != 0)
                    _uC = (_uC + 1) % _uM;
            }

            public abstract void Update(GameTime gameTime);

            public ControlledEntity(int height, int updateDelay) : base(height) {
                _uM = updateDelay;
            }
        }

        public abstract class MovableEntity : ControlledEntity {
            protected int _x = -1;
            protected int _y = -1;
            protected int _level = -1;
            public int X { get => _x; }
            public int Y { get => _y; }

          
            public Layout.Layout CurrentLayout { get; set; }

            public bool Move(int xDiff, int yDiff, int zDiff) {
                return MoveTo(_x + xDiff, _y + yDiff, _level + zDiff);
            }

            public bool MoveTo(int x, int y, int level) {
                // check bounds
                var layer = CurrentLayout[level];
                if (x < 0 || y < 0 || x >= layer.GetLength(1) || y >= layer.GetLength(0)) return false;

                // check if not gap
                if (layer[x, y] is null) return false;

                // check if enough space to place entity
                if (_level != -1 && level + _height < CurrentLayout.LevelCount) {
                    for (int i = level + 1; i < level + _height; i++) {
                        var ll = CurrentLayout[i];
                        if (ll[x, y] is object && ll[x, y].Tile.Solid) return false;
                    }
                }
                TileSlot?[,]? l = null;
                if (_x != -1) {
                    l = CurrentLayout[_level];

                    if (l[_x, _y] is object) {
                        // if (l[x, y].Tile.Solid) return false;
                        l[_x, _y].Entity = null;
                    }
                }
                _x = x;
                _y = y;
                _level = level;
                l = CurrentLayout[_level];
                l[x, y].Entity = this;
                return true;
            }
            public MovableEntity(int height, int updateDelay) : base(height, updateDelay)
            {
                
            }
            
        }
    }
}