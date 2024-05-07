using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Timer timer; // Timer object reference
    public GameObject shopUI; // Shop object reference (UI)

    public float timeBetweenShopActivations = 300f; // Time between shop activations in seconds
    private float timeSinceLastActivation = 0f; // Time passed since the last shop activation

    private bool shopUIActive = false; // Toggle shop interface;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!shopUIActive)
        {
            // Increment time since last activation
            timeSinceLastActivation += Time.deltaTime;

            // Check if enough time has passed for another shop activation
            if (timeSinceLastActivation >= timeBetweenShopActivations)
            {
                ActivateShop();
                timeSinceLastActivation = 0f; // Reset time since last activation
            }
        }
    }

    void ActivateShop()
    {
        // Pause Game
        Time.timeScale = 0f;
        // Bring up ShopUI
        shopUI.SetActive(true);
        // Set shop UI active flag
        shopUIActive = true;
        // Handle Purchases if necessary
    }

    public void DeactivateShop()
    {
        // Unpause Game
        Time.timeScale = 1f;
        // Hide ShopUI
        shopUI.SetActive(false);
        // Set shop UI active flag
        shopUIActive = false;
    }

    // Method to be called when the player interacts with the shop
    public void PlayerInteractedWithShop()
    {
        if (!shopUIActive)
        {
            ActivateShop();
        }
    }
}
