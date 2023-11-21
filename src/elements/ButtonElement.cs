using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MenuEngine.src.elements
{
    public class ButtonElement : TextureRendererElement
    {

        private bool isMouseOver;

        private Color defaultColor, hoveredColor;

        protected override Color Color { get => isMouseOver ? hoveredColor : defaultColor; set => defaultColor = value; }

        private Action? onClick;

        public ButtonElement(Vector2 position, Vector2 size, Color? defaultColor = null, Color? hoveredColor = null, Texture2D? texture = null, Action? onClick = null)
            : this(null, position, size, defaultColor, hoveredColor, texture, onClick)
        { }

        public ButtonElement(Element? parent, Vector2 position, Vector2 size, Color? defaultColor = null, Color? hoveredColor = null, Texture2D? texture = null,
            Action? onClick = null) : base(parent, position, size, defaultColor ?? Color.Black, texture)
        {
            this.hoveredColor = hoveredColor ?? Color.Gray;
            this.onClick = onClick;
        }

        public override void Update()
        {
            isMouseOver = Input.IsMouseOver(Rect);

            if (Input.IsMouseButtonDown(Input.MouseButton.Left))
                OnClick();
        }

        protected virtual void OnClick()
        {
            onClick?.Invoke();
        }
    }
}
