﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MenuEngine.src
{
    public class Input
    {

        public enum InputMode
        {
            /// <summary>
            /// Normal mode, used for keyboard shortcuts.
            /// </summary>
            Keybinds,
            /// <summary>
            /// Used for text entry.
            /// </summary>
            TextInput
        }

        private enum InputState
        {
            Up,
            /// <summary>
            /// Key is pressed down when it was previously up.
            /// </summary>
            Down,
            /// <summary>
            /// Key is pressed down when it was previously down.
            /// </summary>
            Held
        }

        public enum MouseButton
        {
            Left,
            Right,
            Middle
        }

        private static Input? instance;

        private InputMode mode;
        public static InputMode Mode
        {
            get => instance!.mode;
            set
            {
                if (instance!.mode != value)
                {
                    instance.mode = value;
                    Debug.WriteLine($"Input mode changed to {value}");
                }
            }
        }

        private Action<char>? textInputHandler;
        public static Action<char>? TextInputHandler
        {
            get => instance!.textInputHandler;
            set => instance!.textInputHandler = value;
        }

        private readonly Dictionary<Keys, InputState> keyStates;

        private MouseState prevMouseState;

        private Input()
        {
            keyStates = new();
        }

        internal static void Initialize()
        {
            instance = new();

            Engine.Instance.Window.KeyDown += instance.OnKeyDown;
            Engine.Instance.Window.KeyUp += instance.OnKeyUp;
            Engine.Instance.Window.TextInput += instance.OnTextInput;
            Engine.Instance.OnLateUpdate += instance.LateUpdate;

            instance.mode = InputMode.Keybinds;
        }

        private void LateUpdate()
        {
            prevMouseState = Mouse.GetState();
        }

        private void OnKeyDown(object? sender, InputKeyEventArgs e)
        {
            if (keyStates.ContainsKey(e.Key))
            {
                keyStates[e.Key] = InputState.Held;
            }
            else
                keyStates.Add(e.Key, InputState.Down);
        }

        private void OnKeyUp(object? sender, InputKeyEventArgs e)
        {
            keyStates.Remove(e.Key);
        }

        private void OnTextInput(object? sender, TextInputEventArgs e)
        {
            if (mode == InputMode.TextInput)
                textInputHandler?.Invoke(e.Character);
        }

        /// <returns>Whether key is being pressed down. Always returns false when <see cref="Mode"/> is <see cref="InputMode.TextInput"/>.</returns>
        public static bool IsKeyHeld(Keys key)
        {
            return Mode == InputMode.Keybinds && instance!.keyStates.ContainsKey(key);
        }

        /// <returns>Whether key is pressed down when it was not last frame. Always returns false when <see cref="Mode"/> is <see cref="InputMode.TextInput"/>.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return Mode == InputMode.Keybinds && instance!.keyStates.TryGetValue(key, out InputState state) && state == InputState.Down;
        }

        /// <returns>Whether the mouse button is held down.</returns>
        public static bool IsMouseButtonHeld(MouseButton button)
        {
            MouseState mouseState = Mouse.GetState();

            switch (button)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed;
                default:
                    break;
            }

            return false;
        }

        /// <returns>Whether the mouse button is pressed down when it was not last frame.</returns>
        public static bool IsMouseButtonDown(MouseButton button)
        {
            MouseState mouseState = Mouse.GetState();

            switch (button)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed && instance!.prevMouseState.LeftButton == ButtonState.Released;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed && instance!.prevMouseState.RightButton == ButtonState.Released;
                case MouseButton.Middle:
                    return mouseState.MiddleButton == ButtonState.Pressed && instance!.prevMouseState.MiddleButton == ButtonState.Released;
                default:
                    break;
            }

            return false;
        }

        public static bool IsMouseOver(Rectangle rect)
        {
            MouseState mouseState = Mouse.GetState();
            return rect.Contains(mouseState.Position.ToVector2());
        }

    }
}