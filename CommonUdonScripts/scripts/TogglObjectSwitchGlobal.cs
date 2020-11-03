using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{

    // 他と違ってONでオブジェクトがアクティブ OFFでオブジェクトが非アクティブ
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitchGlobal")]
    public class TogglObjectSwitchGlobal : UdonSharpBehaviour
    {
        private bool isSwitchedOnLocal = false;
        [SerializeField] private GameObject[] TargetObject;
        [SerializeField] private GameObject ON_Swittch;
        [SerializeField] private GameObject OFF_Switch;

        private bool[] initObjectState;

        void Start()
        {
            // ターゲットのオブジェクトの初期位相を変数に格納
            initObjectState = new bool[TargetObject.Length];
            for (var i = 0; i < TargetObject.Length; i++)
            {
                initObjectState[i] = TargetObject[i].activeSelf;
            }
            isSwitchedOnLocal = OFF_Switch.activeSelf;
        }

        public override void Interact()
        {
            // 同期変数を弄るためにオーナーを変更
            if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

            if (isSwitchedOnLocal)
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchOFF");
            }
            else
            {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SwitchON");
            }

        }

        public void SwitchON()
        {
            isSwitchedOnLocal = true;
            // TargetObject.SetActive(!initObjectState);
            for (var i = 0; i < TargetObject.Length; i++)
            {
                TargetObject[i].SetActive(initObjectState[i]);
            }
            ON_Swittch.SetActive(!ON_Swittch.activeSelf);
            OFF_Switch.SetActive(!OFF_Switch.activeSelf);
        }

        public void SwitchOFF()
        {
            isSwitchedOnLocal = false;
            // TargetObject.SetActive(initObjectState);
            for (var i = 0; i < TargetObject.Length; i++)
            {
                TargetObject[i].SetActive(!initObjectState[i]);
            }
            ON_Swittch.SetActive(!ON_Swittch.activeSelf);
            OFF_Switch.SetActive(!OFF_Switch.activeSelf);
        }
    }
}