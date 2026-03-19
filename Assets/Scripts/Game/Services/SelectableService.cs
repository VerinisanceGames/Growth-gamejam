using System;
using Core;
using Core.GameServices;
using Game.Actors;
using Game.Interfaces;
using UnityEngine;

namespace Game.Services
{
    public class SelectableService : MonoBehaviour, IService, IInjectWorld
    {
        public event Action<IInteractable> SelectableClickEvent;
        
        [SerializeField] private Camera _rayCamera;
        [SerializeField] private LayerMask _interactableLayers;
        [SerializeField] private float _maxDistance;
        
        private IInteractable _currentInteractable;

        private Vector2 _screenMiddlePosition;
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.SpawnPlayerEvent += OnSpawnPlayerEvent;
        }

        private void Awake()
        {
            _screenMiddlePosition = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        private void Update()
        {
            if(_rayCamera == null) return;
            
            Ray ray = _rayCamera.ScreenPointToRay(_screenMiddlePosition);
            if (Physics.Raycast(ray, out var hitResult, _maxDistance, _interactableLayers))
            {
                if (hitResult.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    if (interactable != _currentInteractable)
                    {
                        _currentInteractable?.OnCursorExit();
                        _currentInteractable = interactable;
                        interactable.OnCursorEnter(hitResult.point);
                    }
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        _currentInteractable.OnCursorClick();
                        SelectableClickEvent?.Invoke(_currentInteractable);
                    }
                }
                else
                {
                    ClearInteractable();
                }
            }
            else
            {
                ClearInteractable();
            }
        }
        
        private void OnSpawnPlayerEvent(Character playerCharacter)
        { 
            _rayCamera = playerCharacter.PlayerCamera;
        }

        public bool TryGetInteractable(out IInteractable interactable)
        {

            if (_currentInteractable == null)
            {
                interactable = null;
                return false;
            }

            interactable = _currentInteractable;
            return true;
        }

        private void ClearInteractable()
        {
            _currentInteractable?.OnCursorExit();
            _currentInteractable = null;
        }

        public Type GetRegisterType() => GetType();

        private void OnDestroy()
        {
            _worldEventsService.SpawnPlayerEvent -= OnSpawnPlayerEvent;
        }
    }
}