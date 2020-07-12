using RedsAndBlues.Blobs;
using UnityEngine;

namespace RedsAndBlues.UI
{
    public class UiPopupsController : MonoBehaviour
    {
        [SerializeField]
        private UiWinPopup _winPopup;

        private GameWinObserver _winObserver;
        private GameBehaviour _game;
        private SimulationTime _time;

        public void Resolve(GameWinObserver gameWinObserver, GameBehaviour game, SimulationTime time)
        {
            _time = time;
            _game = game;
            _winObserver = gameWinObserver;

            _winObserver.WinnerFoundEvent += OnWinnerFound;
        }

        private void OnWinnerFound(BlobColor color)
        {
            _winPopup.Show(color, Mathf.CeilToInt(_time.ElapsedTime), () =>
            {
                _winPopup.gameObject.SetActive(false);
                _game.StartNewGame();
            });
            _winPopup.gameObject.SetActive(true);
        }
    }
}