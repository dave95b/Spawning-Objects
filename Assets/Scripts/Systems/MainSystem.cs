using UnityEngine;
using System.Collections;
using Unity.Jobs;
using NaughtyAttributes;

namespace Systems
{
    public class MainSystem : MonoBehaviour
    {
        [SerializeField, ReorderableList]
        private GameSystem[] systems;

        private JobHandle handle;

        private void Update()
        {
            handle.Complete();
            foreach (var system in systems)
                handle = system.OnUpdate(handle);
        }

        private void LateUpdate()
        {
            
        }

#if UNITY_EDITOR
        [Button]
        private void FindSystems()
        {
            systems = GetComponentsInChildren<GameSystem>();

            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }
}