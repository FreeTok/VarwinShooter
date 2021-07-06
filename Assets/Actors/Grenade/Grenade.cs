using UnityEngine;
using Varwin;
using Varwin.PlatformAdapter;
using Varwin.Public;

namespace Varwin.Types.Grenade_ffc65bd73c4347d4ac8cb4e47e478bd4
{
    [ExecuteAlways]
    [VarwinComponent(English: "Grenade")]
    public class Grenade : VarwinObject
    {
        public GameObject GrenadeMesh;

        public Transform p0;
        public Transform p1;
        public Transform p2;
        
        [Range(0, 1)]
        private float t;
        public float Speed;

        public LineRenderer Line;
        public float LineWidth;
        public int SigmentsNumber = 20;

        private Vector3 p0Pos;
        private Vector3 p1Pos;
        private Vector3 p2Pos;

        private Bezier Bezier;

        public bool Grabbed = false;

        [System.Obsolete]
        private void Start()
        {
            Line.SetWidth(LineWidth, LineWidth);
            Line.SetVertexCount(SigmentsNumber + 1);

            ChangeLine();

            Bezier = GetComponent<Bezier>();
            
            Line.gameObject.SetActive(false);
        }

        private void FixedUpdate()
        {
            GrenadeMesh.transform.position = Bezier.GetPoint(p0.position, p1.position, p2.position, t);
            GrenadeMesh.transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(p0.position, p1.position, p2.position, t));

            if (p0Pos != p0.position || p1Pos != p1.position || p2Pos != p2.position)
            {
                ChangeLine();
            }

            if (Grabbed)
            {
                GameObject rightHand = InputAdapter.Instance?.PlayerController?.Nodes?.RightHand?.GameObject;
                if (rightHand)
                {
                    var rightHandEvents =
                        InputAdapter.Instance?.ControllerInput?.ControllerEventFactory?.GetFrom(rightHand);
                    if (rightHandEvents != null)
                    {
                        if (rightHandEvents.IsTriggerPressed())
                        {
                            Line.gameObject.SetActive(true);
                        }
                        
                        else if (rightHandEvents.IsTriggerReleased())
                        {
                            Line.gameObject.SetActive(false);
                            StartTimer();
                        }
                    }
                }
            }
        }

        private void ChangeLine()
        {
            for (int i = 0; i < SigmentsNumber + 1; i++)
            {
                float Parameter = (float)i / SigmentsNumber;
                Vector3 Point = Bezier.GetPoint(p0.position, p1.position, p2.position, Parameter);

                Line.SetPosition(i, Point);
            }

            p0Pos = p0.position;
            p1Pos = p1.position;
            p2Pos = p2.position;
        }

        private void StartTimer(bool Loop = false)
        {
            if (Loop)
            {
                if (t > 1)
                {
                    t = 0;
                }
            }
            
            t += 1 * Speed * Time.fixedDeltaTime;
        }

        public void OnGrabStart()
        {
            Grabbed = true;
        }
        
        public void OnGrabEnd()
        {
            Grabbed = false;
        }
    }
}