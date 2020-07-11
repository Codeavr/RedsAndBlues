using System;
using TMPro;
using UnityEngine;

namespace RedsAndBlues.UI
{
    public class UiLabel : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _textComponent;

        [SerializeField, Multiline]
        private string _formatString;

        [SerializeField]
        private string[] _defaultValues;

        public void SetValue(string value)
        {
            _textComponent.text = string.Format(_formatString, value);
        }

        public void SetValue(params string[] values)
        {
            _textComponent.text = string.Format(_formatString, values);
        }

        private void OnValidate()
        {
            if (_textComponent == null) return;

            try
            {
                SetValue(_defaultValues);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}