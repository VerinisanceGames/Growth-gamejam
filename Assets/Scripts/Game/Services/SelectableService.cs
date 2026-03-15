using System;
using Core.GameServices;
using Game.Interfaces;
using UnityEngine;

namespace Game.Services
{
    public class SelectableService : MonoBehaviour, IService
    {
        public event Action<IInteractable> SelectableClickEvent;
        
        [SerializeField] private Camera _rayCamera;
        [SerializeField] private LayerMask _interactableLayers;
        [SerializeField] private float _maxDistance;
        
        private IInteractable _currentInteractable;

        private Vector2 _screenMiddlePosition;
        
        private void Awake()
        {
            _screenMiddlePosition = new Vector2(Screen.width / 2, Screen.height / 2);
        }

        private void Update()
        {
            Ray ray = _rayCamera.ScreenPointToRay(_screenMiddlePosition);
            if (Physics.Raycast(ray, out var hitResult, _maxDistance, _interactableLayers))
            {
                if (hitResult.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    if (interactable != _currentInteractable)
                    {
                        _currentInteractable?.OnCursorExit();
                        _currentInteractable = interactable;
                        interactable.OnCursorEnter();
                    }
                    
                    if (Input.GetMouseButtonDown(0))
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
    }
}