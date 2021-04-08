
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class PlayGame : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour gameManager;
        [SerializeField] private int playerNum;

        void Interact()
        {
            gameManager.SendCustomEvent("StartPlayer"+playerNum.ToString());
        }
    }
}