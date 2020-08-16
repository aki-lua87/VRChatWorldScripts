using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace aki_lua87.UdonScripts.marubatu
{
    public class marubatsugame : UdonSharpBehaviour
    {
        // [SerializeField] private GameObject[] masu;
        // [SerializeField] private GameObject[] batu;
        [SerializeField] private GameObject[] masu;

        // 次の挿入図形
        public bool isFigureMaru = true;
        public bool isGameEnd = false;
        
        public void GameEndChaeck(){
            // TODO 終了を判定 あまりいらんかも
            isGameEnd = false;
        }

        public void NextFigure(){
            isFigureMaru = !isFigureMaru;
        }

        public void ResetGame(){
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ResetMasu");
        }

        public void ResetMasu()
        {
            isFigureMaru = true;
            foreach(GameObject obj in masu){
                obj.SetActive(false);
            }
        }
    }
}