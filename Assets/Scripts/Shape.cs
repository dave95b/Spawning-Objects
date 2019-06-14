﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Core
{
    public class Shape : MonoBehaviour, IEquatable<Shape>
    {
        [SerializeField]
        private MeshRenderer[] renderers;

        [SerializeField]
        private bool enableCollider;

        private int id;
        public int Id => id;

        public int Count => renderers.Length;


        private static int colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock propertyBlock;


        private void Awake()
        {
            id = GetInstanceID();

            var collider = GetComponent<Collider>();
            collider.enabled = enableCollider;
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

        private void Reset()
        {
            renderers = GetComponentsInChildren<MeshRenderer>();
        }

        public bool Equals(Shape other)
        {
            if (other == null)
                return false;

            return id == other.id;
        }

        public override bool Equals(object other)
        {
            return Equals(other as Shape);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}