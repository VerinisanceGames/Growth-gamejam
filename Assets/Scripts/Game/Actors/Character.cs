using Core;
using Core.GameServices;
using Game.Components;
using Game.Interfaces;
using Game.Services;
using UnityEngine;

namespace Game.Actors
{
    public sealed class Character : Actor, IInjectWorld
    {
        [field: Header("Camera")]
        [field: SerializeField] public Camera PlayerCamera { get; private set; }
        
        [SerializeField] private GrabComponent _grabComponent;
        [SerializeField] private Rigidbody _socketRigidbody;
        
        private SelectableService _selectableService;
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _selectableService = world.GetService<SelectableService>();
            _worldEventsService = world.GetService<WorldEventsService>();
            
            _selectableService.SelectableClickEvent += OnInteractableClick;
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
            _selectableService.SelectableClickEvent -= OnInteractableClick;
        }
    }
}