using UnityEngine;
using System.Collections.Generic;

namespace SpawnerSystem.Shared
{
    public class SequenceSelector : ISelector
    {
        public int Step { get; set; }

        private bool pingPong;
        public bool PingPong
        {
            get => pingPong;
            set
            {
                pingPong = value;
                if (!pingPong)
                    direction = 1; 
            }
        }

        private readonly int objectCount;
        private int currentIndex = -1;
        private int direction = 1;

        public SequenceSelector(int objectCount, int step, bool pingPong)
        {
            this.objectCount = objectCount;
            PingPong = pingPong;
            Step = step;
        }

        public int SelectIndex()
        {
            currentIndex += direction * Step;
            if (currentIndex >= objectCount || currentIndex < 0)
                HandleOutOfBoundsIndex();

            currentIndex = Mathf.Clamp(currentIndex, 0, objectCount - 1);

            return currentIndex;
        }

        private void HandleOutOfBoundsIndex()
        {
            if (PingPong)
            {
                direction = -direction;
                currentIndex += direction * 2;
            }
            else
                currentIndex = currentIndex % objectCount;
        }
    }
}
