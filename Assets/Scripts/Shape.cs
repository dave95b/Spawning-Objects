using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer renderer;

        private static int colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock propertyBlock;

        public Material Material
        {
            set => renderer.material = value;
        }

        public Color Color
        {
            set
            {
                //renderer.material.color = value;
                if (propertyBlock is null)
                    propertyBlock = new MaterialPropertyBlock();

                propertyBlock.SetColor(colorPropertyId, value);
                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        private void Reset()
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }
}