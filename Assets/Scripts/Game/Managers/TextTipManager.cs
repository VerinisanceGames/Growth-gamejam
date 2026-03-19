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
    public class TextTipManager : MonoBehaviour, IInjectWorld
    {
        [Header("UI References")] 
        [SerializeField] private TMP_Text _tipsText;

        private float delayBeforeStart1 = 3.0f;
        private float delayBeforeStart2 = 15.0f;
        
        private int _fertilizerIndex;

        private string _textStartGame1 = "WAKE UP! YOUR SHIFT IS STARTING!\nFERTILIZE 10 TIMES AND YOU ARE OFF FOR TODAY.";
        private string _textStartGame2 = "TAKE THE FERTILIZER AND DELIVER IT.\n30 SECONDS IS ENOUGHF.";
        private string _textTip1 = "FIRST ONE IS FOR LILI. HURRY UP!";
        private string _textTip6 = "NEXT ONE IS FOR FRED. GOOD LUCK!";
        private string _textTip10 = "LAST ONE. POISON. YOU KNOW WHAT TO DO.";
        private string _textGameLostTimer = "TOO SLOW. WAKE UP THE NEXT ONE.";
        private string _textGameLostFedWrong = "POOR WORKER. CLEAN OUT LAB 12.\nWAKE UP THE NEXT ONE.";
        private string _textGameLostFedJared = "HE DID FED JARED. NOT A SMART ONE.\nCLEAN OUT LAB 12. WAKE UP THE NEXT ONE.";
        //
        private string _textFlytrapAngry = "NO TASTY! NO TASTY!";
        
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
            _tipsText.text = "";
            // if (type == EGameLooseType.Fed_wrong) {
            //     if (bossType == EFertilizerType.Boss_1) {
            //         _tipsText.text = _textFlytrapAngry;
            //     }
            // }
            //
            await UniTask.WaitForSeconds(3.5f);
            if (type == EGameLooseType.Timer) {
                _tipsText.text = _textGameLostTimer;
            } else  if (type == EGameLooseType.Fed_wrong) {
                _tipsText.text = _textGameLostFedWrong;
            } else  if (type == EGameLooseType.Fed_jared) {
                _tipsText.text = _textGameLostFedJared;
            }
        }

        private void OnPlayerTakeFertilizer()
        {
            _fertilizerIndex++;

            if (_fertilizerIndex == 1) {
                _tipsText.text = _textTip1;
            } else if (_fertilizerIndex == 6) {
                _tipsText.text = _textTip6;
            } else if (_fertilizerIndex == 10) {
                _tipsText.text = _textTip10;
            }
        }

        private void OnBossTakeFertilizer(MonsterActor bossActor)
        {
            _tipsText.text = "";
        }

        private async void OnLevelStartEvent()
        {
            _fertilizerIndex = 0;

            await UniTask.WaitForSeconds(delayBeforeStart1);
            _tipsText.text = _textStartGame1;

            await UniTask.WaitForSeconds(delayBeforeStart2);
            if (_fertilizerIndex == 0) {
                _tipsText.text = _textStartGame2;
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