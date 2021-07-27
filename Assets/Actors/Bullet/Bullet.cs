using System;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace Varwin.Types.Bullet_018844b7693a4e2886977d66f956deaf
{
    [VarwinComponent(English: "Bullet")]
    public class Bullet : VarwinObject
    {
        private Rigidbody _rigidbody;

        public float Speed;
        
        [Variable(English: "Speed", Russian: "Скорость")]
        public float SpeedPanel
        {
            get => Speed;
            internal set => Speed = value;
        }
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(transform.forward * (Speed * Time.fixedTime));
        }
    }
}
