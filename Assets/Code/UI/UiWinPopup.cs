using System;
using RedsAndBlues.Blobs;
using UnityEngine;
using UnityEngine.UI;

namespace RedsAndBlues.UI
{
    public class UiWinPopup : MonoBehaviour
    {
        [SerializeField]
        private UiLabel _uiLabel;

        [SerializeField]
        private Button _okButton;

        private Action _okButtonCallback;

        private void Awake()
        {
            _okButton.onClick.AddListener(OnOkButton);
        }

        private void OnOkButton()
        {
            _okButtonCallback?.Invoke();
        }

        public void Show(BlobColor winnerColor, Action okButtonCallback)
        {
            _okButtonCallback = okButtonCallback;
            _uiLabel.SetValue(winnerColor.ToString());
        }
    }
}