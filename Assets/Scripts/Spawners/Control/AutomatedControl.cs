using UnityEngine;
using System.Collections;
using NaughtyAttributes;

namespace Core
{
    public class AutomatedControl : MonoBehaviour
    {
        [SerializeField]
        private Game game;

        [SerializeField, MinValue(0.1f)]
        private float createTime, destroyTime;

        private float _createProgress, _destroyProgress;

        private void Update()
        {
            _createProgress += Time.deltaTime;
            _destroyProgress += Time.deltaTime;

            if (_createProgress >= createTime)
            {
                game.Create();
                _createProgress = 0f;
            }
            else if (_destroyProgress >= destroyTime)
            {
                game.RemoveRandom();
                _destroyProgress = 0f;
            }
        }
    }
}