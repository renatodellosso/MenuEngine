using Microsoft.Xna.Framework;

namespace MenuEngine.src.elements
{
    public class BorderElement : TextureRendererElement
    {

        /// <summary>
        /// Border thickness, in pixels.
        /// </summary>
        protected uint thickness;

        public override Vector2 Pos
        {
            get
            {
                Vector2 pos = ((RectElement)Parent!).Pos;
                pos = pos.ToPixels();
                pos.X -= thickness;
                pos.Y -= thickness;
                return pos;
            }
        }

        public override Vector2 Size
        {
            get
            {
                Vector2 size = ((RectElement)Parent!).Size;
                size = size.ToPixels();
                size.X += thickness * 2;
                size.Y += thickness * 2;
                return size;
            }
        }

        /// <param name="thickness">Border thickness, in pixels.</param>
        public BorderElement(RectElement parent, uint thickness, Color color) : base(parent, Vector2.Zero, Vector2.Zero, color)
        {
            this.thickness = thickness;
        }

        public override void Draw()
        {
            Vector2 pos = Pos;
            Vector2 size = Size;
            // Top
            Engine.SpriteBatch.Draw(Texture, new Rectangle(pos.ToPoint(), new Vector2(size.X, thickness).ToPoint()), Color);
            // Bottom
            Engine.SpriteBatch.Draw(Texture, new Rectangle(new Vector2(pos.X, pos.Y + size.Y - thickness).ToPoint(), new Vector2(size.X, thickness).ToPoint()), Color);
            // Left
            Engine.SpriteBatch.Draw(Texture, new Rectangle(new Vector2(pos.X, pos.Y + thickness).ToPoint(), new Vector2(thickness, size.Y - thickness * 2).ToPoint()), Color);
            // Right
            Engine.SpriteBatch.Draw(Texture, new Rectangle(new Vector2(pos.X + size.X - thickness, pos.Y + thickness).ToPoint(),
                new Vector2(thickness, size.Y - thickness * 2).ToPoint()), Color);
        }

    }
}
