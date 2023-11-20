using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src.elements
{
    public class RootElement : RectElement
    {

        public override Vector2 Size
        {
            get
            {
                Viewport viewport = Engine.Instance.GraphicsDevice.Viewport;
                return new Vector2(viewport.Width, viewport.Height);
            }
            set => base.Size = value;
        }

        public RootElement() : base(null, Vector2.Zero, Vector2.Zero, Color.White)
        {

        }
    }
}