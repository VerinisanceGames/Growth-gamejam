using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class GameInstance : MonoBehaviour
    {
        [SerializeField] private int _firstSceneIndex;

        private static GameInstance _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            LoadScene(_firstSceneIndex);
        }

        public void LoadScene(int sceneIndex)
        {
            StartCoroutine(LoadSceneAsync(sceneIndex));
        }

        private IEnumerator LoadSceneAsync(int sceneIndex)
        {
            AsyncOperation handler = SceneManager.LoadSceneAsync(sceneIndex);

            while (handler.isDone == false)
            {
                yield return null;
            }

            var worldInstance = FindFirstObjectByType<World>();
            if (worldInstance != null)
                worldInstance.Initialize(this);
        }
    }
}
