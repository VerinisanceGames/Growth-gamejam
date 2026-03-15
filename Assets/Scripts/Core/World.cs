using Core.GameServices;
using Game.Services;
using UnityEngine;

namespace Core
{
    public class World : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour[] _services;
        
        private GameInstance _gameInstance;
        private ServiceLocator _serviceLocator;

        public void Initialize(GameInstance gameInstance)
        {
            _gameInstance = gameInstance;

            _serviceLocator = new ServiceLocator();

            var worldEventsService = new WorldEventsService();
            _serviceLocator.Add(worldEventsService);

            foreach (var target in _services)
            {
                if(target is IService service)
                    _serviceLocator.Add(service);
            }

            var sceneObjects = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sceneObject in sceneObjects)
            {
                if(sceneObject is IInjectWorld dependency)
                    dependency.OnInjectWorld(this);
            }
            
            worldEventsService.OnLevelLoaded();;
        }
        
        public GameInstance GetGameInstance() => _gameInstance;

        public TService GetService<TService>() where TService : IService
        {
            return _serviceLocator.Get<TService>();
        }
    }
}