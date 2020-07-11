using System;
using RedsAndBlues.Blobs;
using TMPro;
using UnityEngine;

namespace RedsAndBlues.UI
{
    public class UiGameInfo : MonoBehaviour
    {
        [SerializeField]
        private UiLabel _label;

        [SerializeField]
        private string _redColorText = "<color=red>RED</color>";

        [SerializeField]
        private string _blueColorText = "<color=blue>BLUE</color>";

        public void Resolve(GameWinObserver winObserver)
        {
            winObserver.WinningColorChangedEvent += OnWinningColorChanged;
            OnWinningColorChanged(winObserver.WinningColor);
        }

        private void OnWinningColorChanged(BlobColor color)
        {
            _label.SetValue(PickColorText(color));
        }

        private string PickColorText(BlobColor color)
        {
            switch (color)
            {
                case BlobColor.Blue:
                    return _blueColorText;
                case BlobColor.Red:
                    return _redColorText;
                default:
                    return color.ToString();
            }
        }
    }
}