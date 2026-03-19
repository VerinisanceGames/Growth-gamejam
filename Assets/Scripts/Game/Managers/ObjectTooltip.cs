using System;
using Core;
using Core.GameServices;
using DG.Tweening;
using Game.Actors;
using Game.Services;
using UnityEngine;

namespace Game.Managers
{
    public class ObjectTooltip : MonoBehaviour, IService, IInjectWorld
    {
        [SerializeField] private RectTransform _rectText;

        [SerializeField] private float _widgetOffsetY;
        [SerializeField] private Ease _animationEase = Ease.Linear;
        [SerializeField] private float _animationDuration = 0.5f;

        private Camera _camera;
        private WorldEventsService _worldEventsService;


        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            
            _worldEventsService.SpawnPlayerEvent += OnSpawnPlayerEvent;
        }

        private void OnSpawnPlayerEvent(Character character)
        {
            _camera = character.PlayerCamera;
        }

        public void ShowText(Vector3 targetPosition)
        {
            Vector3 textPosition = _camera.WorldToScreenPoint(targetPosition);
            _rectText.position = textPosition + new Vector3(0.0f, _widgetOffsetY, 0.0f);
            
            _rectText.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            _rectText.gameObject.SetActive(true);
            
            _rectText
                .DOScale(Vector3.one, _animationDuration)
                .SetEase(_animationEase)
                .SetLink(_rectText.gameObject);
        }

        public void HideText()
        {
            _rectText.gameObject.SetActive(false);
        }
        

        public Type GetRegisterType()
        {
            return GetType();
        }
        
        private void OnDestroy()
        {
            _worldEventsService.SpawnPlayerEvent -= OnSpawnPlayerEvent;
        }
    }
}