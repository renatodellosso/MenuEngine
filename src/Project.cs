using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;

namespace MenuEngine.src
{
    public abstract class Project
    {

        public static Project Instance { get; private set; }

        public RootElement? RootElement { get; private set; }

        public Font? DefaultFont { get; private set; }

        public string Title
        {
            get => Engine.Instance.Window?.Title ?? "Window is null!"; protected set
            {
                if (Engine.Instance.Window != null)
                    Engine.Instance.Window.Title = value;
                else
                    Debug.WriteLine("Tried to set title before Window is created!");
            }
        }

        protected string author = "Unknown", projectName = "Unnamed Project";

        public static string PersistantDataPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Instance.author, Instance.projectName);

        /// <summary>
        /// This method is called before <see cref="Engine.Instance"/> exists.
        /// </summary>
        public Project()
        {
            Instance = this;
        }

        /// <summary>
        /// Called after <see cref="Engine.Instance"/> exists. and after <see cref="LoadAssets"/> is called.
        /// </summary>
        internal void Initialize()
        {
            Debug.WriteLine("Initializing project...");

            RootElement = new RootElement();

            DefaultFont = new("Arial");

            Input.Initialize();

            EnableFullScreen();

            OnInitialize();
        }

        public abstract void OnInitialize();

        /// <summary>
        /// Call <see cref="Assets.LoadAsset{T}(string)"/> in here to load assets.
        /// </summary>
        public virtual void LoadAssets()
        {
            Debug.WriteLine("Loading assets...");

            Texture2D blankTexture = new(Engine.Instance.GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });
            Assets.AddAssetDirect("BlankTexture", blankTexture);

            Assets.LoadAsset<SpriteFont>("Arial");
            Assets.LoadAsset<SpriteFont>("ArialBold");
            Assets.LoadAsset<SpriteFont>("ArialItalic");
            Assets.LoadAsset<SpriteFont>("ArialBoldItalic");
        }

        public static void EnableFullScreen()
        {
            Engine.Instance.graphics.PreferredBackBufferHeight = Engine.Instance.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            Engine.Instance.graphics.PreferredBackBufferWidth = Engine.Instance.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            Engine.Instance.graphics.IsFullScreen = true;
            Engine.Instance.graphics.HardwareModeSwitch = false; // Not sure what this actually is
            Engine.Instance.graphics.ApplyChanges(); // Make sure to apply changes!
        }

    }
}