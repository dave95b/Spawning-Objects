using System.Diagnostics;
using UnityEngine;

namespace SpawnerSystem.Shared
{
    public abstract class SelectorProvider : MonoBehaviour
    {
        public abstract ISelector Selector { get; }
        [Conditional("UNITY_EDITOR")]
        public abstract void Initialize(GameObject[] gameObjects);
    }
}