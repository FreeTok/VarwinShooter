using System;
using UnityEngine;
using UnityEngine.Serialization;
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

        public GameObject[] defaultShotVFXs;
        public GameObject[] chargedVFXs;
        public GameObject[] artilleryShotVFXs;
        
        public delegate void OnHitEventHandler(Wrapper targetWrapper);
        public event OnHitEventHandler OnHitTargetEvent;

        public enum EnBulletElement
        {
            Dendro,
            Ice,
            Light,
            Darkness
        };

        public enum EnBulletMode
        {
            DefaultShot,
            ChargedShot,
            ArtilleryShot
        };

        public EnBulletMode bulletMode;
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

            if (Explode)
            {
                GameObject explosion = Instantiate(Explosion, transform.position, transform.rotation);
                explosion.GetComponent<Explosion>().element = bulletElement.ToString();
            }

            OnHitTargetEvent?.Invoke(other.gameObject.GetWrapper());
            
            ContactPoint contact = other.contacts[0];
            Vector3 rot = contact.normal;
            print(rot);
            Vector3 pos = contact.point;

            var vfX = SpawnVfX(pos, rot);
            
            Destroy(vfX, WallHoleLifeTime);
            //holeInstance = Instantiate(HolePrefab, pos, Quaternion.LookRotation(rot));
            // holeInstance.MaxTime = WallHoleLifeTime;
            // var holeSpriteTransform = holeInstance.transform;
            //
            // holeSpriteTransform.Translate(Vector3.forward * 0.001f);
            //
            // holeSpriteTransform.parent = other.transform;
            // holeInstance.gameObject.SetActive(true);

            damage_get handler = other.collider.gameObject.GetComponent<damage_get>();

            if (handler)
            {
                handler.TakeDamage(BaseDamage, bulletElement.ToString());
                Debug.Log(BaseDamage + " enemy damage");
            }

            Destroy(gameObject);
        }

        private GameObject SpawnVfX(Vector3 position, Vector3 rotation)
        {
            switch (bulletMode)
            {
                case EnBulletMode.DefaultShot:
                {
                    if (defaultShotVFXs.Length == 0)
                    {
                        print("VFXs are null");
                        return null;
                    }
                    
                    return Instantiate(defaultShotVFXs[GetBulletElementID()], position, Quaternion.LookRotation(rotation));
                }

                case EnBulletMode.ChargedShot:
                {
                    if (defaultShotVFXs.Length == 0)
                    {
                        print("VFXs are null");
                        return null;
                    }
                    
                    return Instantiate(chargedVFXs[GetBulletElementID()], position, Quaternion.LookRotation(rotation));
                }
                
                case EnBulletMode.ArtilleryShot:
                {
                    if (defaultShotVFXs.Length == 0)
                    {
                        print("VFXs are null");
                        return null;
                    }
                    
                    return Instantiate(artilleryShotVFXs[GetBulletElementID()], position, Quaternion.Euler(new Vector3(
                        90 * rotation.z, 90 * rotation.y, 90 * rotation.z)));
                }
            }

            return null;
        }

        private int GetBulletElementID()
        {
            switch (bulletElement)
            {
                case EnBulletElement.Darkness:
                {
                    return 0;
                }
                case EnBulletElement.Dendro:
                {
                    return 1;
                }
                case EnBulletElement.Ice:
                {
                    return 2;
                }
                        
                case EnBulletElement.Light:
                {
                    return 3;
                }
            }
            
            return 0;
        }
    }
}