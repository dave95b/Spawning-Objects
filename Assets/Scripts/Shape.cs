using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer renderer;

        private int id;
        public int Id => id;


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
                if (propertyBlock is null)
                    propertyBlock = new MaterialPropertyBlock();

                propertyBlock.SetColor(colorPropertyId, value);
                renderer.SetPropertyBlock(propertyBlock);
            }
        }

        public Vector3 AngularVelocity { get; set; }
        public Vector3 Velocity { get; set; }


        private void Awake()
        {
            id = GetInstanceID();
        }

        public void CustomUpdate(float deltaTime)
        {
            transform.Rotate(AngularVelocity * deltaTime);
            transform.localPosition += Velocity * deltaTime;
        }

        private void Reset()
        {
            renderer = GetComponent<MeshRenderer>();
        }
    }
}