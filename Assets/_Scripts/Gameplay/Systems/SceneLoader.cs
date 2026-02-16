using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using FishingGame.UI;

namespace FishingGame.Systems
{
	public class SceneLoader : MonoBehaviour
	{
		// VARIABLES
		[Header("Settings")]
		[SerializeField] private string[] gameplayScenes;

		[Header("References")]
		[SerializeField] private CanvasGroupFader loadingScreenCanvas;
		[SerializeField] private Image loadingBarImage;

		private List<AsyncOperation> scenesToLoad = new();

		public bool IsLoading => scenesToLoad.Count > 0;

		public event Action OnLoadingBegin;
		public event Action OnLoadingCompleted;

        public static SceneLoader Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        // METHODS
        public void LoadEnvironment(string environmentName, Action onFinishedLoading)
        {
            Time.timeScale = 1f;
            StartCoroutine(LoadEnvironmentCoroutine(environmentName, onFinishedLoading));
        }

        private IEnumerator LoadEnvironmentCoroutine(string environmentName, Action onFinishedLoading)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            OnLoadingBegin?.Invoke();

            loadingScreenCanvas.FadeIn(0.25f);

            yield return new WaitForSecondsRealtime(0.3f);

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentSceneName);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(environmentName, LoadSceneMode.Additive);

            scenesToLoad.Add(unloadOperation);
            scenesToLoad.Add(loadOperation);

            yield return StartCoroutine(LoadingScreenCoroutine());

            Scene scene = SceneManager.GetSceneByName(environmentName);
            SceneManager.SetActiveScene(scene);

            yield return new WaitForSecondsRealtime(0.2f);

            onFinishedLoading?.Invoke();
            loadingScreenCanvas.FadeOut(0.25f);

            yield return new WaitForSecondsRealtime(0.3f);

            loadingBarImage.fillAmount = 0f;

            OnLoadingCompleted?.Invoke();
        }

        public void LoadGameplayScene(string sceneName, List<string> additionalScenes = null)
		{
            Time.timeScale = 1f;
            void loadGameplaySceneAction() => LoadGameplaySceneAction(sceneName, additionalScenes);
            StartCoroutine(LoadCoroutine(loadGameplaySceneAction));
        }

        public void LoadScene(string sceneName)
		{
            Time.timeScale = 1f;
            void loadSimpleSceneAction() => LoadSimpleSceneAction(sceneName);
            StartCoroutine(LoadCoroutine(loadSimpleSceneAction));
        }

        public void LoadGameplayAdditiveScenes()
        {
            foreach (var scene in gameplayScenes)
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            }
        }

        private IEnumerator LoadCoroutine(Action sceneLoadingAction)
        {
            OnLoadingBegin?.Invoke();

            foreach (var button in FindObjectsByType<Button>(FindObjectsSortMode.None))
            {
                button.interactable = false;
            }

            loadingScreenCanvas.FadeIn(0.5f);

            yield return new WaitForSecondsRealtime(0.6f);
            sceneLoadingAction?.Invoke();

            yield return StartCoroutine(LoadingScreenCoroutine());
            yield return new WaitForSecondsRealtime(0.5f);

            loadingScreenCanvas.FadeOut(0.5f);

            yield return new WaitForSecondsRealtime(0.5f);

            loadingBarImage.fillAmount = 0f;

            OnLoadingCompleted?.Invoke();
        }

        private void LoadSimpleSceneAction(string sceneName, List<string> additionalScenes = null)
        {
            scenesToLoad.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single));

            if (additionalScenes != null)
			{
                LoadAdditionalScenes(additionalScenes);
			}
        }

        private void LoadGameplaySceneAction(string staticSceneName, List<string> additionalScenes = null)
        {
            scenesToLoad.Add(SceneManager.LoadSceneAsync(staticSceneName, LoadSceneMode.Single));

            foreach (var scene in gameplayScenes)
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
            }

            if (additionalScenes != null)
            {
                LoadAdditionalScenes(additionalScenes);
            }
        }

        private void LoadAdditionalScenes(List<string> additionalScenes)
        {
            foreach (var additionalScene in additionalScenes)
            {
                scenesToLoad.Add(SceneManager.LoadSceneAsync(additionalScene, LoadSceneMode.Additive));
            }
        }

        private IEnumerator LoadingScreenCoroutine()
		{
			float totalProgress = 0f;
			foreach (var scene in scenesToLoad)
			{
				while(!scene.isDone)
				{
					totalProgress += scene.progress;
                    
                    if (loadingBarImage != null)
                    {
					    loadingBarImage.fillAmount = totalProgress / scenesToLoad.Count;
                    }

					yield return null;
				}
			}

			scenesToLoad = new List<AsyncOperation>();
		}
	}
}