using System;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class MagazineBehaviour : VarwinObject
    {
        private int _ammo = 8;

        public GameObject[] Meshes;

        private Rigidbody _rigidbody;
        
        public Rigidbody Rigidbody => GetRigidBody();

        private Rigidbody GetRigidBody()
        {
            if (!_rigidbody)
            {
                _rigidbody = GetComponent<Rigidbody>();
            }

            return _rigidbody;
        }

        private void Awake()
        {
            UpdateMesh();
        }

        [Variable(English:"Ammo", Russian:"Количество патронов")]
        [VarwinInspector(English:"Ammo", Russian:"Количество патронов")]
        public int Ammo
        {
            get => _ammo;
            set
            {
                _ammo = value;
                UpdateMesh();
            }
        }

        public bool GetAmmo()
        {
            if (Ammo == 0)
            {
                return false;
            }

            Ammo--;
            return true;
        }

        [Checker(English:"Is empty", Russian: "Пустой ли")]
        public bool IsEmpty()
        {
            return Ammo <= 0;
        }

        [Checker(English:"Last ammo", Russian: "Последний патрон")]
        public bool IsLastAmmo()
        {
            return Ammo == 1;
        }
        
        private void UpdateMesh()
        {
            foreach (var mesh in Meshes)
            {
                mesh.SetActive(false);
            }
            
            Meshes[Mathf.Clamp(Ammo, 0, Meshes.Length - 1)].SetActive(true);
        }

        public void FixPosition(Vector3 position)
        {
            transform.position = position;
        }

        private void LateUpdate()
        {
            
            Rigidbody.isKinematic = false;
        }
    }
}