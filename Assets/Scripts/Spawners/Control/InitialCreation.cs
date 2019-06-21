using UnityEngine;
using System.Collections;
using NaughtyAttributes;

namespace Core
{
    public class InitialCreation : MonoBehaviour
    {
        [SerializeField]
        private Game game;

        [SerializeField, MinValue(1)]
        private int count;


        private void Start()
        {
            StartCoroutine(CreateDelayed());
        }

        private IEnumerator CreateDelayed()
        {
            yield return null;
            yield return new WaitForEndOfFrame();
            game.Create(count);
        }
    }
}