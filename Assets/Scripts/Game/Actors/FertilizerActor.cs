using Core;
using Core.GameServices;
using Game.Enums;
using Game.Interfaces;
using Game.Managers;
using UnityEngine;

namespace Game.Actors
{
    public class FertilizerActor : Actor, IInteractable, IInjectWorld
    {
        public bool IsActiveFertilizer => _isActive;
        
        [Header("Description")]
        [SerializeField] private string TitleName;
        
        [field: SerializeField] public EFertilizerType FertilizerType { get; private set; }
        
        [Header("References")] 
        [SerializeField] private ConfigurableJoint _joint;

        private bool _isActive = true;
        
        private ObjectTooltip _objectTooltipService;

        public void OnInjectWorld(World world)
        {
            _objectTooltipService = world.GetService<ObjectTooltip>();
        }
        
        
        public void OnGrabEnable(Rigidbody socketRigidbody) =>
                                _joint.connectedBody = socketRigidbody;
        
        public string GetTitleName() => TitleName;

        public Transform GetTransform() => SelfTransform;

        public void OnCursorEnter(Vector3 hitPosition)
        {
            if(_isActive)
                _objectTooltipService.ShowText(hitPosition);
        }

        public void OnCursorClick()
        {
            
        }

        public void OnCursorExit()
        {
            _objectTooltipService.HideText();
        }

        public void OnDestroyFertilizer()
        {
            _isActive = false;
        }
        
    }
}