using DG.Tweening;
using Game.Actors;
using UnityEngine;

namespace Game.Components
{
    public class GrabComponent : MonoBehaviour
    {
        [SerializeField] private Transform _socket;

        [SerializeField] private float _animDuration;
        [SerializeField] private Ease _animEase;

        public bool ExistGrab => _socket.childCount > 0;
        
        public void OnGrabItem(Transform targetItem)
        {
            if (_socket.childCount > 0)
            {
                _socket.GetChild(0).SetParent(null);
            }
            
            targetItem.SetParent(_socket);
            targetItem
                .DOLocalMove(Vector3.zero, _animDuration)
                .SetEase(_animEase)
                .SetLink(targetItem.gameObject);
            
            targetItem
                .DOLocalRotate(Vector3.zero, _animDuration)
                .SetEase(_animEase)
                .SetLink(targetItem.gameObject);
        }


        public FertilizerActor GetFertilizerActor()
        {
            if (ExistGrab)
                return _socket.GetChild(0).GetComponent<FertilizerActor>();

            return null;
        }
    }
}