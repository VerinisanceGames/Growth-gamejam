using Core.GameServices;
using Game.Actors;
using Game.Services;
using UnityEngine;

namespace Core
{
    public class World : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private Character _playerCharacter;
        [SerializeField] private Transform _spawnTransform;

        [Header("Camera")] 
        [SerializeField] private GameObject _menuCamera;
        [SerializeField] private CanvasGroup _menuCanvasGroup;
        
        [Header("Services")]
        [SerializeField] private MonoBehaviour[] _services;
        
        private GameInstance _gameInstance;
        private ServiceLocator _serviceLocator;
        private WorldEventsService _worldEventsService;

        public void Initialize(GameInstance gameInstance)
        {
            _gameInstance = gameInstance;

            _serviceLocator = new ServiceLocator();

            _worldEventsService = new WorldEventsService();
            _serviceLocator.Add(_worldEventsService);

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
            
            _worldEventsService.OnLevelLoaded();
        }
        
        public GameInstance GetGameInstance() => _gameInstance;

        public TService GetService<TService>() where TService : IService
        {
            return _serviceLocator.Get<TService>();
        }

        public void RestartGame()
        {
            _gameInstance.LoadScene(1);
        }


        public void StartGame()
        {
            
            Character character = Instantiate(_playerCharacter, _spawnTransform.position, _spawnTransform.rotation);
            
            var components = character.GetComponentsInChildren<MonoBehaviour>();
            foreach (var component in components)
            {
                if(component is IInjectWorld dependency)
                    dependency.OnInjectWorld(this);
            }
            
            _menuCamera.SetActive(false);
            _menuCanvasGroup.alpha = 0.0f;
            
            _worldEventsService.OnSpawnPlayer(character);
            _worldEventsService.OnLevelStart();
        }
        
    }

    
}