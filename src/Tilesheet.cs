using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src
{
    public class Tilesheet : Texture2D
    {

        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public int TilePadding { get; set; }

        public Tilesheet(GraphicsDevice graphicsDevice, int width, int height) : base(graphicsDevice, width, height)
        {
        }

        public Tilesheet(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format) : base(graphicsDevice, width, height, mipmap, format)
        {
        }

        public Tilesheet(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, int arraySize) : base(graphicsDevice, width, height, mipmap, format, arraySize)
        {
        }

        protected Tilesheet(GraphicsDevice graphicsDevice, int width, int height, bool mipmap, SurfaceFormat format, SurfaceType type, bool shared, int arraySize) : base(graphicsDevice, width, height, mipmap, format, type, shared, arraySize)
        {
        }

        public Tilesheet(Texture2D texture, int tileWidth, int tileHeight, int tilePadding) : base(texture.GraphicsDevice, texture.Width, texture.Height)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            TilePadding = tilePadding;

            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            SetData(data);
        }

        public Texture2D this[int x, int y]
        {
            get
            {
                Rectangle bounds = new(x * (TileWidth + TilePadding), y * (TileHeight + TilePadding), TileWidth, TileHeight);
                Color[] data = new Color[bounds.Width * bounds.Height];
                GetData(0, bounds, data, 0, data.Length);

                Texture2D tile = new(GraphicsDevice, bounds.Width, bounds.Height);
                tile.SetData(data);

                return tile;
            }
        }

    }
}
