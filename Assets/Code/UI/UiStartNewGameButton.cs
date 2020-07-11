using System;
using UnityEngine;
using UnityEngine.UI;

namespace RedsAndBlues.UI
{
    public class UiStartNewGameButton : MonoBehaviour
    {
        [SerializeField]
        private Button _startButton;

        public bool Interactable
        {
            get => _startButton.interactable;
            set => _startButton.interactable = value;
        }

        private GameBehaviour _game;

        public void Resolve(GameBehaviour game)
        {
            _game = game;
        }

        private void Awake()
        {
            _startButton.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            _game.StartNewGame();
        }
    }
}