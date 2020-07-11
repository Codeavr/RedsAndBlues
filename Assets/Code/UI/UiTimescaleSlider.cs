using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RedsAndBlues.UI
{
    public class UiTimescaleSlider : MonoBehaviour
    {
        private const float Epsilon = 0.1f;

        [SerializeField]
        private TMP_Text _minValueLabel;

        [SerializeField]
        private TMP_Text _maxValueLabel;

        [SerializeField]
        private TMP_Text _currentValueLabel;

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private Vector2 _minMaxValues;

        [SerializeField]
        private float _initialValue;


        private void Awake()
        {
            _slider.minValue = _minMaxValues.x;
            _slider.maxValue = _minMaxValues.y;
            _slider.value = _initialValue;
            _slider.onValueChanged.AddListener(OnValueChanged);

            Redraw();
        }

        private void Redraw()
        {
            _currentValueLabel.text = Simulation.Speed.ToString();

            _minValueLabel.text = _minMaxValues.x.ToString();
            _maxValueLabel.text = _minMaxValues.y.ToString();

            float t = Mathf.InverseLerp(_minMaxValues.x, _minMaxValues.y, Simulation.Speed);

            _currentValueLabel.color = Color.Lerp(_minValueLabel.color, _maxValueLabel.color, t);
        }

        private void OnValueChanged(float value)
        {
            var intValue = Mathf.RoundToInt(value);
            if (Mathf.Abs(value - intValue) < Epsilon)
            {
                value = intValue;
            }
            else
            {
                value = (float) Math.Round(value, 1, MidpointRounding.AwayFromZero);
            }

            Simulation.Speed = Mathf.Clamp(value, _minMaxValues.x, _minMaxValues.y);

            Redraw();
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            _slider.value = Simulation.Speed;

            _slider.minValue = _minMaxValues.x;
            _slider.maxValue = _minMaxValues.y;

            Redraw();
        }
    }
}