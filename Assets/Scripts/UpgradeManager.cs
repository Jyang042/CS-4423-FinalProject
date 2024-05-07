using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradeCardPrefab; // Reference to the UpgradeCard prefab
    public RectTransform[] cardSpawnPoints; // Array of spawn points for the cards (RectTransform positions)
    public Upgrade[] availableUpgrades; // Array of available upgrades
    public RectTransform panel; // Reference to the panel where you want to spawn the cards

    void Start()
    {
        // Ensure that there are available upgrades and spawn points
        if (availableUpgrades.Length == 0 || cardSpawnPoints.Length == 0 || panel == null)
        {
            Debug.LogError("Insufficient upgrades, spawn points, or panel not assigned.");
            return;
        }

        // Spawn UpgradeCard prefabs on the panel and display upgrade information
        for (int i = 0; i < Mathf.Min(availableUpgrades.Length, cardSpawnPoints.Length); i++)
        {
            // Convert normalized position to world position
            Vector3 spawnPosition = panel.TransformPoint(cardSpawnPoints[i].anchoredPosition);

            // Spawn UpgradeCard prefab at the world position
            GameObject upgradeCardObject = Instantiate(upgradeCardPrefab, spawnPosition, Quaternion.identity, panel);
            UpgradeCard upgradeCard = upgradeCardObject.GetComponent<UpgradeCard>();

            if (upgradeCard != null)
            {
                upgradeCard.SetUpgradeInfo(availableUpgrades[i]);
            }
            else
            {
                Debug.LogError("UpgradeCard component not found on prefab.");
                return;
            }
        }
    }
}
