using DG.Tweening;
using UnityEngine;

namespace Game.Components
{
    public class AnimComponents : MonoBehaviour
    {
        [SerializeField] private Transform _bodyTransform;

        [SerializeField] private Vector3 _endScale;
        [SerializeField] private float _loopTime;
        
        private void Start()
        {
            _bodyTransform
                .DOScale(_endScale, _loopTime)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }
    }
}