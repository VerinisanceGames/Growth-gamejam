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
    public class FertilizerManager : MonoBehaviour, IInjectWorld
    {
        [SerializeField] private FertilizerActor[] _fertilizerPrefabs;
        [SerializeField] private Transform _spawnTransform;

        [Header("Stage settings")]
        [SerializeField] private float _stageTime = 60.0f;

        [Header("UI References")] 
        [SerializeField] private TMP_Text _stageText;
        [SerializeField] private TMP_Text _stageTimerText;

        [Header("Sound")] 
        [SerializeField] private AudioSource commonAudioSource;
        [SerializeField] private  AudioClip fertilizerDropSound;

        private FertilizerActor currentFertilizer;
        private bool _fertilizerMoving = false;
        
        private int _stageIndex;
        private float _stageTimer;
        private bool _stageActive;

        private bool _bLevelLose;
        
        private WorldEventsService _worldEventsService;
        private Character _playerCharacter;

        public void OnInjectWorld(World world)
        {
            Debug.Log("FertilizerManager OnInjectWorld");
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.SpawnPlayerEvent += OnSpawnPlayerEvent;
            _worldEventsService.LevelStartEvent += OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer += OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer += OnBossTakeFertilizer;
        }

        private void OnSpawnPlayerEvent(Character playerCharacter) => _playerCharacter = playerCharacter;

        private void OnBossTakeFertilizer(MonsterActor bossActor)
        {
            _stageActive = false;
            _stageTimerText.text = "30.0";
        }

        private void OnPlayerTakeFertilizer()
        {
            _fertilizerMoving = false;
            OnRunStage().Forget();
        }

        private async void OnLevelStartEvent()
        {
            await UniTask.WaitForSeconds(10.0f);
            OnSpawnFertilizer();
        }

        private async void OnSpawnFertilizer()
        {
            if (_stageIndex < _fertilizerPrefabs.Length)
            {
                var fertilizer = Instantiate(_fertilizerPrefabs[_stageIndex], _spawnTransform.position,
                    _spawnTransform.rotation);
                currentFertilizer = fertilizer;
                _fertilizerMoving = true;
                if (commonAudioSource != null && fertilizerDropSound != null) {
                    await UniTask.WaitForSeconds(0.08f);
                    commonAudioSource.PlayOneShot(fertilizerDropSound);
                }
            }
            else
            {
                //End
                return;
            }
            
            _stageIndex++;
            _stageText.text = _stageIndex + "/" + _fertilizerPrefabs.Length;
        }

        void Update()
        {
            if (currentFertilizer == null) {
                return;
            }

            if (_fertilizerMoving) {
                Debug.Log(currentFertilizer.transform.position.y);
                if (currentFertilizer.transform.position.y < 0.1) {
                    _fertilizerMoving = false;
                } else {
                    currentFertilizer.transform.Translate(-1* Vector3.up * 4.0f * Time.deltaTime);
                }
            }
        }
        
        private async UniTask OnRunStage()
        {
            _stageActive = true;
            _stageTimer = _stageTime;
            while (_stageActive)
            {
                await UniTask.WaitForEndOfFrame();
                _stageTimer -= Time.deltaTime;

                _stageTimerText.text = _stageTimer.ToString("0.0");
                    
                if (_stageTimer <= 0.0f)
                {
                    _stageActive = false;
                    _playerCharacter.FirstPersonMovement.OnDisableController();
                    _worldEventsService.OnLevelLose(EGameLooseType.Timer, EFertilizerType.Boss_1);
                    _bLevelLose = true;
                }
            }

            if (_bLevelLose == false)
            {
                _stageTimerText.text = "30.0";
                await UniTask.WaitForSeconds(3.0f);
                OnSpawnFertilizer();
            }
        }

        private void OnDestroy()
        {
            _worldEventsService.SpawnPlayerEvent -= OnSpawnPlayerEvent;
            _worldEventsService.LevelStartEvent -= OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer -= OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer -= OnBossTakeFertilizer;
        }
    }
}