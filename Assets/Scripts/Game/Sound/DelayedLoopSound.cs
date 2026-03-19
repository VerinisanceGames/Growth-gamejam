using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using Game.Services;
using UnityEngine;

public class DelayedAudioLoop : MonoBehaviour, IInjectWorld
{
    [SerializeField] private AudioSource audioSource;
    
    public float delayBetweenPlays = 7.0f;
    
    private WorldEventsService _worldEventsService;

    public void OnInjectWorld(World world)
    {
        _worldEventsService = world.GetService<WorldEventsService>();
        _worldEventsService.LevelStartEvent += OnLevelStartEvent;
    }

    private void OnLevelStartEvent()
    {
        if(audioSource != null)
            OnRunAudioAsync().Forget();
    }

    private async UniTask OnRunAudioAsync()
    {
        audioSource.Play();
        await UniTask.WaitForSeconds(delayBetweenPlays);
        OnRunAudioAsync();
    }

    private void OnDestroy()
    {
        _worldEventsService.LevelStartEvent -= OnLevelStartEvent;
    }
}
