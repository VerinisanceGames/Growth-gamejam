using Core;
using Core.GameServices;
using Game.Components;
using Game.Interfaces;
using Game.Services;
using Game.Enums;
using UnityEngine;

namespace Game.Actors
{
    public sealed class Character : Actor, IInjectWorld
    {
        [field: Header("Camera")]
        [field: SerializeField] public Camera PlayerCamera { get; private set; }
        [field: SerializeField] public Transform CameraTransform { get; private set; }
        
        [field: SerializeField] public FirstPersonMovement FirstPersonMovement { get; private set; }
        
        [SerializeField] private GrabComponent _grabComponent;
        [SerializeField] private Rigidbody _socketRigidbody;
        
        private SelectableService _selectableService;
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _selectableService = world.GetService<SelectableService>();
            _worldEventsService = world.GetService<WorldEventsService>();
            
            _worldEventsService.LevelLoseEvent += OnLevelLoseEvent;
            _selectableService.SelectableClickEvent += OnInteractableClick;
        }

        private void OnLevelLoseEvent(EGameLooseType type, EFertilizerType bossType)
        {
            
        }

        private void OnInteractableClick(IInteractable interactable)
        {
            if (_grabComponent.ExistGrab) return;


            if (interactable.GetTransform().TryGetComponent<FertilizerActor>(out var fertilizerActor))
            {
                if (fertilizerActor.IsActiveFertilizer)
                {
                    var interactableTransform = interactable.GetTransform();
                
                    _grabComponent.OnGrabItem(interactableTransform);
                    fertilizerActor.OnGrabEnable(_socketRigidbody);
                    _worldEventsService.OnPlayerTakeFertilizer();
                }
            }
            
        }

        public bool TryGetFertilizer(out FertilizerActor fertilizerActor)
        {
            if (_grabComponent.ExistGrab)
            {
                fertilizerActor = _grabComponent.GetFertilizerActor();
                return true;
            }

            fertilizerActor = null;
            return false;
        }

        private void OnDestroy()
        {
            _worldEventsService.LevelLoseEvent -= OnLevelLoseEvent;
            _selectableService.SelectableClickEvent -= OnInteractableClick;
        }
    }
}