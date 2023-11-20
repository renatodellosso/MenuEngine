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

        public static void LoadAsset<T>(string path)
        {
            instance.assets.TryAdd(path, Engine.Instance.Content.Load<T>(path));
        }

        public static void AddAsset<T>(string path, T asset)
        {
            instance.assets.TryAdd(path, asset);
        }

        public static T GetAsset<T>(string path)
        {
            return (T)instance.assets[path];
        }
    }
}
