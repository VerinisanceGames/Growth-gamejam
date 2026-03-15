using System;
using Core.GameServices;

namespace Game.Services
{
    public class WorldEventsService : IService
    {
        public event Action LevelLoadedEvent;


        public void OnLevelLoaded() => LevelLoadedEvent?.Invoke();

        public Type GetRegisterType() => GetType();
    }
}