using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class InventoryManager : MonoBehaviour
    {
        public List<GameObject> ItemsInInventory;
        public List<Transform> Slots;
        public GameObject SlotContainer;
        public int CurrentItemIndex;
        public static InventoryManager Instance;
        public bool isInventoryOpened = false;

        private void Awake()
        {
            Instance = this;
            ItemsInInventory = new List<GameObject>();
        }

        public void Show_Inventory()
        {
            if (isInventoryOpened)
            {
                isInventoryOpened = false;
                Time.timeScale = 1;
                GameCanvas.Instance.Show_GameUI();
                HeroPlayerScript.Instance.FPSHands.SetActive(true);
                SlotContainer.SetActive(false);
                for (int i = 0; i < ItemsInInventory.Count; i++)
                {
                    ItemsInInventory[i].transform.localPosition = Slots[i].transform.localPosition;
                    ItemsInInventory[i].SetActive(false);
                }
            }
            else
            {
                isInventoryOpened = true;
                Time.timeScale = 0;
                GameCanvas.Instance.Text_Current_Object_Name.text = "";
                GameCanvas.Instance.Hide_GameUI();
                HeroPlayerScript.Instance.FPSHands.SetActive(false);
                for (int i = 0; i < ItemsInInventory.Count; i++)
                {
                    ItemsInInventory[i].transform.localPosition = Slots[i].transform.localPosition;
                    ItemsInInventory[i].SetActive(true);
                }
                SlotContainer.SetActive(true);

                if (CurrentItemIndex >= ItemsInInventory.Count)
                {
                    if(ItemsInInventory.Count > 0)
                    {
                        CurrentItemIndex = 0;
                        SlotContainer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        CurrentItemIndex = 0;
                        SlotContainer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                else
                {
                    CurrentItemIndex = 0;
                    SlotContainer.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (SlotContainer.activeSelf && CurrentItemIndex < ItemsInInventory.Count)
                {
                    GameCanvas.Instance.Text_Current_Object_Name.text = ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().Name;
                }
                if(ItemsInInventory.Count > 0)
                {
                    GameCanvas.Instance.Button_Inventory_Drop.SetActive(true);
                    GameCanvas.Instance.Button_Inventory_Use.SetActive(true);
                }
                else
                {
                    GameCanvas.Instance.Button_Inventory_Drop.SetActive(false);
                    GameCanvas.Instance.Button_Inventory_Use.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (SlotContainer.activeSelf && CurrentItemIndex < ItemsInInventory.Count)
            {
                ItemsInInventory[CurrentItemIndex].transform.Rotate(Vector3.up, 50 * Time.deltaTime);
                if (AdvancedGameManager.Instance.controllerType == ControllerType.PcAndConsole)
                {
                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        Go_Left();
                    }
                    else if (Input.GetKeyUp(KeyCode.D))
                    {
                        Go_Right();
                    }
                }
            }
        }

        public bool Has_Inventory_Room()
        {
            if(ItemsInInventory.Count < 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add_Item(GameObject item)
        {
            ItemsInInventory.Add(item);
            item.GetComponent<ItemScript>().DeactivateCollidersAndRigidbody();
            item.transform.parent = SlotContainer.transform;
            item.transform.localScale = item.transform.localScale / 15;
            item.SetActive(false);
        }

        public void Remove_Item()
        {
            Destroy(ItemsInInventory[CurrentItemIndex]);
            ItemsInInventory.RemoveAt(CurrentItemIndex);
        }

        IEnumerator DroppingSomething()
        {
            GameCanvas.Instance.isDroppingSomething = true;
            yield return new WaitForSeconds(1);
            GameCanvas.Instance.isDroppingSomething = false;
        }

        public void Drop_Item()
        {
            StartCoroutine(DroppingSomething());
            ItemsInInventory[CurrentItemIndex].SetActive(true);
            ItemsInInventory[CurrentItemIndex].transform.parent = null;
            ItemsInInventory[CurrentItemIndex].transform.position = HeroPlayerScript.Instance.transform.position + HeroPlayerScript.Instance.transform.forward+ HeroPlayerScript.Instance.transform.up;
            ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().ActivateCollidersAndRigidbody();
            if(ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().itemType == ItemType.Pistol)
            {
                if(HeroPlayerScript.Instance.Hand_Pistol.activeSelf)
                {
                    PistolScript.Instance.enabled = false;
                    HeroPlayerScript.Instance.Hand_Pistol.SetActive(false);
                    GameCanvas.Instance.Button_Hit.gameObject.SetActive(false);
                }
            }
            else if (ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().itemType == ItemType.Key)
            {
                if (HeroPlayerScript.Instance.Hand_Key.activeSelf)
                {
                    HeroPlayerScript.Instance.Hand_Key.SetActive(false);
                }
            }
            else if (ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().itemType == ItemType.BaseBallStick)
            {
                if (HeroPlayerScript.Instance.Hand_Baseball.activeSelf)
                {
                    BaseballScript.Instance.enabled = false;
                    HeroPlayerScript.Instance.Hand_Baseball.SetActive(false);
                    GameCanvas.Instance.Button_Hit.gameObject.SetActive(false);
                }
            }
            ItemsInInventory.RemoveAt(CurrentItemIndex);
            GameCanvas.Instance.Click_Inventory();
        }

        public void Use_Item()
        {
            if(ItemsInInventory.Count > CurrentItemIndex)
            {
                switch (ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().itemType)
                {
                    case ItemType.BaseBallStick:
                        HeroPlayerScript.Instance.Take_BaseballBat_OnHand();
                        GameCanvas.Instance.Click_Inventory();
                        break;
                    case ItemType.MedKit:
                        if (HeroPlayerScript.Instance.Health < 100)
                        {
                            HeroPlayerScript.Instance.Get_Health();
                            Remove_Item();
                            GameCanvas.Instance.Click_Inventory();
                        }
                        break;
                    case ItemType.Pistol:
                        HeroPlayerScript.Instance.Take_Pistol_OnHand();
                        GameCanvas.Instance.Click_Inventory();
                        break;
                    case ItemType.Key:
                        HeroPlayerScript.Instance.Take_Key_OnHand(ItemsInInventory[CurrentItemIndex].GetComponent<KeyScript>().KeyID);
                        GameCanvas.Instance.Click_Inventory();
                        break;
                }
                AudioManager.Instance.Play_Audio_Inventory_Select();
            }
        }

        public void Go_Left()
        {
            if (CurrentItemIndex == 0 && ItemsInInventory.Count == 8)
            {
                RotateObjectByAmount(-45);
                CurrentItemIndex = 7;
            }
            else if (CurrentItemIndex > 0)
            {
                RotateObjectByAmount(-45);
                CurrentItemIndex--;
            }
            if (SlotContainer.activeSelf && CurrentItemIndex < ItemsInInventory.Count)
            {
                GameCanvas.Instance.Text_Current_Object_Name.text = ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().Name;
            }
        }

        public void Go_Right()
        {
            if (CurrentItemIndex == 7 && ItemsInInventory.Count > 0)
            {
                RotateObjectByAmount(45);
                CurrentItemIndex = 0;
            }
            else if (CurrentItemIndex < 7 && ItemsInInventory.Count - 1 > CurrentItemIndex)
            {
                RotateObjectByAmount(45);
                CurrentItemIndex++;
            }
            if (SlotContainer.activeSelf && CurrentItemIndex < ItemsInInventory.Count)
            {
                GameCanvas.Instance.Text_Current_Object_Name.text = ItemsInInventory[CurrentItemIndex].GetComponent<ItemScript>().Name;
            }
        }

        void RotateObjectByAmount(float amount)
        {
            Vector3 currentRotation = SlotContainer.transform.localRotation.eulerAngles;
            float newRotation = currentRotation.y + amount;
            SlotContainer.transform.localRotation = Quaternion.Euler(currentRotation.x, newRotation, currentRotation.z);
            AudioManager.Instance.Play_Audio_GoLeftRightButton();
        }
    }
}