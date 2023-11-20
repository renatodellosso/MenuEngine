using MenuEngine.src.elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MenuEngine.src
{
    public abstract class Project
    {

        public RootElement RootElement { get; private set; }

        /// <summary>
        /// This method is called before <see cref="Engine.Instance"/> exists.
        /// </summary>
        public Project()
        {

        }

        /// <summary>
        /// Called after <see cref="Engine.Instance"/> exists."/>
        /// </summary>
        internal void Initialize()
        {
            RootElement = new RootElement();

            OnInitialize();
        }

        public abstract void OnInitialize();

        /// <summary>
        /// Call <see cref="Assets.LoadAsset{T}(string)"/> in here to load assets.
        /// </summary>
        public virtual void LoadAssets()
        {
            Texture2D blankTexture = new(Engine.Instance.GraphicsDevice, 1, 1);
            blankTexture.SetData(new[] { Color.White });
            Assets.AddAsset("BlankTexture", blankTexture);
        }

    }
}