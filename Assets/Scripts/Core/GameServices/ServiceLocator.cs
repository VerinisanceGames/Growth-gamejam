using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.GameServices
{
    public class ServiceLocator
    {
        private Dictionary<Type, IService> _services = new();

        public void Add<TService>(TService Service) where TService : IService
        {
            _services[Service.GetRegisterType()] = Service;
        }

        public TService Get<TService>() where TService : IService
        {
            return (TService)_services[typeof(TService)];
        }
        
    }
}