using RedsAndBlues.ECS.PhysicsEngine;
using RedsAndBlues.ECS.PhysicsEngine.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace RedsAndBlues.Utils
{
    public class GizmosDebugger : MonoBehaviour
    {
        [SerializeField]
        private bool _isEnabled;

        [SerializeField]
        private float _extrude = 0.1f;

        private EntityManager _manager;

        public void Resolve(EntityManager manager)
        {
            _manager = manager;
        }

        private void OnDrawGizmos()
        {
            if (!_isEnabled) return;
            if (!Application.isPlaying) return;

            var query = _manager.CreateEntityQuery(
                typeof(CollisionInfoElementData),
                typeof(CircleColliderComponent),
                typeof(Translation));

            var entities = query.ToEntityArray(Allocator.TempJob);

            for (int i = 0; i < entities.Length; i++)
            {
                bool isColliding = _manager.GetBuffer<CollisionInfoElementData>(entities[i]).Length > 0;
                var collider = _manager.GetComponentData<CircleColliderComponent>(entities[i]);
                float radius = collider.Radius + _extrude;

                var position = _manager.GetComponentData<Translation>(entities[i]).Value;

                if (isColliding)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = collider.Group == CollisionLayer.Blue ? Color.blue : Color.red;
                }

                Gizmos.DrawWireSphere(new Vector3(position.x, position.y, 0), radius);
            }

            entities.Dispose();
        }
    }
}