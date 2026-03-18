
using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using Game.Services;
using Game.Enums;
using UnityEngine;

namespace Game.Actors
{
    public class Glass : MonoBehaviour, IInjectWorld
    {
        [SerializeField] private GameObject _glassObject;
        [SerializeField] private GameObject _textObject;
        
        [SerializeField] private Rigidbody[] _shards;
        [SerializeField] private Transform _epicenterTransform;
        [SerializeField] private float _explosionPower;
        [SerializeField] private float _explosionRadius;

        [SerializeField] private float _deltaStartAnim;
        
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.LevelLoseEvent += OnLevelLoseEvent;
        }

        private void OnLevelLoseEvent(EGameLooseType type)
        {
            RunAnimAsync().Forget();
        }

        private async UniTask RunAnimAsync()
        {
            await UniTask.WaitForSeconds(_deltaStartAnim);
            
            _glassObject.SetActive(false);
            gameObject.SetActive(true);
            _textObject.SetActive(false);
            
            foreach (var shard in _shards)
            {
                shard.isKinematic = false;
            }
            
            foreach (var shard in _shards)
            {
                shard.AddExplosionForce(_explosionPower, _epicenterTransform.position, 2.5f, _explosionRadius);
            }
        }

        private void OnDestroy()
        {
            _worldEventsService.LevelLoseEvent -= OnLevelLoseEvent;
        }
    }
}