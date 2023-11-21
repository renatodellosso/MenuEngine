using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MenuEngine.src.elements
{
    public class TextElement : RectElement
    {

        public enum Justify
        {
            Left,
            Center,
            Right
        }

        public enum Align
        {
            Top,
            Center,
            Bottom
        }

        public enum Wrapping
        {
            Word,
            Char
        }

        private class TextChunk : IFormattable
        {
            internal TextChunk? Container { get; set; }

            internal string text;
            internal SpriteFont font;
            internal Color color;

            internal string[] args;

            internal Vector2 pos;

            internal Vector2 Size { get => font.MeasureString(text); }

            internal TextChunk(string text, string[] args, SpriteFont font, Color color)
            {
                this.text = text;
                this.args = args;
                this.font = font;
                this.color = color;
            }

            internal TextChunk(string text, TextChunk styling, Vector2 pos)
            {
                this.text = text;
                args = styling.args;
                font = styling.font;
                color = styling.color;
                this.pos = pos;
            }

            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return text;
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
                    text += chunk.text;
                }
                return text;
            }
        }

        public Font font;

        protected Justify justify;
        protected Align align;
        protected Wrapping wrapping;

        public TextElement(Vector2 position, Vector2 size, string text = "", Font? font = null, Justify justify = Justify.Left, Align align = Align.Top,
            Wrapping wrapping = Wrapping.Word) : this(null, position, size, text, font, justify, align, wrapping)
        { }

        public TextElement(Element? parent, Vector2 position, Vector2 size, string text = "", Font? font = null, Justify justify = Justify.Left, Align align = Align.Top,
            Wrapping wrapping = Wrapping.Word) : base(parent, position, size)
        {
            this.font = font ?? Project.Instance.DefaultFont!;
            this.justify = justify;
            this.align = align;
            this.wrapping = wrapping;

            SetText(text);
        }

        public override void Draw()
        {
            if (textChunks == null)
                return;

            foreach (TextChunk chunk in textChunks)
            {
                Engine.SpriteBatch.DrawString(chunk.font, chunk.text, chunk.pos, chunk.color);
            }
        }

        public void SetText(string text)
        {
            if (text != prevText)
            {
                ParseStringToChunks(text);
                prevText = text;
            }
        }

        private void ParseStringToChunks(string text)
        {
            List<TextChunk> chunks = new();

            TextChunk currentChunk = new("", Array.Empty<string>(), font.Regular, font.Color);

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
                        currentChunk = new TextChunk("", args, font.Regular, font.Color);

                        // Add previous args to the new chunk, so container formatting carries over
                        List<string> argsList = args.ToList();
                        argsList.AddRange(prevChunk.args);
                        args = argsList.ToArray();
                        currentChunk.args = args;

                        currentChunk.Container = prevChunk;

                        // Check if it's an italic or bold tag
                        bool bold = args.Contains("b"), italic = args.Contains("i");

                        if (bold && italic)
                            currentChunk.font = font.BoldItalic ?? font.Regular;
                        else if (bold)
                            currentChunk.font = font.Bold ?? font.Regular;
                        else if (italic)
                            currentChunk.font = font.Italic ?? font.Regular;

                        // Check for a color tag
                        IEnumerable<string> colorArgs = args.Where(arg => arg.StartsWith("color="));
                        if (colorArgs.Any())
                        {
                            string arg = colorArgs.First();
                            string colorName = arg[(arg.IndexOf('=') + 1)..];

                            Color color = ((Color?)typeof(Color).GetProperty(colorName)?.GetValue(null)) ?? Color.White;
                            currentChunk.color = color;
                        }
                    }
                    else
                    {
                        // Found a closing tag

                        currentChunk = new("", currentChunk.Container?.args ?? Array.Empty<string>(), currentChunk.Container?.font ?? font.Regular,
                            currentChunk.Container?.color ?? Color.White);
                    }
                }
                else currentChunk.text += text.ElementAt(i);
            }

            chunks.Add(currentChunk);

            CalculateCharChunks(chunks.ToArray());
        }

        /// <summary>
        /// Converts the text chunks into character chunks, so that each character can be drawn individually.
        /// </summary>
        /// <param name="chunks"></param>
        private void CalculateCharChunks(TextChunk[] chunks)
        {
            List<TextChunk> charChunks = new();

            Vector2 pos = Pos.ToPixels();

            void NewLine()
            {
                pos = new(Pos.ToPixels().X, pos.Y + font.Regular.MeasureString("A").Y);
            }

            foreach (TextChunk chunk in chunks)
            {
                string[] words = chunk.text.Split(' ');
                for (int i = 0; i < words.Length - 1; i++)
                    words[i] += " ";

                foreach (string word in words)
                {
                    if (wrapping == Wrapping.Word && pos.X + chunk.font.MeasureString(word).X > Pos.ToPixels().X + Size.ToPixels().X)
                    {
                        // Word is too long for the line, move to the next line
                        NewLine();
                    }

                    foreach (char c in word)
                    {
                        if (c == '\n')
                        {
                            NewLine();
                            continue;
                        }

                        if (wrapping == Wrapping.Char && pos.X + chunk.font.MeasureString(c.ToString()).X > Pos.ToPixels().X + Size.ToPixels().X)
                            NewLine();

                        TextChunk charChunk = new(c.ToString(), chunk, pos);
                        charChunks.Add(charChunk);
                        pos.X += charChunk.Size.X;
                    }
                }
            }

            textChunks = charChunks.ToArray();
        }
    }
}