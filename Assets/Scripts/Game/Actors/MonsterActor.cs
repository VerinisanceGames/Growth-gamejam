using Core;
using Core.GameServices;
using DG.Tweening;
using Game.Enums;
using Game.Services;
using UnityEngine;

namespace Game.Actors
{
    public class MonsterActor : Actor, IInjectWorld
    {
        [SerializeField] private Transform _containerTransform;
        [SerializeField] private EFertilizerType _conditionType;
        [SerializeField] private float _moveTime;

        [SerializeField] private float _moveJumpPower = 3.0f;
        
        [Header("Light intensity")]
        [SerializeField] private Light _light;
        [SerializeField] private float _defaultIntensity;
        [SerializeField] private float _enterIntensity;
        
        private EFertilizerType _lastFertilizerType;
        private FertilizerActor _fertilizer;
        private bool _bIsTake;
        
        private WorldEventsService _worldEventsService;

        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
        }

        private void OnValidateCondition()
        {
            if (_lastFertilizerType == _conditionType)
            {
                
            }
            else
            {
                _worldEventsService.OnLevelLose();
            }
        }

        private void Update()
        {
            if (_fertilizer != null && _bIsTake == false)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
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
                        .DOScale(new Vector3(0.5f, 0.5f, 0.5f), _moveTime)
                        .SetLink(_fertilizer.gameObject);
                }
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character))
            {
                if (character.TryGetFertilizer(out FertilizerActor fertilizer))
                {
                    _light.intensity = _enterIntensity;
                    _fertilizer = fertilizer;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Character>(out var character) && _bIsTake == false)
            {
                _light.intensity = _defaultIntensity;
                _fertilizer = null;
            }
        }
    }
}