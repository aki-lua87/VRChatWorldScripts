using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.marubatu
{
    public class masu : UdonSharpBehaviour
    {
        // 図形オブジェクト参照
        [SerializeField] private GameObject maru;
        [SerializeField] private GameObject batu;
        [SerializeField] private UdonBehaviour parent;
        public override void Interact()
        {
            // 書き込まれていた場合は挿入不可
            if(maru.activeSelf || batu.activeSelf){
                return;
            }
            parent.SendCustomEvent("GameEndChaeck");
            var isGameEnd = (bool) parent.GetProgramVariable("isGameEnd");
            if(isGameEnd){
                // TODO: ゲーム終了してたら書けない
                return;
            }
            parent.SendCustomEvent("NextFigure");
            var isFigureMaru = (bool) parent.GetProgramVariable("isFigureMaru");

            // 描写
            if(isFigureMaru){
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WriteBatu");
            }else{
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "WriteMaru");
            }
        }

        public void WriteMaru()
        {
            maru.SetActive(true);
        }

        public void WriteBatu()
        {
            batu.SetActive(true);
        }
    }
}