using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src
{
    public static class Utils
    {

        /// <summary>
        /// Treats the vector as a percentage of the screen size and returns the pixel values.
        /// </summary>
        public static Vector2 ToPixels(this Vector2 vector2)
        {
            Viewport viewport = Engine.Instance.GraphicsDevice.Viewport;

            return new(vector2.X * viewport.Width, vector2.Y * viewport.Height);
        }

        /// <summary>
        /// Generates a rectangle given the position and size as percentages of the screen size.
        /// </summary>
        public static Rectangle RectFromPercents(Vector2 pos, Vector2 size)
        {
            pos = pos.ToPixels();
            size = size.ToPixels();

            return new((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

    }
}
