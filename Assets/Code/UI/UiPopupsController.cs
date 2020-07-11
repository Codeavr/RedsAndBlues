using System;
using RedsAndBlues.Blobs;
using RedsAndBlues.UI;
using UnityEngine;

namespace RedsAndBlues
{
    public class UiPopupsController : MonoBehaviour
    {
        [SerializeField]
        private UiWinPopup _winPopup;

        private GameWinObserver _winObserver;

        public void Resolve(GameWinObserver gameWinObserver)
        {
            _winObserver = gameWinObserver;

            _winObserver.WinnerFoundEvent += OnWinnerFound;
        }

        private void OnWinnerFound(BlobColor color)
        {
            _winPopup.Show(color, () => _winPopup.gameObject.SetActive(false));
            _winPopup.gameObject.SetActive(true);
        }
    }
}