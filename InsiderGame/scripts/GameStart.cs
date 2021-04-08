
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.insider
{
    public class GameStart : UdonSharpBehaviour
    {
        [SerializeField] private UdonBehaviour gameManager;
        void Start()
        {

        }

        void Interact()
        {
            // var gameState = (int) gameManager.GetProgramVariable("GameState");
            // if(gameState != 0)
            // {
            //     return;
            // }
            gameManager.SendCustomEvent("GameInit");
        }
    }
}