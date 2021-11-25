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
        [HideInInspector] public bool Explode;
        public GameObject Explosion;
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
                return;
            }
            
            if (other.gameObject.CompareTag("Tower"))
            {
                return;
            }

            if (Explode)
            {
                GameObject explosion = Instantiate(Explosion, transform.position, transform.rotation);
                explosion.GetComponent<Explosion>().element = bulletElement.ToString();
            }
            
            OnHitTargetEvent?.Invoke(other.gameObject.GetWrapper());

            ContactPoint contact = other.contacts[0];
            Vector3 rot = contact.normal;
            Vector3 pos = contact.point;

            HoleBehaviour holeInstance;
            holeInstance = Instantiate(HolePrefab, pos, Quaternion.LookRotation(rot));
            holeInstance.MaxTime = WallHoleLifeTime;
            var holeSpriteTransform = holeInstance.transform;
            
            holeSpriteTransform.Translate(Vector3.forward * 0.001f);
            
            holeSpriteTransform.parent = other.transform;
            holeInstance.gameObject.SetActive(true);

            damage_get handler = other.collider.gameObject.GetComponent<damage_get>();

            if (handler)
            {
                handler.TakeDamage(BaseDamage, bulletElement.ToString());
                Debug.Log(BaseDamage + " enemy damage");
            }

            Destroy(gameObject);
        }
    }
}