using UnityEngine;

namespace RedsAndBlues.GameArea
{
    public class GameAreaView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Vector2 _baseSize;

        [SerializeField]
        private Vector2 _sizeOffset;

        public void Resolve(GameAreaSettings settings)
        {
            var finalSize = _baseSize;
            finalSize.Scale(new Vector2(settings.Width, settings.Height));
            _spriteRenderer.size = finalSize + _sizeOffset;
        }
    }
}