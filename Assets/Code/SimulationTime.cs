using System;
using UnityEngine;

namespace RedsAndBlues
{
    [Serializable]
    public class SimulationTime : ITickable
    {
        [SerializeField]
        private float _elapsedTime;

        [SerializeField]
        private float _deltaTime;

        [SerializeField]
        private float _speed = 1f;
        
        public float ElapsedTime => _elapsedTime;
        public float DeltaTime => _deltaTime;

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        bool ITickable.IsEnabled { get; set; } = true;
        
        void ITickable.Tick()
        {
            _deltaTime = Time.deltaTime * _speed;

            _elapsedTime += _deltaTime;
        }

        public void Restore(SimulationTime source)
        {
            _elapsedTime = source._elapsedTime;
            _deltaTime = source._deltaTime;
            _speed = source._speed;
        }
    }
}