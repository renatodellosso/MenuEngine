using Microsoft.Xna.Framework.Graphics;
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

        /// <summary>
        /// Will not add the asset if it is null.
        /// </summary>
        public static T LoadAsset<T>(string path)
        {
            T asset = LoadAssetFromFile<T>(path);

            if (asset == null) return default;

            instance.assets.TryAdd(path, asset);

            return asset;
        }

        public static T LoadAssetFromFile<T>(string path)
        {
            return Engine.Instance.Content.Load<T>(path);
        }

        public static Tilesheet LoadTilesheet(string path, int tileWidth, int tileHeight, int tilePadding)
        {
            Texture2D texture = LoadAssetFromFile<Texture2D>(path);

            Tilesheet tilesheet = new(texture, tileWidth, tileHeight, tilePadding);
            instance.assets.TryAdd(path, tilesheet);

            return tilesheet;
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
