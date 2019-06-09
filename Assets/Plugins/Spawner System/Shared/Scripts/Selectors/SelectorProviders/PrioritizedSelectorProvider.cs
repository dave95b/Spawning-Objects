using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using NaughtyAttributes;
using System.Diagnostics;

namespace SpawnerSystem.Shared
{
    public class PrioritizedSelectorProvider : SelectorProvider
    {
        [SerializeField]
        private List<Entry> priorities;

        private PrioritizedSelector selector;
        public override ISelector Selector
        {
            get
            {
                if (selector is null)
                {
                    int[] priorities = GetPriorities();
                    selector = new PrioritizedSelector(priorities);
                }

                return selector;
            }
        }

        private int[] GetPriorities()
        {
            int length = priorities.Count;
            int[] result = new int[length];
            result[0] = priorities[0].Priority;

            for (int i = 1; i < length; i++)
                result[i] = priorities[i].Priority + result[i - 1];

            return result;
        }

        public override void Initialize(GameObject[] gameObjects)
        {
            foreach (var obj in gameObjects)
            {
                if (!priorities.Any(entry => entry.GameObject == obj))
                    priorities.Add(new Entry(obj));
            }
        }

        [Serializable]
        private struct Entry
        {
            public GameObject GameObject;
            public int Priority;

            public Entry(GameObject provider)
            {
                GameObject = provider;
                Priority = 0;
            }
        }
    }
}