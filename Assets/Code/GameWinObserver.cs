using System;
using RedsAndBlues.Blobs;
using RedsAndBlues.ECS.General.Components;
using RedsAndBlues.ECS.PhysicsEngine;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Collections;
using Unity.Entities;

namespace RedsAndBlues
{
    public class GameWinObserver : ITickable
    {
        public event Action<BlobColor> WinnerFoundEvent;
        public event Action<BlobColor> WinningColorChangedEvent;

        public bool IsEnabled { get; set; }
        public BlobColor WinningColor => _winningColor;

        private EntityQuery _blobsQuery;
        private BlobColor _winningColor;
        private GameBehaviour _gameBehaviour;

        public GameWinObserver(EntityManager manager)
        {
            _blobsQuery = manager.CreateEntityQuery(typeof(CircleColliderComponent));
        }

        void ITickable.Tick()
        {
            var colliders = _blobsQuery.ToComponentDataArray<CircleColliderComponent>(Allocator.TempJob);

            int redCount = 0;
            int blueCount = 0;

            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider.Group == CollisionLayer.Blue)
                {
                    blueCount++;
                }
                else if (collider.Group == CollisionLayer.Red)
                {
                    redCount++;
                }
            }

            var currentWinningColor = redCount > blueCount ? BlobColor.Red : BlobColor.Blue;

            if (currentWinningColor != WinningColor)
            {
                _winningColor = currentWinningColor;
                WinningColorChangedEvent?.Invoke(WinningColor);
            }

            _winningColor = currentWinningColor;

            if (redCount == 0 || blueCount == 0)
            {
                if (redCount == blueCount)
                {
                    WinnerFoundEvent?.Invoke(BlobColor.None);
                }
                else
                {
                    WinnerFoundEvent?.Invoke(redCount > blueCount ? BlobColor.Red : BlobColor.Blue);
                }
            }

            colliders.Dispose();
        }
    }
}