using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Core
{
    public static class SceneLoader
    {
        private static string _currentScene;
        
        public static IEnumerator LoadSceneRoutine(Scene scene)
        {
            string newScene = scene.ToString();

            if (!string.IsNullOrEmpty(_currentScene) && _currentScene != "Boot")
            {
                yield return SceneManager.UnloadSceneAsync(_currentScene);
            }

            var handle = Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _currentScene = newScene;
                SceneManager.SetActiveScene(handle.Result.Scene);
            }
            else
            {
                Debug.LogError($"Scene {newScene} failed to load!");
            }
        }

        public static void LoadScene(Scene scene)
        {
            string sceneName = scene.ToString();
            GameManager.Instance.StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private static IEnumerator LoadSceneRoutine(string newScene)
        {
            // Mevcut sahne boş değilse ve Boot değilse unload et
            if (!string.IsNullOrEmpty(_currentScene) && _currentScene != "Boot")
            {
                yield return SceneManager.UnloadSceneAsync(_currentScene);
            }

            // Yeni sahneyi yükle
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _currentScene = newScene;
                SceneManager.SetActiveScene(handle.Result.Scene);
                Debug.Log($"[SceneLoader] {newScene} sahnesi başarıyla yüklendi.");
            }
            else
            {
                Debug.LogError($"[SceneLoader] {newScene} sahnesi yüklenemedi!");
            }
        }
    }
}