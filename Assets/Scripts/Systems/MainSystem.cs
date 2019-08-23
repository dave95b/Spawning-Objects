using NaughtyAttributes;
using Unity.Jobs;
using UnityEngine;

namespace Systems
{
    public class MainSystem : MonoBehaviour
    {
        [SerializeField, ReorderableList]
        private GameSystem[] systems;

        private JobHandle handle;

        private void Update()
        {
            foreach (var system in systems)
                system.OnUpdate(ref handle);
        }

        private void LateUpdate()
        {
            handle.Complete();
            foreach (var system in systems)
                system.OnLateUpdate();
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