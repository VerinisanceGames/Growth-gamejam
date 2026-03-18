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
        [SerializeField] private float _duration;
        
        private WorldEventsService _eventsWorldService;

        public void OnInjectWorld(World world)
        {
            _eventsWorldService = world.GetService<WorldEventsService>();
            _eventsWorldService.LevelLoseEvent += OnLevelLoseEvent;
        }

        private void OnLevelLoseEvent(EGameLooseType type, EFertilizerType bossType)
        {
            DelayShowing().Forget();
        }

        private async UniTask DelayShowing()
        {
            await UniTask.WaitForSeconds(2.5f);
            
            _loseCanvas.enabled = true;
            //_canvasGroup.DOFade(1, _duration);
            _canvasGroup.alpha = 0f; 
            _canvasGroup.DOFade(1, 2.0f);
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        private void OnDestroy()
        {
            _eventsWorldService.LevelLoseEvent -= OnLevelLoseEvent;
        }
    }
}