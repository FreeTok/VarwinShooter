using UnityEngine;
using Varwin;
using Varwin.PlatformAdapter;
using Varwin.Public;

namespace Varwin.Types.Weapon_ccc5102640fe48729f5919dc3f07e470
{
    [VarwinComponent(English: "Weapon")]
    public class Weapon : VarwinObject
    {
        private bool bIsGrabbed = false;
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

        public void StartGrab()
        {
            bIsGrabbed = true;
        }

        public void EndGrab()
        {
            bIsGrabbed = false;
        }

        [Action(English: "Shot", Russian: "Выстрел")]
        public void Shot(Wrapper BulletPrefab, float Speed = 20f)
        {
            GameObject rightHand = InputAdapter.Instance?.PlayerController?.Nodes?.RightHand?.GameObject;
            if (rightHand)
            {
                var rightHandEvents = InputAdapter.Instance?.ControllerInput?.ControllerEventFactory?.GetFrom(rightHand);
                if (rightHandEvents != null)
                {
                    if (rightHandEvents.IsTriggerPressed())
                    {
                        //MakeShot(BulletPrefab, Speed);
                        Destroy(this.gameObject);
                        SpawnObject(BulletPrefab);
                        Destroy(this.gameObject);
                    }

                }
            }
        }

        void SpawnObject(Wrapper SpawnPrefab)
        {
            GameObject Bullet = ObjectManager.Instantiate(SpawnPrefab.GetGameObject(), MuzzleTransform);
            ObjectManager.Instantiate(SpawnPrefab.GetGameObject(), MuzzleTransform);
        }

        void MakeShot(Wrapper BulletPrefab, float Speed)
        {
            GameObject Bullet = ObjectManager.Instantiate(BulletPrefab.GetGameObject(), MuzzleTransform);
            if (Bullet == null) return;

            Debug.LogWarning("OK");

            Rigidbody BulletRb = Bullet.AddComponent<Rigidbody>();

            if (BulletRb)
            {
                thisRB.AddForce(MuzzleTransform.forward * Speed);
            }
        }
    }
}

