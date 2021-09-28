using System;
using UnityEngine;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class BulletBehaviour : MonoBehaviour
    {
        [HideInInspector] public GameObject Rifle;
        [HideInInspector] public float WallHoleLifeTime;
        [HideInInspector] public float BaseDamage;
        public HoleBehaviour HolePrefab;
        
        public delegate void OnHitEventHandler(Wrapper targetWrapper);
        public event OnHitEventHandler OnHitTargetEvent;
        
        public enum EnBulletElement
        {
            Dendro,
            Ice,
            Light,
            Darkness
        };
    
        public EnBulletElement bulletElement;

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
            
            OnHitTargetEvent?.Invoke(other.gameObject.GetWrapper());
            
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
                handler.TakeDamage(BaseDamage, bulletElement.ToString());
            }
            else
            {
                print("Miss");
            }
            
            Destroy(this.gameObject);
        }
    }
}