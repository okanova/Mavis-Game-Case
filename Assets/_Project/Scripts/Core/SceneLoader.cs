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

        public static void LoadScene(Scene scene)
        {
            string sceneName = scene.ToString();
            GameManager.Instance.StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private static IEnumerator LoadSceneRoutine(string newScene)
        {
            // Unload current scene (if not null or Boot)
            if (!string.IsNullOrEmpty(_currentScene) && _currentScene != "Boot")
            {
                yield return SceneManager.UnloadSceneAsync(_currentScene);
            }

            // Load new scene
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(newScene, LoadSceneMode.Additive);
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
    }
}