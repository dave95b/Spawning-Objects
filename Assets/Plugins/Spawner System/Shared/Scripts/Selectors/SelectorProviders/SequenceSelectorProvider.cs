using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace SpawnerSystem.Shared
{
    public class SequenceSelectorProvider : SelectorProvider
    {
        [SerializeField, ReadOnly]
        private int objectCount;

        [SerializeField]
        private int step;

        [SerializeField]
        private bool pingPong;

        private SequenceSelector selector;
        public override ISelector Selector
        {
            get {
                if (selector is null)
                    selector = new SequenceSelector(objectCount, step, pingPong);

                return selector;
            }
        }

        public override void Initialize(GameObject[] gameObjects)
        {
            objectCount = gameObjects.Length;
        }

        private void OnValidate()
        {
            if (selector != null)
            {
                selector.PingPong = pingPong;
                selector.Step = step;
            }
        }
    }
}