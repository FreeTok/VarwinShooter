using UnityEngine;
using Varwin;
using Varwin.PlatformAdapter;
using Varwin.Public;

namespace Varwin.Types.Weapon_ccc5102640fe48729f5919dc3f07e470
{
    [VarwinComponent(English: "Weapon")]
    public class Weapon : VarwinObject
    {
        public Transform MuzzleTransform;
        private InteractableObjectBehaviour InterectBehaviour;

        private bool Shoted;

        private void Start()
        {
            InterectBehaviour = GetComponent<InteractableObjectBehaviour>();
            Shoted = false;
        }

        [Action(English: "Shot", Russian: "Выстрел")]
        public void Shot(Wrapper BulletPrefab, float Speed = 20f)
        {
            if (Shoted) return;
            if (InterectBehaviour.IsGrabbed && InterectBehaviour.IsUsed)
            {
                Debug.LogWarning("Shot started");
                Shoted = true;
                GameObject Bullet = ObjectManager.Instantiate(BulletPrefab.GetGameObject(), MuzzleTransform);
                Rigidbody BulletRb;

                if (Bullet == null) return;
                Debug.LogWarning("OK");

                BulletRb = Bullet.GetComponent<Rigidbody>() == null
                    ? Bullet.AddComponent<Rigidbody>()
                    : Bullet.GetComponent<Rigidbody>();

                BulletRb.AddForce(MuzzleTransform.forward * Speed);
            }
        }

        public void Reload()
        {
            Shoted = false;
        }
    }
}