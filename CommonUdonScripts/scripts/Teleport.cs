
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.Common
{
    [AddComponentMenu("aki_lua87/UdonScripts/Teleport")]
    public class Teleport : UdonSharpBehaviour
    {
        [SerializeField] private Transform to;

        public override void Interact()
        {
            var player = Networking.LocalPlayer;
            player.TeleportTo(to.position, to.rotation);
        }
    }
}