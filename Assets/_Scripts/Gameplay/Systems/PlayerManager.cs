using UnityEngine;
using UnityEngine.InputSystem;

namespace FishingGame.Systems
{
	public class PlayerManager : MonoBehaviour
	{
        // VARIABLES
        [SerializeField] private PlayerFishingController fishingController;

        public PlayerWallet Wallet { get; private set; }

        public static PlayerManager Instance { get; private set; }

        // EXECUTION FUNCTIONS
        private void Awake()
        {
            Instance = this;
            Wallet = new();
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            if (Keyboard.current.fKey.wasPressedThisFrame)
            {
                fishingController.BeginFishing();
            }
        }

        // INPUT
        private void OnFish(InputValue input)
        {
            fishingController.SetCatchBarMovementActive(input.isPressed);
        }

        private void OnPause(InputValue input)
        {
            Application.Quit();
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