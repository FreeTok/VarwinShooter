using System;
using UnityEngine;
using Varwin;

namespace WeaponLibrary
{
    public class TargetBehaviour : MonoBehaviour
    {
        public int CountMarks = 4;
        public float HitForce = 100.0f;

        private Rigidbody _mainBody;

        private void Start()
        {
            _mainBody = GetComponentInParent<Rigidbody>();
        }

        public void Hit(Vector3 hitPoint)
        {
            _mainBody.AddExplosionForce(HitForce, hitPoint, 0.5f);
        }
    }
}