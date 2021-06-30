using UnityEngine;
using Varwin;
using Varwin.PlatformAdapter;
using Varwin.Public;

namespace Varwin.Types.Weapon_ccc5102640fe48729f5919dc3f07e470
{
    [VarwinComponent(English: "Weapon")]
    public class Weapon : VarwinObject
    {
        public bool bIsGrabbed = false;
        public Transform MuzzleTransform;

        private GameObject Bullet;

        [Variable(English: "Bullet", Russian: "Пуля")]
        public GameObject BulletPanel
        {
            get => Bullet;
            internal set => Bullet = value;
        }

        private void Update()
        {
            GameObject rightHand = InputAdapter.Instance?.PlayerController?.Nodes?.RightHand?.GameObject;
            if(rightHand)
            {
                var rightHandEvents = InputAdapter.Instance?.ControllerInput?.ControllerEventFactory?.GetFrom(rightHand);
                if(rightHandEvents != null)
                {
                    if(bIsGrabbed)
                    {
                        if (rightHandEvents.IsTriggerPressed())
                        {
                            Shot(Bullet);
                        }
                    }
                }
            }
        }

        public void StartGrab()
        {
            bIsGrabbed = true;
        }

        public void EndGrab()
        {
            bIsGrabbed = false;
        }

        void Shot(GameObject BulletPrefab)
        {
            GameObject Bullet = Instantiate(BulletPrefab, MuzzleTransform);
            if (Bullet == null) return;

            Rigidbody BulletRb = Bullet.AddComponent<Rigidbody>();

            if (BulletRb)
            {
                BulletRb.AddForce(MuzzleTransform.forward * 20f);
            }
        }
    }
}
