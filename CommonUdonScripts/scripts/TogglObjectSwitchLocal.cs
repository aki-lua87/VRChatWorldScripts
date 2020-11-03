using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

namespace aki_lua87.UdonScripts.Common
{
    // ON/OFFのInteractを発火させるオブジェクトが同一であること
    [AddComponentMenu("aki_lua87/UdonScripts/TogglObjectSwitchLocal")]
    public class TogglObjectSwitchLocal : UdonSharpBehaviour
    {
        [SerializeField] private GameObject[] TargetObject;

        public override void Interact()
        {
            for(var i = 0; i<TargetObject.Length; i++)
            {
                TargetObject[i].SetActive(!TargetObject[i].activeSelf);
            }
        }
    }
}