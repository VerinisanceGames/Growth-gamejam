using System;
using System.Collections;
using Core;
using Core.GameServices;
using Game.Actors;
using Game.Services;
using UnityEngine;

namespace Game.Managers
{
    public class CutsceneLook : MonoBehaviour, IInjectWorld
    {
        [Header("References")] [SerializeField]
        private Transform playerBody; // Transform персонажа

        [SerializeField] private Transform playerCamera; // Transform камеры

        [Header("Settings")] [SerializeField] private float rotationSpeed = 2f; // Скорость поворота

        [SerializeField] private AnimationCurve easing = // Плавность (S-кривая по умолчанию)
            AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private FirstPersonMovement playerMovement; // Твой скрипт управления
        private float currentXRotation; // Текущий угол камеры по X
        
        private Character _playerCharacter;
        private WorldEventsService _worldEventsService;
        
        
        public void OnInjectWorld(World world)
        {
            _worldEventsService = world.GetService<WorldEventsService>();
            _worldEventsService.SpawnPlayerEvent += OnSpawnPlayerEvent;
        }

        private void OnSpawnPlayerEvent(Character playerCharacter)
        {
            _playerCharacter = playerCharacter;
            
            playerCamera = _playerCharacter.CameraTransform;
            playerBody = _playerCharacter.SelfTransform;
            playerMovement = playerCharacter.GetComponent<FirstPersonMovement>();
        }

        /// <summary>
        /// Повернуть камеру к точке в мире.
        /// </summary>
        public void LookAtPoint(Vector3 worldPoint, float duration, Action onComplete = null)
        {
            StartCoroutine(LookAtRoutine(worldPoint, duration, onComplete));
        }

        private IEnumerator LookAtRoutine(Vector3 targetPoint, float duration, Action onComplete)
        {
            // Блокируем управление игроком
            SetPlayerControlEnabled(false);

            // Запоминаем начальные повороты
            Quaternion startBodyRot = playerBody.rotation;
            Quaternion startCamRot = playerCamera.localRotation;
            float startXRot = currentXRotation;

            // Считаем целевые повороты
            Vector3 dirToTarget = targetPoint - playerCamera.position;
            Quaternion targetFullRot = Quaternion.LookRotation(dirToTarget);

            // Разделяем горизонталь (тело) и вертикаль (камера)
            float targetBodyY = targetFullRot.eulerAngles.y;
            float targetCamX = Mathf.Clamp(
                WrapAngle(targetFullRot.eulerAngles.x), -85f, 85f
            );

            Quaternion targetBodyRot = Quaternion.Euler(0f, targetBodyY, 0f);
            Quaternion targetCamRot = Quaternion.Euler(targetCamX, 0f, 0f);

            // Анимируем поворот
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = easing.Evaluate(Mathf.Clamp01(elapsed / duration));

                playerBody.rotation = Quaternion.Slerp(startBodyRot, targetBodyRot, t);
                playerCamera.localRotation = Quaternion.Slerp(startCamRot, targetCamRot, t);

                yield return null;
            }

            // Фиксируем конечные значения
            playerBody.rotation = targetBodyRot;
            playerCamera.localRotation = targetCamRot;
            currentXRotation = targetCamX;

            // Возвращаем управление и вызываем коллбэк
            SetPlayerControlEnabled(true);
            if (playerMovement != null)
                playerMovement.XRotation = targetCamX;
            
            onComplete?.Invoke();
            
        }

        // Приводим угол из диапазона [0..360] в [-180..180]
        private float WrapAngle(float angle)
            => angle > 180f ? angle - 360f : angle;

        private void SetPlayerControlEnabled(bool enabled)
        {
            if (playerMovement != null)
                playerMovement.enabled = enabled;

            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enabled;
        }


        private void OnDestroy()
        {
            _worldEventsService.SpawnPlayerEvent -= OnSpawnPlayerEvent;
        }
    }
}