using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MenuEngine.src.elements
{
    public class InputFieldElement : ButtonElement
    {

        private bool selected;

        private string text, defaultText;

        private bool cursorDisplayedPrev;

        private Action<string>? onSubmit;

        public InputFieldElement(Vector2 position, Vector2 size, string defaultText, Color? defaultTextColor = null, Color? defaultColor = null, Color? hoveredColor = null, Texture2D? texture = null,
            uint borderThickness = 1, Color? borderColor = null, Action<string>? onSubmit = null)
            : this(null, position, size, defaultText, defaultTextColor, defaultColor, hoveredColor, texture, borderThickness, borderColor, onSubmit)
        {
        }

        public InputFieldElement(Element? parent, Vector2 position, Vector2 size, string defaultText, Color? defaultTextColor = null, Color? defaultColor = null,
            Color? hoveredColor = null, Texture2D? texture = null, uint borderThickness = 1, Color? borderColor = null, Action<string>? onSubmit = null)
            : base(parent, position, size, defaultColor, hoveredColor, texture, borderThickness: borderThickness, borderColor: borderColor, labelText: defaultText)
        {
            Label!.justify = TextElement.Justify.Left;
            Label!.RegenerateText();

            text = "";
            this.defaultText = defaultText;

            this.onSubmit = onSubmit;
        }

        public override void Update()
        {
            isMouseOver = Input.IsMouseOver(Rect);

            if (Input.IsMouseButtonDown(Input.MouseButton.Left))
            {
                selected = isMouseOver;

                if (selected)
                {
                    Label!.SetText(text);
                    Input.Mode = Input.InputMode.TextInput;
                    Input.TextInputHandler = OnTextInput;
                }
                else
                {
                    if (text == "")
                        Label!.SetText(defaultText);
                    else
                        Label!.SetText(text);
                    Input.Mode = Input.InputMode.Keybinds;
                }
            }

            // Cursor blinking
            if (selected && DateTime.Now.Second % 2 > 0 != cursorDisplayedPrev)
            {
                cursorDisplayedPrev = !cursorDisplayedPrev;
                Label!.SetText(text + (cursorDisplayedPrev ? "|" : ""));
            }
        }

        private void OnTextInput(char c)
        {
            if (c == '\b')
            {
                if (text.Length > 0)
                    text = text.Remove(text.Length - 1);
            }
            else if (c == '\r')
            {
                Submit();
                return;
            }
            else
                text += c;

            Label!.SetText(text + (cursorDisplayedPrev ? "|" : ""));
        }

        /// <summary>
        /// Runs when the user presses enter while the input field is selected.
        /// </summary>
        public virtual void Submit()
        {
            Debug.WriteLine($"Input field submitted: {text}");
            onSubmit?.Invoke(text);
        }
    }
}
