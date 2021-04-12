using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace aki_lua87.UdonScripts.OnPrehab
{
    public class JoinHistory : UdonSharpBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _toastCanvas;
        [SerializeField] private Text _toastText;
        private int count = 0;

        void Start()
        {
            _text.text = "          Join History";
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            _text.text = $"{_text.text}\n{++count}: {player.displayName} Join";

            // ユーザの前にトーストを出す処理
            var toastText = player.displayName + " \nJoin";
            ToastForUserFronts(toastText);
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            _text.text = $"{_text.text}\n{++count}: {player.displayName} Left";

            var toastText = player.displayName + " \nLeft";
            ToastForUserFronts(toastText);
        }

        private void ToastForUserFronts(string text)
        {
            _toastCanvas.SetActive(false);
            _toastText.text = text;
            _toastCanvas.SetActive(true);
        }
    }
}