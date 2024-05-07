using UnityEngine;

public class UpgradeDisplay : MonoBehaviour
{
    public GameObject upgradeCardPrefab; // Reference to the upgrade card prefab
    public Transform cardPanel; // Panel to hold the upgrade cards
    public int numRows = 2; // Number of rows in the grid layout
    public int numCols = 3; // Number of columns in the grid layout
    public float cardSpacingX = 1.5f; // Spacing between cards horizontally
    public float cardSpacingY = 1.5f; // Spacing between cards vertically
    public Upgrade[] allUpgrades; // Array of all available upgrades
    public float[] rarityWeights; // Array of weighted rarities

    void Start()
    {
        DisplayUpgradeCards();
    }

    void DisplayUpgradeCards()
    {
        // Clear existing upgrade cards
        foreach (Transform child in cardPanel)
        {
            Destroy(child.gameObject);
        }

        // Calculate total number of cards
        int numCards = numRows * numCols;

        // Calculate card panel width and height
        float panelWidth = numCols * cardSpacingX;
        float panelHeight = numRows * cardSpacingY;

        // Calculate starting position
        Vector3 startPos = cardPanel.position - new Vector3(panelWidth / 2f, panelHeight / 2f, 0f);

        // Instantiate upgrade cards in a grid layout
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                // Calculate position for the current card
                Vector3 cardPos = startPos + new Vector3(col * cardSpacingX, row * cardSpacingY, 0f);

                // Instantiate upgrade card prefab at the calculated position
                GameObject card = Instantiate(upgradeCardPrefab, cardPos, Quaternion.identity, cardPanel);
                UpgradeCard upgradeCard = card.GetComponent<UpgradeCard>();
                if (upgradeCard != null)
                {
                    // Set upgrade information on the card
                    Upgrade upgrade = GetRandomUpgrade();
                    upgradeCard.SetUpgradeInfo(upgrade);
                }
            }
        }
    }

    Upgrade GetRandomUpgrade()
    {
        // Ensure there are upgrades and rarity weights
        if (allUpgrades == null || allUpgrades.Length == 0 || rarityWeights == null || rarityWeights.Length != (int)Upgrade.Rarity.Mythic + 1)
        {
            Debug.LogWarning("Invalid upgrades or rarity weights.");
            return null;
        }

        // Calculate total rarity weight
        float totalWeight = 0f;
        foreach (float weight in rarityWeights)
        {
            totalWeight += weight;
        }

        // Generate a random value between 0 and total weight
        float randomValue = Random.Range(0f, totalWeight);

        // Determine the rarity based on the random value
        float cumulativeWeight = 0f;
        for (int i = 0; i < rarityWeights.Length; i++)
        {
            cumulativeWeight += rarityWeights[i];
            if (randomValue <= cumulativeWeight)
            {
                // Filter upgrades of the specified rarity
                Upgrade[] upgradesOfRarity = System.Array.FindAll(allUpgrades, u => u.rarity == (Upgrade.Rarity)i);

                // Select a random upgrade from the filtered list
                if (upgradesOfRarity.Length > 0)
                {
                    Upgrade selectedUpgrade = upgradesOfRarity[Random.Range(0, upgradesOfRarity.Length)];
                    return selectedUpgrade; // return upgrade
                }
                else
                {
                    Debug.LogWarning("No upgrades found for rarity: " + (Upgrade.Rarity)i);
                    return null;
                }
            }
        }

        Debug.LogWarning("Failed to get random upgrade.");
        return null;
    }

}
