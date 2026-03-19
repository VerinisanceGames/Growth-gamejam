using Core;
using Core.GameServices;
using DG.Tweening;
using Game.Enums;
using Game.Managers;
using Game.Services;
using UnityEngine;

namespace Game.Actors
{
    public class MonsterActor : Actor, IInjectWorld
    {
        [SerializeField] private CutsceneLook _cutsceneLook;
        [SerializeField] private Transform _lookAtPoint;
        [SerializeField] private float _transitionDuration;
        
        [SerializeField] private Transform _containerTransform;
        [SerializeField] public EFertilizerType _conditionType;
        [SerializeField] private float _moveTime;

        [SerializeField] private float _moveJumpPower = 3.0f;
        
        [Header("Light intensity")]
        [SerializeField] private Light _light;
        [SerializeField] private float _defaultIntensity;
        [SerializeField] private float _enterIntensity;
        
        private EFertilizerType _lastFertilizerType;
        private FertilizerActor _fertilizer;
        private bool _bIsTake;

        private Character _playerCharacter;
        
        private WorldEventsService _worldEventsService;
        private ObjectTooltip _objectTooltipService;

        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            _objectTooltipService = world.GetService<ObjectTooltip>();
            
            _worldEventsService.SpawnPlayerEvent += OnSpawnPlayerEvent;
            
        }

        private void OnValidateCondition()
        {
            if (_lastFertilizerType == _conditionType)
            {
                
            }
            else
            {
                _playerCharacter.FirstPersonMovement.OnDisableController();
                _cutsceneLook.LookAtPoint(_lookAtPoint.position, _transitionDuration, () =>
                {
                    if (_conditionType == EFertilizerType.Boss_4) {
                        _worldEventsService.OnLevelLose(EGameLooseType.Fed_jared, _conditionType);
                    } else {
                        _worldEventsService.OnLevelLose(EGameLooseType.Fed_wrong, _conditionType);
                    }
                });
            }
        }

        private void Update()
        {
            if (_fertilizer != null && _bIsTake == false)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    _objectTooltipService.HideText();
                    _bIsTake = true;
                    _fertilizer.OnDestroyFertilizer();
                    _fertilizer.SelfTransform.SetParent(null);
                    _lastFertilizerType = _fertilizer.FertilizerType;
                        
                    _worldEventsService.OnBossTakeFertilizer(this);
                        
                    _fertilizer.SelfTransform
                        .DOJump(_containerTransform.position, _moveJumpPower, 1, _moveTime)
                        .SetLink(_fertilizer.gameObject)
                        .OnComplete(() =>
                        {
                            _bIsTake = false;
                            Destroy(_fertilizer.gameObject);
                            _fertilizer = null;
                            OnValidateCondition();
                        });

                    _fertilizer.SelfTransform
                        .DOScale(new Vector3(0.8f, 0.8f, 0.8f), _moveTime)
                        .SetLink(_fertilizer.gameObject);
                }
            }
        }
        
        private void OnSpawnPlayerEvent(Character playerCharacter) => _playerCharacter = playerCharacter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                if (character.TryGetFertilizer(out FertilizerActor fertilizer))
                {
                    _light.intensity = _enterIntensity;
                    _fertilizer = fertilizer;
                    
                    _objectTooltipService.ShowText(_containerTransform.position);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character) && _bIsTake == false)
            {
                _light.intensity = _defaultIntensity;
                _fertilizer = null;
                _objectTooltipService.HideText();
            }
        }

        private void OnDestroy()
        {
            _worldEventsService.SpawnPlayerEvent -= OnSpawnPlayerEvent;
        }
    }
}