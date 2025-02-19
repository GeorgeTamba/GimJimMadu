using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float sensitivity = 2f; // Default sensitivity
    private float rotationX = 0f;
    private Transform playerBody;

    void Start()
    {
        playerBody = transform.parent; // Pastikan kamera ada dalam parent (Player)

        // Load sensitivitas dari PlayerPrefs jika tersedia
        if (PlayerPrefs.HasKey("Sensitivity"))
            sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); // Batasi rotasi vertikal agar tidak terbalik

        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
