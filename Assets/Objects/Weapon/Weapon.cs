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

        private Wrapper Bullet;

        private Rigidbody thisRB;

        [Variable(English: "Bullet", Russian: "Пуля")]
        public Wrapper BulletPanel
        {
            get => Bullet;
            internal set => Bullet = value;
        }

        private void Start()
        {
            thisRB = GetComponent<Rigidbody>();
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
                            Shot(Bullet.GetGameObject());
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
            GameObject Bullet = ObjectManager.Instantiate(BulletPrefab, MuzzleTransform);
            if (Bullet == null) return;

            Debug.LogWarning("OK");

            Rigidbody BulletRb = Bullet.AddComponent<Rigidbody>();

            if (BulletRb)
            {
                thisRB.AddForce(MuzzleTransform.forward * 20f);
            }
        }
    }
}
