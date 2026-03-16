using Game.Enums;
using Game.Interfaces;
using UnityEngine;

namespace Game.Actors
{
    public class FertilizerActor : Actor, IInteractable
    {
        public bool IsActiveFertilizer => _isActive;
        
        [Header("Description")]
        [SerializeField] private string TitleName;
        
        [field: SerializeField] public EFertilizerType FertilizerType { get; private set; }
        
        [Header("References")] 
        [SerializeField] private ConfigurableJoint _joint;

        private bool _isActive = true;
        
        public void OnGrabEnable(Rigidbody socketRigidbody) =>
                                _joint.connectedBody = socketRigidbody;
        
        public string GetTitleName() => TitleName;

        public Transform GetTransform() => SelfTransform;

        public void OnCursorEnter()
        {
            Debug.Log("Enter: " + TitleName);
        }

        public void OnCursorClick()
        {
            Debug.Log("Click: " + TitleName);
        }

        public void OnCursorExit()
        {
            Debug.Log("Exit: " + TitleName);
        }

        public void OnDestroyFertilizer()
        {
            _isActive = false;
        }
    }
}