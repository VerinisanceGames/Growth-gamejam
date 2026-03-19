using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
        
        private int _stageIndex;
        private float _stageTimer;
        private bool _stageActive;

        private bool _bLevelLose;
        
        private WorldEventsService _worldEventsService;
        private World _world;

        public void OnInjectWorld(World world)
        {
            _world = world;
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.LevelStartEvent += OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer += OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer += OnBossTakeFertilizer;
        }

        private void OnBossTakeFertilizer(MonsterActor bossActor)
        {
            _stageActive = false;
            _stageTimerText.text = "30:0";
        }

        private void OnPlayerTakeFertilizer()
        {
            OnRunStage().Forget();
        }

        private void OnLevelStartEvent()
        {
            OnSpawnFertilizer();
        }

        private void OnSpawnFertilizer()
        {
            if (_stageIndex < _fertilizerPrefabs.Length)
            {
                var fertilizer = Instantiate(_fertilizerPrefabs[_stageIndex], _spawnTransform.position,
                    _spawnTransform.rotation);
                
                Vector3 destination = fertilizer.SelfTransform.position;
                destination.y = 0.2f;
                fertilizer.SelfTransform.DOMove(destination, 1.0f);
                
                var dp = fertilizer.GetComponentsInChildren<MonoBehaviour>();
                foreach (var d in dp)
                    if(d is IInjectWorld injectWorld)
                        injectWorld.OnInjectWorld(_world);
            }
            else
            {
                //End
                return;
            }
            
            _stageIndex++;
            _stageText.text = _stageIndex + "/" + _fertilizerPrefabs.Length;
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
                    _worldEventsService.OnLevelLose(EGameLooseType.Timer, EFertilizerType.Boss_1);
                    _bLevelLose = true;
                }
            }

            if (_bLevelLose == false)
            {
                _stageTimerText.text = "30:0";
                OnSpawnFertilizer();
            }
        }

        private void OnDestroy()
        {
            _worldEventsService.LevelStartEvent -= OnLevelStartEvent;
            _worldEventsService.PlayerTakeFertilizer -= OnPlayerTakeFertilizer;
            _worldEventsService.BossTakeFertilizer -= OnBossTakeFertilizer;
        }
    }
}