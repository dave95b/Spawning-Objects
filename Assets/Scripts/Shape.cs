using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core
{
    public class Shape : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] renderers;

        private int id;
        public int Id => id;

        public int Count => renderers.Length;

        public Vector3 AngularVelocity { get; set; }
        public Vector3 Velocity { get; set; }


        private static int colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock propertyBlock;


        private void Awake()
        {
            id = GetInstanceID();
        }

        public void SetColor(Color color)
        {
            for (int i = 0; i < Count; i++)
                SetColor(color, i);
        }

        public void SetColor(Color color, int index)
        {
            Assert.IsTrue(index < Count);

            if (propertyBlock is null)
                propertyBlock = new MaterialPropertyBlock();

            propertyBlock.SetColor(colorPropertyId, color);
            renderers[index].SetPropertyBlock(propertyBlock);
        }

        public void SetMaterial(Material material)
        {
            for (int i = 0; i < Count; i++)
                SetMaterial(material, i);
        }

        public void SetMaterial(Material material, int index)
        {
            Assert.IsTrue(index < Count);
            renderers[index].material = material;
        }

        public void CustomUpdate(float deltaTime)
        {
            transform.Rotate(AngularVelocity * deltaTime);
            transform.localPosition += Velocity * deltaTime;
        }

        private void Reset()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }
    }
}