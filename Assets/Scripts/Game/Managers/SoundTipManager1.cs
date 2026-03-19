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

        [Header("Game finish sounds")] 
        [SerializeField] private  AudioClip gameLostTimer;
        [SerializeField] private  AudioClip gameLostFedWrong;
        [SerializeField] private  AudioClip gameLostFedJared;
        [SerializeField] private  AudioClip gameSuccessFedJared;

        [Header("Common sounds")] 
        [SerializeField] private  AudioClip takeFertilizer;
        [SerializeField] private  AudioClip deliverFertilizer;
        [SerializeField] private  AudioClip glassBreak;
        [SerializeField] private  AudioClip gameLost;
        [SerializeField] private  AudioClip gameSuccess;

        [Header("Plant sounds")] 
        // [SerializeField] private  AudioClip eatFoodFlytrap;
        // [SerializeField] private  AudioClip eatFoodMushroom;
        // [SerializeField] private  AudioClip eatFoodFlower;
        // [SerializeField] private  AudioClip eatFoodJared;
        [SerializeField] private  AudioClip angryFlytrap;
        [SerializeField] private  AudioClip angryMushroom;
        [SerializeField] private  AudioClip angryFlower;
        [SerializeField] private  AudioClip angryJared;
        [Header("-")] 
        [SerializeField] private  AudioClip eatPlayerFlytrap;
        [SerializeField] private  AudioClip eatPlayerMushroom;
        [SerializeField] private  AudioClip eatPlayerFlower;
        [SerializeField] private  AudioClip eatPlayerJared;
        
        private int _fertilizerIndex;

        private float delayBeforeStart1 = 3.0f;
        private float delayBeforeStart2 = 15.0f;
        
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.LevelStartEvent += OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer += OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer += OnBossTakeFertilizer;
            _worldEventsService.LevelLoseEvent += OnLevelLoseEvent;
        }

        private async void OnLevelLoseEvent(EGameLooseType type, EFertilizerType bossType)
        {
            if (type == EGameLooseType.Timer) {
                await UniTask.WaitForSeconds(2.0f);
                audioSource.PlayOneShot(gameLost);
                await UniTask.WaitForSeconds(1.5f);
                audioSource.PlayOneShot(gameLostTimer);
            } else  if (type == EGameLooseType.Fed_wrong) {
                commonAudioSource.PlayOneShot(glassBreak);
                //
                // FLYTRAP
                if (bossType == EFertilizerType.Boss_1) {
                    plantAudioSource.PlayOneShot(angryFlytrap);
                    //
                    await UniTask.WaitForSeconds(2.5f);
                    audioSource.PlayOneShot(eatPlayerFlytrap);
                } else 
                // MUSHROOM
                if (bossType == EFertilizerType.Boss_2) {
                    //plantAudioSource.PlayOneShot(angryFlytrap);
                    //
                    await UniTask.WaitForSeconds(1.5f);
                    audioSource.PlayOneShot(eatPlayerMushroom);
                } else 
                // FLOWER
                if (bossType == EFertilizerType.Boss_3) {
                    plantAudioSource.PlayOneShot(angryFlower);
                    //
                    await UniTask.WaitForSeconds(1.7f);
                    audioSource.PlayOneShot(eatPlayerFlower);
                }
                //
                await UniTask.WaitForSeconds(1.0f);
                audioSource.PlayOneShot(gameLost);
                //
                await UniTask.WaitForSeconds(1.5f);
                audioSource.PlayOneShot(gameLostFedWrong);
            } else  if (type == EGameLooseType.Fed_jared) {
                commonAudioSource.PlayOneShot(glassBreak);
                //
                plantAudioSource.PlayOneShot(angryJared);
                //
                await UniTask.WaitForSeconds(1.5f);
                audioSource.PlayOneShot(eatPlayerJared);
                //
                await UniTask.WaitForSeconds(0.5f);
                audioSource.PlayOneShot(gameLost);
                //
                await UniTask.WaitForSeconds(1.5f);
                audioSource.PlayOneShot(gameLostFedJared);
            } else if (type == EGameLooseType.Success) {
                commonAudioSource.PlayOneShot(glassBreak);
                //
                plantAudioSource.PlayOneShot(angryJared);
                //
                await UniTask.WaitForSeconds(2.0f);
                audioSource.PlayOneShot(gameSuccess);
                //
                await UniTask.WaitForSeconds(1.5f);
                audioSource.PlayOneShot(gameSuccessFedJared);
            }
        }

        private void OnBossTakeFertilizer(MonsterActor bossActor)
        {
            commonAudioSource.PlayOneShot(deliverFertilizer);
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