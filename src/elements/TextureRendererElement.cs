using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src.elements
{
    public class TextureRendererElement : RectElement
    {
        protected virtual Color Color { get; set; }

        public Texture2D Texture { get; set; }

        public TextureRendererElement(Vector2 position, Vector2 size, Color? color = null, Texture2D? texture = null) : this(null, position, size, color, texture) { }

        public TextureRendererElement(Element? parent, Vector2 position, Vector2 size, Color? color = null, Texture2D? texture = null) : base(parent, position, size)
        {
            Color = color ?? Color.White;
            Texture = texture ?? Assets.GetAsset<Texture2D>("BlankTexture")!;
        }

        public override void Draw()
        {
            Engine.SpriteBatch.Draw(Texture, Utils.RectFromPercents(Pos, Size), Color);
        }
    }
}
