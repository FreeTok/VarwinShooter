using System;
using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Varwin.PlatformAdapter;
using Varwin.Public;

namespace Varwin.Types.HandHoverSetupper_3dd8389454ba49d890ed369c4c784a0a
{
    public class HandHoverSetupper : VarwinObject
    {
        private void Start()
        {
            var distancePointerSettings = ScriptableObject.CreateInstance<DistancePointerSettings>();
            StartCoroutine(ChangeMask(distancePointerSettings.LayerMask));
        }

        private IEnumerator ChangeMask(int mask)
        {
            while (!Player.instance || Player.instance.hands == null)
            {
                yield return new WaitForEndOfFrame();
            }

            foreach (var hand in Player.instance.hands)
            {
                hand.hoverLayerMask = mask;
            }
        }
    }
}