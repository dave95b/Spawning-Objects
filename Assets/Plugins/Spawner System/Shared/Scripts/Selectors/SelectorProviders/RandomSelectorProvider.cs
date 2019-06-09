using UnityEngine;
using System.Collections.Generic;
using NaughtyAttributes;

namespace SpawnerSystem.Shared
{
    public class RandomSelectorProvider : SelectorProvider
    {
        [SerializeField, ReadOnly]
        private int objectCount;

        private RandomSelector selector;
        public override ISelector Selector
        {
            get
            {
                if (selector is null)
                    selector = new RandomSelector(objectCount);

                return selector;
            }
        }

        public override void Initialize(GameObject[] gameObjects)
        {
            objectCount = gameObjects.Length;
        }
    }
}