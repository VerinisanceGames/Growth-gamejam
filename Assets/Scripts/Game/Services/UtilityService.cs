using System;
using Core.GameServices;
using UnityEngine;

namespace Game.Services
{
    public class UtilityService : MonoBehaviour, IService
    {
        [SerializeField] private float _distanceToHZ;

        public Type GetRegisterType() => GetType();
    }
}