using System;
using RedsAndBlues.Blobs;
using UnityEngine;
using UnityEngine.UI;

namespace RedsAndBlues.UI
{
    public class UiWinPopup : MonoBehaviour
    {
        [SerializeField]
        private UiLabel _winnerColorLabel;

        [SerializeField]
        private UiLabel _simulationTimeLabel;

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

        public void Show(BlobColor winnerColor, int simulationDuration, Action okButtonCallback)
        {
            _okButtonCallback = okButtonCallback;
            _winnerColorLabel.SetValue(winnerColor.ToString());
            _simulationTimeLabel.SetValue(simulationDuration.ToString());
        }
    }
}