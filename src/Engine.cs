using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace MenuEngine.src
{
    public class Engine : Game
    {

        internal static Engine Instance { get; private set; }

        private readonly Project project;

        internal GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;
        public static SpriteBatch SpriteBatch => Instance.spriteBatch;

        private Assets assets;

        /// <summary>
        /// Seconds since the last frame.
        /// </summary>
        private float deltaTime;
        public static float DeltaTime => Instance.deltaTime;

        public event Action OnUpdate;
        public static event Action OnUpdateEvent
        {
            add => Instance.OnUpdate += value;
            remove => Instance.OnUpdate -= value;
        }

        public event Action OnLateUpdate;
        /// <summary>
        /// Called after all <see cref="OnUpdate"/> events.
        /// </summary>
        public static event Action OnLateUpdateEvent
        {
            add => Instance.OnLateUpdate += value;
            remove => Instance.OnLateUpdate -= value;
        }

        public event Action OnDraw;
        public static event Action OnDrawEvent
        {
            add => Instance.OnDraw += value;
            remove => Instance.OnDraw -= value;
        }

        public Engine(Project project)
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "content";
            IsMouseVisible = true;

            this.project = project;
        }

        protected override void Initialize()
        {
            base.Initialize();

            project.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            assets = new Assets();
            project.LoadAssets();
        }

        protected override void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            OnUpdate?.Invoke();
            OnLateUpdate?.Invoke();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            OnDraw?.Invoke();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Exits the program.
        /// </summary>
        public static void Quit()
        {
            Debug.WriteLine("Quitting...");
            Instance.Exit();
        }
    }
}