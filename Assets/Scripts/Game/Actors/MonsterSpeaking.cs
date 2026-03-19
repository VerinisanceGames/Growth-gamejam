using System;
using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using Game.Actors;
using Game.Services;
using Game.Enums;
using TMPro;
using UnityEngine;

public class MonsterSpeaking : MonoBehaviour, IInjectWorld
{
    [SerializeField] public EFertilizerType bossType;
    
    [Header("Audio Sources")] 
    [SerializeField] private AudioSource plantAudioSource;

    [Header("Sounds")] 
    [SerializeField] private  AudioClip helloSound;
    [SerializeField] private  AudioClip noSound;
    [SerializeField] private  AudioClip giveMeSound;

    private bool _withFertilizer = false;
    private bool _firstEntry = true;
    private bool _gameLost = false;
    private int _fertilizerIndex;

    private WorldEventsService _worldEventsService;

    public void OnInjectWorld(World world)
    {
        _worldEventsService = world.GetService<WorldEventsService>();
        _worldEventsService.LevelStartEvent += OnLevelStartEvent;
        _worldEventsService.PlayerTakeFertilizer += OnPlayerTakeFertilizer;
        _worldEventsService.BossTakeFertilizer += OnBossTakeFertilizer;
        _worldEventsService.LevelLoseEvent += OnLevelLoseEvent;
    }

    private void OnBossTakeFertilizer(MonsterActor bossActor)
    {
        _withFertilizer = false;
    }

    private void OnPlayerTakeFertilizer()
    {
        _fertilizerIndex++;
        _withFertilizer = true;
    }

    private void OnLevelStartEvent()
    {
        _fertilizerIndex = 0;
    }

    private async void OnLevelLoseEvent(EGameLooseType type, EFertilizerType bossType)
    {
        _gameLost = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_gameLost) {
            return;
        }

        if (other.TryGetComponent<Character>(out var character))
        {
            if (plantAudioSource == null) {
                return;
            }

            if (_firstEntry && !_withFertilizer) {
                _firstEntry = false;
                if (helloSound != null) {
                    plantAudioSource.PlayOneShot(helloSound);
                }
            }

            if (_withFertilizer) {
                // Jared always asks to give him fertilizer
                if (bossType == EFertilizerType.Boss_4) {
                    if (giveMeSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(giveMeSound);
                    }
                }
                //
                //
                // Fert 3. Flytrap - give
                if (bossType == EFertilizerType.Boss_1 && _fertilizerIndex == 3) {
                    if (giveMeSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(giveMeSound);
                    }
                }
                // Fert 4. Flower - give. Flytrap - reject
                if (bossType == EFertilizerType.Boss_3 && _fertilizerIndex == 4) {
                    if (giveMeSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(giveMeSound);
                    }
                }
                // Fert 4. Flytrap - reject
                if (bossType == EFertilizerType.Boss_1 && _fertilizerIndex == 4) {
                    if (noSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(noSound);
                    }
                }
                // Fert 6. Flower - reject
                if (bossType == EFertilizerType.Boss_3 && _fertilizerIndex == 6) {
                    if (noSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(noSound);
                    }
                }
                // Fert 7. Flytrap - give
                if (bossType == EFertilizerType.Boss_1 && _fertilizerIndex == 7) {
                    if (giveMeSound != null) {
                        if (plantAudioSource.isPlaying) {
                            plantAudioSource.Stop();
                        }
                        plantAudioSource.PlayOneShot(giveMeSound);
                    }
                }
            }
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
