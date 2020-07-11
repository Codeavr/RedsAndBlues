using UnityEngine;
using UnityEngine.UI;

namespace RedsAndBlues.UI
{
    public class UiSaveLoadButtons : MonoBehaviour, ITickable
    {
        [SerializeField]
        private Button _saveButton;

        [SerializeField]
        private Button _loadButton;

        [SerializeField]
        private CanvasGroup _group;

        public bool IsEnabled
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public bool Interactable
        {
            get => _group.interactable;
            set => _group.interactable = value;
        }

        private GameSaveSystem _saveSystem;

        public void Resolve(GameSaveSystem saveSystem)
        {
            _saveSystem = saveSystem;
        }

        private void Awake()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        private void OnSaveButtonClicked()
        {
            _saveSystem.Save();
        }

        void ITickable.Tick()
        {
            _loadButton.interactable = _saveSystem.HaveGameSave;
        }

        private void OnLoadButtonClicked()
        {
            _saveSystem.Load();
        }
    }
}