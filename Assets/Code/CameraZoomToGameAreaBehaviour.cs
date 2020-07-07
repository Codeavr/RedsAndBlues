using RedsAndBlues.Code.GameArea;
using UnityEngine;

namespace RedsAndBlues.Code
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoomToGameAreaBehaviour : MonoBehaviour
    {
        private Camera _camera;
        private GameAreaSettings _areaSettings;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            Scale();
        }

        public void Initialize(GameAreaSettings areaSettings)
        {
            _areaSettings = areaSettings;
        }

        private void Scale()
        {
            _camera.orthographicSize = 1;

            var size = new Vector2(_areaSettings.Width, _areaSettings.Height);

            var viewportSize = _camera.WorldToViewportPoint(size);

            _camera.orthographicSize = Mathf.Max(viewportSize.x, viewportSize.y);
        }
    }
}