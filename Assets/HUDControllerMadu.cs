using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControllerMadu : MonoBehaviour
{

    public static HUDControllerMadu instance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        instance = this;
    }

    public Text pickupText;

    public void EnableInteractionText(string text)
    {
        pickupText.text = text;
        pickupText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        pickupText.gameObject.SetActive(false);
    }
}
