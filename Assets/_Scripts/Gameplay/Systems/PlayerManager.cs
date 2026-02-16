using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingGame.Systems
{
	public class PlayerManager : MonoBehaviour
	{
        // VARIABLES
        public static PlayerManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }

        // METHODS
        public void ChangeEnvironment(string levelName)
        {
            SceneLoader.Instance.LoadEnvironment(levelName, () =>
            {
                
            });
        }
    }
}