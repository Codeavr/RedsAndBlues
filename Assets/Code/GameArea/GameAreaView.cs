using RedAndBlues.Field;
using UnityEngine;

namespace RedAndBlues.Field
{
    public class GameAreaView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Vector2 _baseSize;

        [SerializeField]
        private Vector2 _sizeOffset;

        public void Initialize(GameAreaSettings settings)
        {
            var finalSize = _baseSize;
            finalSize.Scale(new Vector2(settings.Width, settings.Height));
            _spriteRenderer.size = finalSize + _sizeOffset;
        }
    }
}