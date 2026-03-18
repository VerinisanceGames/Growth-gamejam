using System;
using Core.GameServices;
using Game.Actors;
using Game.Enums;

namespace Game.Services
{
    public class WorldEventsService : IService
    {
        public event Action LevelLoadedEvent;
        public event Action LevelStartEvent;
        public event Action<EGameLooseType, EFertilizerType> LevelLoseEvent;


        public event Action<Character> SpawnPlayerEvent;
        public event Action PlayerTakeFertilizer;

        public event Action<MonsterActor> BossTakeFertilizer;

        public void OnLevelLoaded() => LevelLoadedEvent?.Invoke();

        public void OnLevelStart() => LevelStartEvent?.Invoke();
        public void OnLevelLose(EGameLooseType looseType, EFertilizerType bossType) => LevelLoseEvent?.Invoke(looseType, bossType);
        
        public void OnSpawnPlayer(Character character) => SpawnPlayerEvent?.Invoke(character);
        public void OnPlayerTakeFertilizer() => PlayerTakeFertilizer?.Invoke();
        
        public void OnBossTakeFertilizer(MonsterActor monster) => BossTakeFertilizer?.Invoke(monster);
        
        public Type GetRegisterType() => GetType();
    }
}