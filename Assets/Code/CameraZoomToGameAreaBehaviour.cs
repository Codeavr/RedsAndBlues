using RedsAndBlues.GameArea;
using UnityEngine;

namespace RedsAndBlues
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoomToGameAreaBehaviour : MonoBehaviour
    {
        private Camera _camera;
        private GameAreaBarrier _barrier;

        public void Resolve(GameAreaBarrier barrier)
        {
            _barrier = barrier;
        }

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (_barrier == null) return;

            Scale();
        }

        private void Scale()
        {
            _camera.orthographicSize = 1;

            var size = new Vector2(_barrier.Settings.Width, _barrier.Settings.Height);

            var viewportSize = _camera.WorldToViewportPoint(size);

            _camera.orthographicSize = Mathf.Max(viewportSize.x, viewportSize.y);
        }
    }
}