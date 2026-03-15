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
        [SerializeField] private GrabComponent _grabComponent;
        [SerializeField] private Rigidbody _socketRigidbody;
        
        private SelectableService _selectableService;

        public void OnInjectWorld(World world)
        {
            _selectableService = world.GetService<SelectableService>();
            _selectableService.SelectableClickEvent += OnInteractableClick;
        }

        private void OnInteractableClick(IInteractable interactable)
        {
            if (_grabComponent.ExistGrab) return;
            
            var interactableTransform = interactable.GetTransform();
            
            _grabComponent.OnGrabItem(interactableTransform);
            var fertilizerActor = interactableTransform.GetComponent<FertilizerActor>();
            fertilizerActor.OnGrabEnable(_socketRigidbody);

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