using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class AdvancedFPSInputManager
    {
        private static AdvancedFPSInputManager _instance;

        private static AdvancedFPSInputManager Instance
        {
            get { return _instance ?? (_instance = new AdvancedFPSInputManager()); }
        }

        private AdvancedFPSInputManager() { }

        private Dictionary<string, List<VirtualAxis>> _virtualAxisDictionary =
            new Dictionary<string, List<VirtualAxis>>();

        private Dictionary<string, List<VirtualButton>> _virtualButtonsDictionary =
            new Dictionary<string, List<VirtualButton>>();

        public static int TouchCount
        {
            get
            {
                return Input.touchCount;
            }
        }

        public static Touch GetTouch(int touchIndex)
        {
            return Input.GetTouch(touchIndex);
        }

        public static float GetAxis(string axisName)
        {
            return GetAxis(axisName, false);
        }

        public static float GetAxisRaw(string axisName)
        {
            return GetAxis(axisName, true);
        }

        private static float GetAxis(string axisName, bool isRaw)
        {
            if (AxisExists(axisName))
            {
                return GetVirtualAxisValue(Instance._virtualAxisDictionary[axisName], axisName, isRaw);
            }

            if (ButtonExists(axisName))
            {
                var anyButtonIsPressed = GetAnyVirtualButton(Instance._virtualButtonsDictionary[axisName]);
                if (anyButtonIsPressed)
                {
                    return 1f;
                }
            }

            return isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
        }

        public static bool GetButton(string buttonName)
        {
            var standardInputButtonState = Input.GetButton(buttonName);
            if (standardInputButtonState == true) return true;

            if (ButtonExists(buttonName))
            {
                return GetAnyVirtualButton(Instance._virtualButtonsDictionary[buttonName]);
            }

            return false;
        }

        public static bool GetButtonDown(string buttonName)
        {
            var standardInputButtonState = Input.GetButtonDown(buttonName);
            if (standardInputButtonState == true) return true;

            if (ButtonExists(buttonName))
            {
                return GetAnyVirtualButtonDown(Instance._virtualButtonsDictionary[buttonName]);
            }

            return false;
        }

        public static bool GetButtonUp(string buttonName)
        {
            var standardInputButtonState = Input.GetButtonUp(buttonName);
            if (standardInputButtonState == true) return true;

            if (ButtonExists(buttonName))
            {
                return GetAnyVirtualButtonUp(Instance._virtualButtonsDictionary[buttonName]);
            }

            return false;
        }

        public static bool AxisExists(string axisName)
        {
            return Instance._virtualAxisDictionary.ContainsKey(axisName);
        }

        public static bool ButtonExists(string buttonName)
        {
            return Instance._virtualButtonsDictionary.ContainsKey(buttonName);
        }

        public static void RegisterVirtualAxis(VirtualAxis virtualAxis)
        {
            if (!Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
            {
                Instance._virtualAxisDictionary[virtualAxis.Name] = new List<VirtualAxis>();
            }

            Instance._virtualAxisDictionary[virtualAxis.Name].Add(virtualAxis);
        }

        public static void UnregisterVirtualAxis(VirtualAxis virtualAxis)
        {
            if (Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
            {
                if (!Instance._virtualAxisDictionary[virtualAxis.Name].Remove(virtualAxis))
                {
                    Debug.LogError("Requested axis " + virtualAxis.Name + " exists.");
                }
            }
            else
            {
                Debug.LogError("Trying to unregister an axis " + virtualAxis.Name + " is not registered");
            }
        }

        public static void RegisterVirtualButton(VirtualButton virtualButton)
        {
            if (!Instance._virtualButtonsDictionary.ContainsKey(virtualButton.Name))
            {
                Instance._virtualButtonsDictionary[virtualButton.Name] = new List<VirtualButton>();
            }

            Instance._virtualButtonsDictionary[virtualButton.Name].Add(virtualButton);
        }

        public static void UnregisterVirtualButton(VirtualButton virtualButton)
        {
            if (Instance._virtualButtonsDictionary.ContainsKey(virtualButton.Name))
            {
                if (!Instance._virtualButtonsDictionary[virtualButton.Name].Remove(virtualButton))
                {
                    Debug.LogError("Requested button axis exists, but there's no such virtual button that you're trying to unregister");
                }
            }
            else
            {
                Debug.LogError("Trying to unregister a button that was never registered");
            }
        }

        private static float GetVirtualAxisValue(List<VirtualAxis> virtualAxisList, string axisName, bool isRaw)
        {
            float axisValue = isRaw ? Input.GetAxisRaw(axisName) : Input.GetAxis(axisName);
            if (!Mathf.Approximately(axisValue, 0f))
            {
                return axisValue;
            }

            for (int i = 0; i < virtualAxisList.Count; i++)
            {
                var currentAxisValue = virtualAxisList[i].Value;
                if (!Mathf.Approximately(currentAxisValue, 0f))
                {
                    return currentAxisValue;
                }
            }

            return 0f;
        }

        private static bool GetAnyVirtualButtonDown(List<VirtualButton> virtualButtons)
        {
            for (int i = 0; i < virtualButtons.Count; i++)
            {
                if (virtualButtons[i].GetButtonDown) return true;
            }

            return false;
        }

        private static bool GetAnyVirtualButtonUp(List<VirtualButton> virtualButtons)
        {
            for (int i = 0; i < virtualButtons.Count; i++)
            {
                if (virtualButtons[i].GetButtonUp) return true;
            }

            return false;
        }

        private static bool GetAnyVirtualButton(List<VirtualButton> virtualButtons)
        {
            for (int i = 0; i < virtualButtons.Count; i++)
            {
                if (virtualButtons[i].GetButton) return true;
            }

            return false;
        }

    }
}

