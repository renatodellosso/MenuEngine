using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src
{
    /// <summary>
    /// Note for SpriteFonts: kerning must be disabled and spacing must be manually set.
    /// </summary>
    public class Font
    {

        /// <summary>
        /// I recommend 2px spacing.
        /// </summary>
        public SpriteFont Regular { get; private set; }
        /// <summary>
        /// I recommend 2px spacing.
        /// </summary>
        public SpriteFont? Bold { get; private set; }
        /// <summary>
        /// Should use less spacing than <see cref="Regular"/> and <see cref="Bold"/>. I find 1px is a good value.
        /// </summary>
        public SpriteFont? Italic { get; private set; }
        /// <summary>
        /// Should use less spacing than <see cref="Regular"/> and <see cref="Bold"/>. I find 1px is a good value.
        /// </summary>
        public SpriteFont? BoldItalic { get; private set; }

        public Color Color { get; private set; }

        public Font(string name, string boldSuffix = "Bold", string italicSuffix = "Italic", string boldItalicSuffix = "BoldItalic", Color? color = null)
        {
            Regular = Assets.GetAsset<SpriteFont>(name)!;
            Bold = Assets.GetAsset<SpriteFont>(name + boldSuffix);
            Italic = Assets.GetAsset<SpriteFont>(name + italicSuffix);
            BoldItalic = Assets.GetAsset<SpriteFont>(name + boldItalicSuffix);

            Color = color ?? Color.White;
        }

    }
}
