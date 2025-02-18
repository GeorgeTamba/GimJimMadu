using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace AdvancedHorrorFPS
{
    public class AdvancedGameManager : MonoBehaviour
    {
        [Tooltip("Player will die when Enemy AI catches him OR Health Bar will appear and Player will die when his health is run out.")]
        public GameType gameType;

        [Tooltip("According to your choice, Input Controllers will be activated automatically. Keyboard and Mouse for PC and Console Option. Touchpad and Joystick for Mobile.")]
        public ControllerType controllerType;


        [Tooltip("You can burn the Enemies with Blue UV Light Effect on your Flashlight")]
        public bool blueUVLightAttack = true;

        public bool canJump;
        public bool canCrouch;
        public bool canSprint;
        public bool canCarryBoxes;
        public bool canCarryMultipleWeapon = false;
        public bool showHealthBar = true;
        public bool hasInventory = true;

        [Tooltip("Depending on your choice, Blink Effect on Interactable Objects will be active or passive.")]
        public bool blinkOnInteractableObjects = true;
        public bool showCrosshair = true;
        public UnityEvent gameOverEventToInvoke;

        [Header("Escape from Enemy AI Feature")]
        public bool canPlayerEscapeByClickEnough = true;
        public int neededClickAmountToEscape = 10;
        public float durationForClickingTime = 3;

        public static AdvancedGameManager Instance;
        public SaverScript[] SavingAreas;

        [HideInInspector]
        public int Difficulty = 1;

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Difficulty = PlayerPrefs.GetInt("Difficulty", 1);
            GameCanvas.Instance.Crosshair.SetActive(showCrosshair);
            if (!canJump)
            {
                GameCanvas.Instance.Button_Jump.SetActive(false);
            }
            else
            {
                GameCanvas.Instance.Button_Jump.SetActive(true);
            }

            if(!canCrouch)
            {
                GameCanvas.Instance.Button_Crouch.SetActive(false);
            }
            else
            {
                GameCanvas.Instance.Button_Crouch.SetActive(true);
            }

            if (!canSprint)
            {
                GameCanvas.Instance.Button_Sprint.SetActive(false);
            }
            else
            {
                GameCanvas.Instance.Button_Sprint.SetActive(true);
            }

            if (!showHealthBar)
            {
                GameCanvas.Instance.Panel_Health.SetActive(false);
            }
            else if (gameType == GameType.DieWhenYourHealthIsRunOut)
            {
                GameCanvas.Instance.Panel_Health.SetActive(true);
            }

            if (controllerType == ControllerType.Mobile)
            {
                GameCanvas.Instance.Controller_Joystick.SetActive(true);
                GameCanvas.Instance.Controller_Touchpad.SetActive(true);
                GameCanvas.Instance.Button_Pause.SetActive(true);
            }
            else
            {
                GameCanvas.Instance.Controller_Joystick.SetActive(false);
                GameCanvas.Instance.Controller_Touchpad.SetActive(false);
                GameCanvas.Instance.Button_Pause.SetActive(false);
                GameCanvas.Instance.Button_Jump.SetActive(false);
                GameCanvas.Instance.Button_Crouch.SetActive(false);
                GameCanvas.Instance.Button_Sprint.SetActive(false);
                GameCanvas.Instance.Button_Hit.SetActive(false);
                GameCanvas.Instance.Button_Flashlight.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            if (hasInventory)
            {
                if (controllerType == ControllerType.Mobile)
                {
                    GameCanvas.Instance.Button_Inventory.SetActive(true);
                    GameCanvas.Instance.Button_Inventory_Left_Text.SetActive(false);
                    GameCanvas.Instance.Button_Inventory_Right_Text.SetActive(false);
                }
                else
                {
                    GameCanvas.Instance.Button_Inventory.SetActive(false);
                }
            }
            HeroPlayerScript.Instance.characterController.enabled = false;
            HeroPlayerScript.Instance.firstPersonController.enabled = false;
            int lastSavedIndex = PlayerPrefs.GetInt("SavingIndex", -1);
            if (lastSavedIndex != -1 && SavingAreas != null && SavingAreas.Length > 0)
            {
                SaverScript lastSavedPoint = SavingAreas.Where(x => x.SavingIndex == lastSavedIndex).FirstOrDefault();
                if(lastSavedPoint != null)
                {
                    HeroPlayerScript.Instance.transform.position = lastSavedPoint.transform.position;
                }
            }
            HeroPlayerScript.Instance.characterController.enabled = true;
            HeroPlayerScript.Instance.firstPersonController.enabled = true;
        }
    }


    public enum GameType
    {
        DieWhenYouAreCaught,
        DieWhenYourHealthIsRunOut
    }
    public enum ControllerType
    {
        PcAndConsole,
        Mobile
    }
}