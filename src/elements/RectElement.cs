using Microsoft.Xna.Framework;

namespace MenuEngine.src.elements
{
    public class RectElement : Element
    {

        protected Vector2 pos;
        /// <summary>
        /// The rect's position, in percentages of the screen size.
        /// </summary>
        public virtual Vector2 Pos { get => pos; set => pos = value; }

        protected Vector2? size;
        /// <summary>
        /// The rect's size, in percentages of the screen size.
        /// </summary>
        public virtual Vector2 Size { get => size.GetValueOrDefault(); set => size = value; }

        /// <summary>
        /// Creates a rect from <see cref="Pos"/> and <see cref="Size"/>. NOTE: This is in pixels of the screen size.
        /// </summary>
        public Rectangle Rect => new(Pos.ToPixels().ToPoint(), Size.ToPixels().ToPoint());

        public RectElement(Vector2 position, Vector2? size) : this(null, position, size) { }

        public RectElement(Element? parent, Vector2 position, Vector2? size) : base(parent)
        {
            Pos = position;
            Size = size.GetValueOrDefault();
        }

    }
}
