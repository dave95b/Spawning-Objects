using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer renderer;

        public Material Material
        {
            set => renderer.material = value;
        }

        private void Reset()
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }
}