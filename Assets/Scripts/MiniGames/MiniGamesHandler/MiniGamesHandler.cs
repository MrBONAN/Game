using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private Camera zoneCamera;
        private CurrentMiniGameHandler currentMiniGameHandler;
        private Queue<CurrentMiniGameHandler> miniGamesQueue = new();
        private Vector2 cameraSettings = new(0.8f, 0.8f);
        public Vector2 targetCameraPosition = new(0.1f, 0.1f);
        // Камера как бы пролетает targetPosition до bottomPosition, и потом возвращается к target
        public Vector2 bottomCameraPosition = new(0.1f, 0.05f);

        public bool IsMiniGameActive
            => currentMiniGameHandler is not null && currentMiniGameHandler.isMiniGameExist;

        private void Awake()
        {
            zoneCamera = GetComponentInChildren<Camera>();
            zoneCamera.enabled = false;

            zoneCamera.rect = new Rect(targetCameraPosition, cameraSettings);
        }

        public void CreateMiniGame(CurrentMiniGameHandler miniGameHandler)
        {
            if (currentMiniGameHandler is not null && currentMiniGameHandler.isMiniGameExist) return;
            if (currentMiniGameHandler is not null)
            {
                miniGamesQueue.Enqueue(miniGameHandler);
                return;
            }
            currentMiniGameHandler = miniGameHandler;
            currentMiniGameHandler.transform.position = zoneCamera.transform.position + new Vector3(0, 0, 1);
            StartCoroutine(OpenMiniGameAnimation());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMiniGame()
        {
            if (currentMiniGameHandler is null || !currentMiniGameHandler.isMiniGameExist) return;
            var result = currentMiniGameHandler.UpdateMiniGame();
            if (result is not MiniGameResult.ContinuePlay) CloseMiniGame();
        }

        public void CloseMiniGame()
        {
            if (currentMiniGameHandler is null) return;
            currentMiniGameHandler.CloseGame();
            currentMiniGameHandler = null;
            StartCoroutine(CloseMiniGameAnimation());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator OpenMiniGameAnimation()
        {
            yield return new WaitForSeconds(0.01f);
            var startCameraPosition = new Vector2(targetCameraPosition.x, 1.1f);
            yield return CameraMove(startCameraPosition, bottomCameraPosition, 0.07f);
            yield return CameraMove(zoneCamera.rect.position, targetCameraPosition, 0.04f);
            currentMiniGameHandler.StartMiniGame();
            yield return new WaitForSeconds(0.01f);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator CloseMiniGameAnimation()
        {
            yield return new WaitForSeconds(0.01f);
            var endCameraPosition = new Vector2(targetCameraPosition.x, 1.1f);
            yield return CameraMove(targetCameraPosition, bottomCameraPosition, 0.1f);
            yield return CameraMove(zoneCamera.rect.position, endCameraPosition, 0.07f);
            zoneCamera.enabled = false;
            yield return new WaitForSeconds(0.01f);
            if (miniGamesQueue.Count > 0)
                CreateMiniGame(miniGamesQueue.Dequeue());
        }

        private IEnumerator CameraMove(Vector2 currentPosition, Vector2 targetPosition, float speed)
        {
            zoneCamera.rect = new Rect(currentPosition, cameraSettings);
            zoneCamera.enabled = true;
            var velocity = Vector2.zero;
            while ((currentPosition - targetPosition).magnitude > 0.01f)
            {
                currentPosition = Vector2.SmoothDamp(currentPosition, targetPosition, ref velocity, speed);
                zoneCamera.rect = new Rect(currentPosition, cameraSettings);
                yield return new WaitForSeconds(0.01f);
            }
            zoneCamera.rect = new Rect(targetPosition, cameraSettings);
        }
    }
}