using System.Threading;
using Core;
using Core.GameServices;
using Cysharp.Threading.Tasks;
using Game.Services;
using UnityEngine;

public class DelayedAudioLoop : MonoBehaviour, IInjectWorld
{
    [SerializeField] private AudioSource audioSource;
    
    public float delayBetweenPlays = 5.0f;
    
    private WorldEventsService _worldEventsService;
    private CancellationTokenSource _tokenSource;

    public void OnInjectWorld(World world)
    {
        _worldEventsService = world.GetService<WorldEventsService>();
        _worldEventsService.LevelStartEvent += OnLevelStartEvent;
    }

    private void OnLevelStartEvent()
    {
        _tokenSource = new CancellationTokenSource();
        if(audioSource != null)
            LoopPlaying().Forget();
    }

    private async UniTask LoopPlaying()
    {
        while (_tokenSource.IsCancellationRequested == false)
        {
            await OnRunAudioAsync();
        }
    }

    private async UniTask OnRunAudioAsync()
    {
        await UniTask.WaitForSeconds(delayBetweenPlays);
        audioSource.Play();
    }

    private void OnDestroy()
    {
        _tokenSource?.Cancel();
        _worldEventsService.LevelStartEvent -= OnLevelStartEvent;
    }
}
