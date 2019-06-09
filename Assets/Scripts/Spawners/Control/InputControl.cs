using UnityEngine;
using System.Collections;

namespace Core
{
    public class InputControl : MonoBehaviour
    {
        [SerializeField]
        private Game game;

        [SerializeField]
        private KeyCode createKey = KeyCode.C,
            removeAllKey = KeyCode.R,
            removeRandomKey = KeyCode.V;

        private void Update()
        {
            if (Input.GetKeyDown(createKey))
                game.Create();
            else if (Input.GetKeyDown(removeAllKey))
                game.RemoveAll();
            else if (Input.GetKeyDown(removeRandomKey))
                game.RemoveRandom();
        }
    }
}