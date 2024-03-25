using System.Collections.Generic;
using UnityEngine;

namespace Services.Utils
{
    public static class TransformUtils
    {
        private static Transform[] GetChildren(this Transform transform)
        {
            var childCount = transform.childCount;
            var children = new Transform[childCount];
            for (int i = 0; i < childCount; i++) children[i] = transform.GetChild(i);
            return children;
        }
        
        public static List<Transform> GetDeepChildren(this Transform transform)
        {
            var queue = new Queue<Transform>(256);
            var children = new List<Transform>(256);
            AddChildren(queue, transform);

            static void AddChildren(Queue<Transform> queue, Transform parent)
            {
                var mainChildren = parent.GetChildren();
                var childCount = mainChildren.Length;
                for (int i = 0; i < childCount; i++) queue.Enqueue(mainChildren[i]);
            }

            while (queue.Count > 0)
            {
                var child = queue.Dequeue();
                children.Add(child);
                AddChildren(queue, child);
            }

            return children;
        }
    }
}