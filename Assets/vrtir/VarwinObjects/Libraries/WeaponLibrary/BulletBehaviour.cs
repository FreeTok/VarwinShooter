using System;
using UnityEngine;

namespace WeaponLibrary
{
    public class BulletBehaviour : MonoBehaviour
    {
        [HideInInspector] public GameObject Rifle;
        [HideInInspector] public float WallHoleLifeTime;
        [HideInInspector] public float BaseDamage;
        public HoleBehaviour HolePrefab;

        private void Start()
        {
            if (WallHoleLifeTime == 0)
            {
                WallHoleLifeTime = 200f;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == Rifle)
            {
                print("Rifle hitted");
                return;
            }
            
            ContactPoint contact = other.contacts[0];
            Vector3 rot = contact.normal;
            Vector3 pos = contact.point;
            
            var holeInstance = Instantiate(HolePrefab, pos, Quaternion.LookRotation(rot));
            holeInstance.MaxTime = WallHoleLifeTime;
            var holeSpriteTransform = holeInstance.transform;
            
            holeSpriteTransform.Translate(Vector3.forward * 0.001f);
            
            holeSpriteTransform.parent = other.transform;
            holeInstance.gameObject.SetActive(true);
            print(holeInstance.name);
            
            damage_get handler = other.collider.gameObject.GetComponent<damage_get>();

            if (handler)
            {
                print("Enemy hitted");
                handler.TakeDamage(BaseDamage);
            }
            else
            {
                print("Miss");
            }
            
            Destroy(this.gameObject);
        }
    }
}