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

        private GameAreaBarrier _barrier;

        public void Resolve(GameAreaBarrier barrier)
        {
            _barrier = barrier;

            _barrier.RebuildEvent += OnBarrierRebuild;
            
            if (barrier.IsBuild)
            {
                OnBarrierRebuild();
            }
        }

        private void OnBarrierRebuild()
        {
            Rescale(_barrier.Settings);
        }

        private void Rescale(GameAreaSettings settings)
        {
            var finalSize = _baseSize;
            finalSize.Scale(new Vector2(settings.Width, settings.Height));
            _spriteRenderer.size = finalSize + _sizeOffset;
        }
    }
}