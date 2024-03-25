using System;
using System.IO;
using Services.Utils;
using UnityEditor;
using UnityEngine;

namespace Services.Singletons
{
    public abstract class SingletonScriptable<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T GetInstance() => _instance ??= Resources.Load<T>(typeof(T).Name);

#if UNITY_EDITOR
        // InitializeOnLoad can't be called on types with generics, so children need to call this method
        protected static void VerifyLocationImp()
        {
            var path = GetAssetPath();
            if (path == null)
                return;
            var fileName = Path.GetFileNameWithoutExtension(path);
            var directoryPath = Path.GetDirectoryName(path)!;
            if (fileName != typeof(T).Name)
            {
                Debug.LogError($"{fileName} does not match {typeof(T).Name}");
                return;
            }

            var directory = new DirectoryInfo(directoryPath);
            if (directory.Name.Equals("resources", StringComparison.InvariantCultureIgnoreCase) == false)
                Debug.LogError($"{typeof(T).Name} asset must be directly inside Resources folder");
        }

        protected static string GetAssetPath()
        {
            var assets = AssetDatabaseUtil.LoadAssetsByType<T>();
            switch (assets.Count)
            {
                case 0:
                    Debug.LogError($"{typeof(T).Name} asset does not exists");
                    return null;
                case > 1:
                    Debug.LogError($"Found multiple instances of {typeof(T).Name}");
                    return null;
                default:
                {
                    var asset = assets[0];
                    return AssetDatabase.GetAssetPath(asset);
                }
            }
        }
#endif
    }
}