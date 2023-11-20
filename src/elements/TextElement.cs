using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuEngine.src.elements
{
    public class TextElement : RectElement
    {

        private class TextChunk : IFormattable
        {
            internal TextChunk? Container { get; set; }

            internal string Text { get; set; }
            internal SpriteFont Font { get; set; }
            internal Color Color { get; set; }

            internal string[] Args { get; set; }

            internal TextChunk(string text, string[] args, SpriteFont font, Color color)
            {
                Text = text;
                Args = args;
                Font = font;
                Color = color;
            }

            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return Text;
            }
        }

        private TextChunk[]? textChunks;
        private string prevText = "";
        public string Text
        {
            get
            {
                if (textChunks == null)
                    return "";

                string text = "";
                foreach (TextChunk chunk in textChunks)
                {
                    text += chunk.Text;
                }
                return text;
            }
            set
            {
                if (value != prevText)
                {
                    ParseStringToChunks(value);
                    prevText = value;
                }
            }
        }

        public Font Font { get; set; }

        public TextElement(Vector2 position, string text = "", Font? font = null) : this(null, position, text, font)
        { }

        public TextElement(Element? parent, Vector2 position, string text = "", Font? font = null) : base(parent, position, null)
        {
            Font = font ?? Project.Instance.DefaultFont!;
            Text = text;
        }

        public override void Draw()
        {
            if (textChunks == null)
                return;

            Vector2 pos = Pos.ToPixels();

            foreach (TextChunk chunk in textChunks)
            {
                Engine.SpriteBatch.DrawString(chunk.Font, chunk.Text, pos, chunk.Color);
                pos.X += chunk.Font.MeasureString(chunk.Text).X;
            }
        }

        private void ParseStringToChunks(string text)
        {
            List<TextChunk> chunks = new();

            TextChunk currentChunk = new("", Array.Empty<string>(), Font.Regular, Font.Color);

            for (int i = 0; i < text.Length; i++)
            {
                // Check for the start of a tag, ignoring \<
                if (text.ElementAt(i) == '<' && (i == 0 || text.ElementAt(i - 1) != '\\'))
                {
                    // Found a tag

                    // Read the tag
                    string tag = "";
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        char c = text.ElementAt(j);
                        if (c == '>' && text.ElementAt(j - 1) != '\\')
                        {
                            i += tag.Length + 1;
                            break;
                        }
                        tag += c;
                    }

                    tag = tag.Trim();
                    string[] args = tag.Split(' ');

                    chunks.Add(currentChunk);

                    if (!args.Contains("/"))
                    {
                        // Found an opening tag

                        TextChunk prevChunk = currentChunk;
                        currentChunk = new TextChunk("", args, Font.Regular, Font.Color);

                        // Add previous args to the new chunk, so container formatting carries over
                        List<string> argsList = args.ToList();
                        argsList.AddRange(prevChunk.Args);
                        args = argsList.ToArray();
                        currentChunk.Args = args;

                        currentChunk.Container = prevChunk;

                        // Check if it's an italic or bold tag
                        bool bold = args.Contains("b"), italic = args.Contains("i");

                        if (bold && italic)
                            currentChunk.Font = Font.BoldItalic ?? Font.Regular;
                        else if (bold)
                            currentChunk.Font = Font.Bold ?? Font.Regular;
                        else if (italic)
                            currentChunk.Font = Font.Italic ?? Font.Regular;

                        // Check for a color tag
                        IEnumerable<string> colorArgs = args.Where(arg => arg.StartsWith("color="));
                        if (colorArgs.Any())
                        {
                            string arg = colorArgs.First();
                            string colorName = arg[(arg.IndexOf('=') + 1)..];

                            Color color = ((Color?)typeof(Color).GetProperty(colorName)?.GetValue(null)) ?? Color.White;
                            currentChunk.Color = color;
                        }
                    }
                    else
                    {
                        // Found a closing tag

                        currentChunk = new("", currentChunk.Container?.Args ?? Array.Empty<string>(), currentChunk.Container?.Font ?? Font.Regular,
                            currentChunk.Container?.Color ?? Color.White);
                    }
                }
                else currentChunk.Text += text.ElementAt(i);
            }

            chunks.Add(currentChunk);

            textChunks = chunks.ToArray();
        }
    }
}