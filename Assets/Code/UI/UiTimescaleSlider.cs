using System;
using System.Collections;
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

        private SimulationTime _time;

        public void Resolve(SimulationTime time)
        {
            _time = time;
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _time != null);

            _time.Speed = _initialValue;

            _slider.minValue = _minMaxValues.x;
            _slider.maxValue = _minMaxValues.y;
            _slider.value = _initialValue;
            _slider.onValueChanged.AddListener(OnValueChanged);

            Redraw();
        }

        private void Update()
        {
            if (_time == null) return;

            if (Math.Abs(_slider.value - _time.Speed) > .0001f)
            {
                Redraw();
            }
        }

        private void Redraw()
        {
            if (_time == null) return;

            _slider.value = _time.Speed;

            _currentValueLabel.text = _time.Speed.ToString();

            _minValueLabel.text = _minMaxValues.x.ToString();
            _maxValueLabel.text = _minMaxValues.y.ToString();

            float t = Mathf.InverseLerp(_minMaxValues.x, _minMaxValues.y, _time.Speed);

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

            _time.Speed = Mathf.Clamp(value, _minMaxValues.x, _minMaxValues.y);

            Redraw();
        }

        private void OnValidate()
        {
            if (Application.isPlaying) return;

            _slider.value = _initialValue;

            _slider.minValue = _minMaxValues.x;
            _slider.maxValue = _minMaxValues.y;

            Redraw();
        }
    }
}