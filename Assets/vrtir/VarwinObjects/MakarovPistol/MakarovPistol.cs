using WeaponLibrary;

namespace Varwin.Types.MakarovPistol_1d429d7bed2046099bd99355c171bc7c
{
    public class MakarovPistol : WeaponBehaviour
    {
        public HandleMovementsBehaviour mainHandle;

        private void Start()
        {
            mainHandle.Grabbed += MainHandleOnGrabbed;
            mainHandle.Ungrabbed += MainHandleOnUngrabbed;
        }

        private bool _mainHandleGrabbed;

        private void MainHandleOnUngrabbed()
        {
            _mainHandleGrabbed = false;
            ProcessGrab();
        }

        private void MainHandleOnGrabbed()
        {
            _mainHandleGrabbed = true;
            ProcessGrab();
        }

        private void ProcessGrab()
        {
            Rigidbody.isKinematic = _mainHandleGrabbed;
        }
    }
}
