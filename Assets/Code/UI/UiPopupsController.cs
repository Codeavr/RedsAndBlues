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
        private GameBehaviour _game;

        public void Resolve(GameWinObserver gameWinObserver, GameBehaviour game)
        {
            _game = game;
            _winObserver = gameWinObserver;

            _winObserver.WinnerFoundEvent += OnWinnerFound;
        }

        private void OnWinnerFound(BlobColor color)
        {
            _winPopup.Show(color, () =>
            {
                _winPopup.gameObject.SetActive(false);
                _game.StartNewGame();
            });
            _winPopup.gameObject.SetActive(true);
        }
    }
}