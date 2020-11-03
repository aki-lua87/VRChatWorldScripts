using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    // ON/OFFのInteractを発火させるオブジェクトが変更されるケース
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitch2Local")]
    public class TogglObjectSwitch2Local : UdonSharpBehaviour
    {
        private bool isSwitchedOn = false;
        [SerializeField] private GameObject[] TargetObject;
        [SerializeField] private GameObject ON_Swittch;
        [SerializeField] private GameObject OFF_Switch;

        public override void Interact()
        {
            PushSwitch();
        }

        // 対象のオブジェクトをトグル、ONスイッチを非表示 OFFスイッチを表示
        private void PushSwitch()
        {
            for (var i = 0; i < TargetObject.Length; i++)
            {
                TargetObject[i].SetActive(!TargetObject[i].activeSelf);
            }
            ON_Swittch.SetActive(!ON_Swittch.activeSelf);
            OFF_Switch.SetActive(!OFF_Switch.activeSelf);
            isSwitchedOn = !isSwitchedOn;
        }
    }
}