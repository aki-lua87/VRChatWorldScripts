using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class Reset : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour gameManager;

        void Interact()
        {
            gameManager.SendCustomEvent("Reset");
        }
    }
}