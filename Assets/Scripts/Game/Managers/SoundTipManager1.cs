using System;
using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using Game.Actors;
using Game.Services;
using Game.Enums;
using TMPro;
using UnityEngine;

namespace Game.Managers
{
    public class SoundTipManager : MonoBehaviour, IInjectWorld
    {
        [Header("Audio Sources")] 
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioSource commonAudioSource;
        [SerializeField] private AudioSource plantAudioSource;

        [Header("Tip sounds")] 
        [SerializeField] private  AudioClip startGameTip1;
        [SerializeField] private  AudioClip startGameTip2;
        [SerializeField] private  AudioClip fertilizerTip1;
        [SerializeField] private  AudioClip fertilizerTip6;
        [SerializeField] private  AudioClip fertilizerTip10;

        [Header("Game lost sounds")] 
        [SerializeField] private  AudioClip gameLostTimer;
        [SerializeField] private  AudioClip gameLostFedWrong;
        [SerializeField] private  AudioClip gameLostFedJared;

        [Header("Common sounds")] 
        [SerializeField] private  AudioClip takeFertilizer;
        [SerializeField] private  AudioClip deliverFertilizer;

        [Header("Plant sounds")] 
        [SerializeField] private  AudioClip firstTimeFlytrap;
        [SerializeField] private  AudioClip firstTimeMushroom;
        [SerializeField] private  AudioClip firstTimeFlower;
        [SerializeField] private  AudioClip firstTimeJared;
        
        [SerializeField] private  AudioClip readyToEatFlytrap;
        [SerializeField] private  AudioClip readyToEatMushroom;
        [SerializeField] private  AudioClip readyToEatFlower;
        [SerializeField] private  AudioClip readyToEatJared;

        [SerializeField] private  AudioClip rejectFoodFlytrap;
        [SerializeField] private  AudioClip rejectFoodMushroom;
        [SerializeField] private  AudioClip rejectFoodFlower;

        [SerializeField] private  AudioClip eatFoodFlytrap;
        [SerializeField] private  AudioClip eatFoodMushroom;
        [SerializeField] private  AudioClip eatFoodFlower;
        [SerializeField] private  AudioClip eatFoodJared;

        [SerializeField] private  AudioClip angryFlytrap;
        [SerializeField] private  AudioClip angryMushroom;
        [SerializeField] private  AudioClip angryFlower;
        [SerializeField] private  AudioClip angryJared;
        
        private int _fertilizerIndex;

        private float delayBeforeStart1 = 3.0f;
        private float delayBeforeStart2 = 15.0f;
        
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            Debug.Log("SoundTipManager OnInjectWorld");
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.LevelStartEvent += OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer += OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer += OnBossTakeFertilizer;
            _worldEventsService.LevelLoseEvent += OnLevelLoseEvent;
        }

        private void OnLevelLoseEvent(EGameLooseType type)
        {
            if (type == EGameLooseType.Timer) {
                audioSource.PlayOneShot(gameLostTimer);
            } else  if (type == EGameLooseType.Fed_wrong) {
                audioSource.PlayOneShot(gameLostFedWrong);
            } else  if (type == EGameLooseType.Fed_jared) {
                audioSource.PlayOneShot(gameLostFedJared);
            }
        }

        private void OnBossTakeFertilizer(MonsterActor bossActor)
        {
            commonAudioSource.PlayOneShot(deliverFertilizer);

            if (bossActor._conditionType == EFertilizerType.Boss_1) {
                Debug.Log("delivered to FRED");
            } else  if (bossActor._conditionType == EFertilizerType.Boss_2) {
                Debug.Log("delivered to LILI");
            } else  if (bossActor._conditionType == EFertilizerType.Boss_3) {
                Debug.Log("delivered to FLOWER");
            } else  if (bossActor._conditionType == EFertilizerType.Boss_4) {
                Debug.Log("delivered to JARED");
            }
        }

        private void OnPlayerTakeFertilizer()
        {
            _fertilizerIndex++;

            commonAudioSource.PlayOneShot(takeFertilizer);

            if (audioSource.isPlaying) {
                audioSource.Stop();
            }

            if (_fertilizerIndex == 1) {
                audioSource.PlayOneShot(fertilizerTip1);
            } else if (_fertilizerIndex == 6) {
                audioSource.PlayOneShot(fertilizerTip6);
            } else if (_fertilizerIndex == 10) {
                audioSource.PlayOneShot(fertilizerTip10);
            }
        }

        private async void OnLevelStartEvent()
        {
            _fertilizerIndex = 0;

            await UniTask.WaitForSeconds(delayBeforeStart1);
            audioSource.PlayOneShot(startGameTip1);

            await UniTask.WaitForSeconds(delayBeforeStart2);
            if (_fertilizerIndex == 0) {
                audioSource.PlayOneShot(startGameTip2);
            }
        }

        private void OnDestroy()
        {
            _worldEventsService.LevelStartEvent -= OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer -= OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer -= OnBossTakeFertilizer;
            _worldEventsService.LevelLoseEvent -= OnLevelLoseEvent;
        }
    }
}