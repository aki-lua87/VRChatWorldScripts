
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class odai_open : UdonSharpBehaviour
    {
        [SerializeField] private GameObject openObject;
        void Start()
        {
            
        }

        void OnPickupUseDown()
        {
            TogglOpenOdai();
        }

        void OnPickupUseUp()
        {
            TogglOpenOdai();
        }

        private void TogglOpenOdai()
        {
            openObject.SetActive(!openObject.activeSelf);
        }
    }
}