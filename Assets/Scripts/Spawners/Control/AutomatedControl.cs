using NaughtyAttributes;
using UnityEngine;

namespace Core
{
    public class AutomatedControl : MonoBehaviour
    {
        [SerializeField]
        private Game game;

        [SerializeField, MinValue(1)]
        private int createAmount = 1;

        [SerializeField, MinValue(0.1f)]
        private float createTime, destroyTime;

        [SerializeField]
        private bool create = true, destroy;

        private float createProgress, destroyProgress;

        private void Update()
        {
            createProgress += Time.deltaTime;
            destroyProgress += Time.deltaTime;

            if (create && createProgress >= createTime)
            {
                game.Create(createAmount);
                createProgress = 0f;
            }
            else if (destroy && destroyProgress >= destroyTime)
            {
                game.RemoveAll();
                destroyProgress = 0f;
            }
        }
    }
}