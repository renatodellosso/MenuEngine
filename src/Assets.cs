using System.Collections.Generic;

namespace MenuEngine.src
{
    public class Assets
    {
        private static Assets instance;

        private Dictionary<string, object> assets;

        internal Assets()
        {
            instance = this;

            assets = new Dictionary<string, object>();
        }

        internal void LoadAssets()
        {

        }

        /// <summary>
        /// Will not add the asset if it is null.
        /// </summary>
        public static void LoadAsset<T>(string path)
        {
            T asset = Engine.Instance.Content.Load<T>(path);

            if (asset == null) return;

            instance.assets.TryAdd(path, asset);
        }

        /// <summary>
        /// Directly adds to the asset to <see cref="assets"/>. Does not load the asset from the content folder.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="asset"></param>
        public static void AddAssetDirect<T>(string path, T asset)
        {
            instance.assets.TryAdd(path, asset!);
        }

        public static T? GetAsset<T>(string path)
        {
            return instance.assets.TryGetValue(path, out object? asset) ? (T)asset : default!;
        }
    }
}
