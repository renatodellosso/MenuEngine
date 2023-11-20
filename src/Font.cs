using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src
{
    public class Font
    {

        public SpriteFont Regular { get; private set; }
        public SpriteFont? Bold { get; private set; }
        public SpriteFont? Italic { get; private set; }
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
