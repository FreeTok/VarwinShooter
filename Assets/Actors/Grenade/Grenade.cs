using UnityEngine;
using Varwin;
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
        public float t;

        public LineRenderer Line;
        public float LineWidth;
        public Color LineColor;
        public int SigmentsNumber = 20;

        private Vector3 p0Pos;
        private Vector3 p1Pos;
        private Vector3 p2Pos;

        [System.Obsolete]
        private void Start()
        {
            Line.SetWidth(LineWidth, LineWidth);
            Line.SetVertexCount(SigmentsNumber + 1);
            Line.SetColors(LineColor, LineColor);

            ChangeLine();
        }

        private void Update()
        {
            GrenadeMesh.transform.position = Bezier.GetPoint(p0.position, p1.position, p2.position, t);
            GrenadeMesh.transform.rotation = Quaternion.LookRotation(Bezier.GetFirstDerivative(p0.position, p1.position, p2.position, t));

            if (p0Pos != p0.position || p1Pos != p1.position || p2Pos != p2.position)
            {
                ChangeLine();
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
    }
}
