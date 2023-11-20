using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src.elements
{
    public class RectElement : Element
    {

        protected Vector2 pos;
        public virtual Vector2 Pos { get => pos; set => pos = value; }

        protected Vector2 size;
        public virtual Vector2 Size { get => size; set => size = value; }

        protected virtual Color Color { get; set; }

        public RectElement(Vector2 position, Vector2 size, Color color) : this(null, position, size, color) { }

        public RectElement(Element? parent, Vector2 position, Vector2 size, Color color) : base(parent)
        {
            Pos = position;
            Size = size;
            Color = color;
        }

        public override void Draw()
        {
            Engine.SpriteBatch.Draw(Assets.GetAsset<Texture2D>("BlankTexture"), Pos, new Rectangle((int)Pos.X, (int)Pos.Y, (int)Size.X, (int)Size.Y), Color);
        }

    }
}
