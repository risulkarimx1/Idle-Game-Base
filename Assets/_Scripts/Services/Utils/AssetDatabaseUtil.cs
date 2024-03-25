using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Services.Utils
{
    public static class AssetDatabaseUtil
    {
        public static List<T> LoadAssetsByType<T>() where T : Object
        {
            return LoadAssetsByType(typeof(T)).Cast<T>().ToList();
        }

        public static List<object> LoadAssetsByType(Type type)
        {
            var assets = new List<object>();
#if UNITY_EDITOR
            Assert.IsTrue(type == typeof(Object) || type.IsSubclassOf(typeof(Object)));
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{(type == typeof(GameObject) ? "GameObject" : type)}");
            
            foreach (var id in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(id);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath(path, type);
                if (asset != null) assets.Add(asset);
            }
#endif
            return assets;
        }

        public static List<T> LoadAssetsByName<T>(string name) where T : Object
        {
            var assets = new List<T>();
#if UNITY_EDITOR
            var guids = UnityEditor.AssetDatabase.FindAssets(name);
            foreach (var id in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(id);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                    assets.Add(asset);
            }
#endif
            return assets;
        }

        public static List<string> GetGuidsByName<T>(string name) where T : Object
        {
            var assets = new List<string>();
#if UNITY_EDITOR
            var guids = UnityEditor.AssetDatabase.FindAssets(name);
            foreach (var id in guids)
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(id);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (asset != null)
                    assets.Add(id);
            }
#endif
            return assets;
        }

        public static void SetDirty(this Object @object)
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(@object);
            if (@object is ScriptableObject) return;
            GameObject target = @object as GameObject ?? (@object as Component)!.gameObject;
            if (!UnityEditor.PrefabUtility.IsAnyPrefabInstanceRoot(target)) return;
            UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            List<Transform> children = target.transform.GetDeepChildren();
            int childrenCount = children.Count;
            for (int j = 0; j < childrenCount; j++) UnityEditor.EditorUtility.SetDirty(children[j]);
#endif
        }
    }
}