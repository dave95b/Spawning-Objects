using UnityEngine;
using System.Collections;
using NaughtyAttributes;

namespace Core
{
    public class InputControl : MonoBehaviour
    {
        [SerializeField]
        private Game game;

        [SerializeField, MinValue(1)]
        private int createCount = 10;

        [SerializeField]
        private KeyCode createKey = KeyCode.C,
            removeAllKey = KeyCode.R,
            removeRandomKey = KeyCode.V;

        private void Update()
        {
            if (Input.GetKeyDown(createKey))
                game.Create(createCount);
            else if (Input.GetKeyDown(removeAllKey))
                game.RemoveAll();
            else if (Input.GetKeyDown(removeRandomKey))
                game.RemoveRandom();
        }
    }
}