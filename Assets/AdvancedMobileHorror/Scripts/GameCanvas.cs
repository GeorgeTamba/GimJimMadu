using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace AdvancedHorrorFPS
{
    public class GameCanvas : MonoBehaviour
    {
        public static GameCanvas Instance;
        public Image image_Blinking;
        public Image image_Healing;
        public GameObject Indicator_BlueLight;
        public GameObject Panel_ClickToEscape;
        public Image Image_ClickToEscape;
        public Image Image_BlueLight;
        public GameObject Panel_WarningPanel;
        public GameObject Button_Flashlight;
        public GameObject Button_Jump;
        public GameObject Button_Crouch;
        public GameObject Button_Sprint;
        public GameObject Image_CaughtBlood;
        public GameObject Button_Pause;
        public LayerMask layerMaskForInteract;
        public Color BlueLightcolor;
        public GameObject Panel_GameUI;
        public GameObject Panel_Pause;
        public GameObject Panel_Health;
        public Image Slider_Stamina;
        public GameObject Panel_Ammo;
        public GameObject Panel_Settings;
        public GameObject Panel_Note;
        public GameObject Panel_Note_Text;
        public GameObject Button_HideOut;
        public Text Button_Close_Note_Text;
        public GameObject Panel_GameOver;
        public Image Image_Sprite_Blood;
        public GameObject Controller_Joystick;
        public GameObject Controller_Touchpad;
        public Text Text_Health;
        public Text Text_Ammo;
        [HideInInspector]
        public bool isFlashBlueNow = false;
        [HideInInspector]
        public GameObject LastClickedArea;
        [HideInInspector]
        public NoteScript CurrentNote;
        private bool isGameOver = false;
        [HideInInspector]
        public bool isPaused = false;
        public GameObject Button_Hit;
        public GameObject Crosshair;
        public Sprite Pistol_Fire;
        public Sprite BaseballStick_Hit;
        public Image Button_FireOrHit;

        public GameObject Panel_Inventory;
        public GameObject Button_Inventory;
        public GameObject Button_Inventory_Use;
        public GameObject Button_Inventory_Drop;
        public GameObject Button_Inventory_Left_Text;
        public GameObject Button_Inventory_Right_Text;
        public Text Text_Current_Object_Name;

        private void Awake()
        {
            Instance = this;
        }

        public void Click_BacktoMenu()
        {
            SceneManager.LoadScene("Scene_MainMenu");
        }

        public void Click_Inventory_UseIt()
        {
            InventoryManager.Instance.Use_Item();
        }

        public void Click_Inventory_Drop()
        {
            InventoryManager.Instance.Drop_Item();
        }

        public void Click_Inventory()
        {
            if (PistolScript.Instance.enabled && PistolScript.Instance.isFiring) return;
            if (BaseballScript.Instance.enabled && BaseballScript.Instance.isHitting) return;

            if (Panel_Inventory.activeSelf)
            {
                Panel_Inventory.SetActive(false);
                InventoryManager.Instance.Show_Inventory();
                if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                HeroPlayerScript.Instance.ActivatePlayer();
                HeroPlayerScript.Instance.ActivatePlayerInputs();
                CameraLook.Instance.enabled = true;
                if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
                {
                    GameCanvas.Instance.Button_Inventory.SetActive(true);
                }
            }
            else
            {
                Panel_Inventory.SetActive(true);
                InventoryManager.Instance.Show_Inventory();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                HeroPlayerScript.Instance.DeactivatePlayer();
                CameraLook.Instance.enabled = false;
                if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
                {
                    GameCanvas.Instance.Button_Inventory.SetActive(false);
                }
            }
        }

        public void Click_Inventory_NoHands()
        {
            HeroPlayerScript.Instance.ToggleHandObjects();
            GameCanvas.Instance.Click_Inventory();
        }

        public void Click_Inventory_Left()
        {
            InventoryManager.Instance.Go_Left();
        }

        public void Click_Inventory_Right()
        {
            InventoryManager.Instance.Go_Right();
        }

        public void Click_Hit()
        {
            if (PistolScript.Instance.isGrabbed && PistolScript.Instance.enabled)
            {
                PistolScript.Instance.Fire();
            }
            else if (BaseballScript.Instance.isGrabbed && BaseballScript.Instance.enabled)
            {
                BaseballScript.Instance.Hit();
            }
        }

        public void Click_Escape()
        {
            if (AdvancedGameManager.Instance.canPlayerEscapeByClickEnough)
            {
                if (Image_ClickToEscape.fillAmount < 1)
                {
                    float factor = 1f / AdvancedGameManager.Instance.neededClickAmountToEscape;
                    Image_ClickToEscape.fillAmount = Image_ClickToEscape.fillAmount + factor;
                    CameraLook.Instance.fCamShakeImpulse = 0.5f;
                    if (Image_ClickToEscape.fillAmount >= 1)
                    {
                        // Can Escape!
                        Image_ClickToEscape.fillAmount = 0;
                        Panel_ClickToEscape.SetActive(false);
                        HeroPlayerScript.Instance.Escape();
                    }
                }
            }
        }

        public void Switch_HitButtonToPistol()
        {
            Button_FireOrHit.sprite = Pistol_Fire;
            Panel_Ammo.SetActive(true);
        }

        public void Switch_HitButtonToBaseball()
        {
            Button_FireOrHit.sprite = BaseballStick_Hit;
            Panel_Ammo.SetActive(false);
        }



        public void Show_Blood_Effect()
        {
            Image_Sprite_Blood.gameObject.SetActive(true);
            Image_Sprite_Blood.GetComponent<Animation>().Play("BloodEffect");
            StartCoroutine(HideEffect());
        }

        public void Click_Button_HideOut()
        {
            if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                if (HeroPlayerScript.Instance.isHiding && HeroPlayerScript.Instance.hidingPlace != null)
                {
                    HeroPlayerScript.Instance.hidingPlace.Hide();
                }
            }
        }

        public void Blink()
        {
            image_Blinking.GetComponent<Animation>().Play();
        }

        IEnumerator HideEffect()
        {
            yield return new WaitForSeconds(1);
            Image_Sprite_Blood.gameObject.SetActive(false);
        }

        public void Click_Continue()
        {
            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            HeroPlayerScript.Instance.ActivatePlayerInputs();
            CameraLook.Instance.enabled = true;
            Time.timeScale = 1;
            isPaused = false;
            Panel_Pause.SetActive(false);
            Panel_Settings.SetActive(false);
            Panel_GameUI.SetActive(true);
        }

        public void ShowHint(string text)
        {
            StartCoroutine(ShowHintInTime(text));
        }

        IEnumerator ShowHintInTime(string text)
        {
            yield return new WaitForSeconds(0.25f);
            GameCanvas.Instance.Show_Warning(text);
            yield return new WaitForSeconds(3);
            GameCanvas.Instance.Hide_Warning();
        }

        public void Click_Pause()
        {
            if (InventoryManager.Instance.isInventoryOpened) return;
            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            HeroPlayerScript.Instance.DeactivatePlayer();
            CameraLook.Instance.enabled = false;
            Time.timeScale = 0;
            isPaused = true;
            Panel_Pause.SetActive(true);
            Panel_GameUI.SetActive(false);
        }

        public void UpdateHealth()
        {
            Text_Health.text = HeroPlayerScript.Instance.Health.ToString();
        }

        public void IncreaseHealth()
        {
            StartCoroutine(HealingEffect());
        }

        IEnumerator HealingEffect()
        {
            image_Healing.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            image_Healing.gameObject.SetActive(false);
        }

        [HideInInspector]
        public bool isDroppingSomething = false;

        void Update()
        {
            if (isDroppingSomething) return;

            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver && !Panel_Note.activeSelf)
                {
                    if (isPaused)
                    {
                        Click_Continue();
                    }
                    else
                    {
                        Click_Pause();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Tab) && !isGameOver)
                {
                    Click_Inventory();
                }
            }
            if (AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 2.5f, layerMaskForInteract))
                        {
                            if (hit.transform.GetComponent<ItemScript>() != null)
                            {
                                LastClickedArea = hit.transform.gameObject;
                            }
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 2.5f, layerMaskForInteract))
                        {
                            if (hit.transform.GetComponent<ItemScript>() != null && LastClickedArea == hit.transform.gameObject)
                            {
                                ItemScript item = hit.transform.GetComponent<ItemScript>();
                                if (HeroPlayerScript.Instance.isHoldingBox && item.GetComponent<BoxCollider>() != null)
                                {
                                    item.Interact();
                                }
                                else if (!HeroPlayerScript.Instance.isHoldingBox)
                                {
                                    item.Interact();
                                }
                            }
                        }
                    }
                }
            }
            if (AdvancedGameManager.Instance.canPlayerEscapeByClickEnough)
            {
                if (Panel_ClickToEscape.activeSelf)
                {
                    Image_ClickToEscape.fillAmount -= Time.deltaTime * 0.2f;
                }
            }
        }

        public void Show_GameOverPanel()
        {
            HeroPlayerScript.Instance.DeactivatePlayer();
            CameraLook.Instance.enabled = false;
            isGameOver = true;
            if (AdvancedGameManager.Instance.gameType == GameType.DieWhenYouAreCaught)
            {
                StartCoroutine(WaitAndShowGameOver(3));
            }
            else
            {
                StartCoroutine(WaitAndShowGameOver(1));
            }
        }

        IEnumerator WaitAndShowGameOver(int time)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            CameraLook.Instance.enabled = false;
            if(AdvancedGameManager.Instance.gameType == GameType.DieWhenYourHealthIsRunOut)
            {
                CameraLook.Instance.GetComponent<Animation>().Play();
            }
            yield return new WaitForSeconds(time);
            Time.timeScale = 0;
            Panel_GameUI.SetActive(false);
            Panel_GameOver.SetActive(true);
        }

        public void Click_Settings()
        {
            Panel_Settings.SetActive(true);
            Panel_Pause.SetActive(false);
        }

        public void Click_Close_Settings()
        {
            Panel_Settings.SetActive(false);
            Panel_Pause.SetActive(true);
        }

        public void Click_ShowNote()
        {
            Panel_GameUI.SetActive(false);
            HeroPlayerScript.Instance.DeactivatePlayer();
        }

        public void Click_Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Show_Note(string text)
        {
            Panel_GameUI.SetActive(false);
            HeroPlayerScript.Instance.DeactivatePlayer();
            Panel_Note.SetActive(true);
            Panel_Note_Text.GetComponent<Text>().text = text;
            AudioManager.Instance.Play_Note_Reading();
            if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
            {
                Button_Close_Note_Text.text = "CLOSE (E)";
            }
        }


        public void Hide_Note()
        {
            Panel_GameUI.SetActive(true);
            HeroPlayerScript.Instance.ActivatePlayerInputs();
            if (CurrentNote != null)
            {
                CurrentNote.Unread();
                Panel_Note.SetActive(false);
                Panel_Note_Text.GetComponent<Text>().text = "";
            }
            AudioManager.Instance.Play_Item_Close();
        }


        public void Show_GameUI()
        {
            Panel_GameUI.SetActive(true);
        }

        public void Hide_GameUI()
        {
            Panel_GameUI.SetActive(false);
        }

        public void Show_Warning(String textID)
        {
            ShowWarningPanel(textID);
        }

        void ShowWarningPanel(String text)
        {
            Panel_WarningPanel.SetActive(true);
            Panel_WarningPanel.GetComponentInChildren<Text>().text = text;
        }

        public void Hide_Warning()
        {
            Panel_WarningPanel.SetActive(false);
            Panel_WarningPanel.GetComponentInChildren<Text>().text = "";
        }

        public void Activate_FlashLightButton()
        {
            Button_Flashlight.SetActive(true);
        }

        public void Activate_FiringButton()
        {
            if(AdvancedGameManager.Instance.controllerType == ControllerType.Mobile)
            {
                Button_Hit.SetActive(true);
            }
        }

        public void Deactivate_FiringButton()
        {
            Button_Hit.SetActive(false);
        }

        public void Activate_PistolPanel()
        {
            Panel_Ammo.SetActive(true);
        }

        public void Deactivate_PistolPanel()
        {
            Panel_Ammo.SetActive(false);
        }


        public void Update_Text_Ammo(int i, int capacity)
        {
            Text_Ammo.text = i.ToString() + "/" + capacity.ToString();
        }

        public void FlashLight_Click()
        {
            FlashLightScript.Instance.FlashLight_Decision(true);
            AudioManager.Instance.Play_Flashlight_Open();
            if (AdvancedGameManager.Instance.blueUVLightAttack)
            {
                Button_Flashlight.GetComponent<Image>().color = BlueLightcolor;
            }
            else
            {
                GameCanvas.Instance.Button_Flashlight.SetActive(false);
            }
        }

        public void FlashLight_BlueEffect_Down()
        {
            if (FlashLightScript.Instance.Light.enabled && !isFlashBlueNow && FlashLightScript.Instance.BlueBattery > 0 && AdvancedGameManager.Instance.blueUVLightAttack)
            {
                isFlashBlueNow = true;
                FlashLightScript.Instance.Light.color = Color.blue;
                AudioManager.Instance.Play_Flashlight_Close();
            }
        }

        public void FlashLight_BlueEffect_Up()
        {
            if (FlashLightScript.Instance.Light.enabled && isFlashBlueNow && AdvancedGameManager.Instance.blueUVLightAttack)
            {
                FlashLightScript.Instance.StopAudioBlueLight();
                isFlashBlueNow = false;
                FlashLightScript.Instance.Light.color = Color.white;
                FlashLightScript.Instance.Light.intensity = 3;
            }
        }

        public void Drop_GrabbedLadder(Transform LadderPutPoint)
        {
            HeroPlayerScript.Instance.Carrying_Ladder.SetActive(true);
            HeroPlayerScript.Instance.Carrying_Ladder.transform.parent = null;
            HeroPlayerScript.Instance.Carrying_Ladder.transform.position = LadderPutPoint.transform.position;
            HeroPlayerScript.Instance.Carrying_Ladder.transform.eulerAngles = LadderPutPoint.transform.eulerAngles;
            HeroPlayerScript.Instance.Carrying_Ladder.transform.localScale = LadderPutPoint.transform.localScale;
            HeroPlayerScript.Instance.Carrying_Ladder.GetComponent<BoxCollider>().enabled = true;
            HeroPlayerScript.Instance.Carrying_Ladder.tag = "Untagged";
            if(HeroPlayerScript.Instance.Carrying_Ladder.GetComponent<ItemScript>() != null)
            {
                Destroy(HeroPlayerScript.Instance.Carrying_Ladder.GetComponent<ItemScript>());
                Destroy(HeroPlayerScript.Instance.Carrying_Ladder.GetComponent<LadderScript>());
            }
            AudioManager.Instance.Play_Item_Grab();
            HeroPlayerScript.Instance.Carrying_Ladder.GetComponent<LadderScript>().isPut = true;
            HeroPlayerScript.Instance.Carrying_Ladder = null;
            Destroy(LadderPutPoint.gameObject);
        }
    }
}