using System;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Services;
using Game.Enums;
using UnityEngine;

namespace Core
{
    public class LoseScreen : MonoBehaviour, IInjectWorld
    {
        [SerializeField] private Canvas _loseCanvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _successCanvas;
        [SerializeField] private CanvasGroup _successCanvasGroup;
        [SerializeField] private float _duration;
        
        private WorldEventsService _eventsWorldService;

        public void OnInjectWorld(World world)
        {
            _eventsWorldService = world.GetService<WorldEventsService>();
            _eventsWorldService.LevelLoseEvent += OnLevelLoseEvent;
        }

        private void OnLevelLoseEvent(EGameLooseType type, EFertilizerType bossType)
        {
            DelayShowing(type).Forget();
        }

        private async UniTask DelayShowing(EGameLooseType type)
        {
            await UniTask.WaitForSeconds(2.5f);
            
            if (type == EGameLooseType.Success) {
                 _successCanvas.enabled = true;
                _successCanvasGroup.alpha = 0f; 
                _successCanvasGroup.DOFade(1, 1.3f);
            } else {
                _loseCanvas.enabled = true;
                _canvasGroup.alpha = 0f; 
                _canvasGroup.DOFade(1, 1.3f);
            }
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        private void OnDestroy()
        {
            _eventsWorldService.LevelLoseEvent -= OnLevelLoseEvent;
        }
    }
}