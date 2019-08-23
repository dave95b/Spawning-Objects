using NaughtyAttributes;
using System.Collections;
using UnityEngine;

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