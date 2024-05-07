using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    public GameObject upgradeCardPrefab;
    public Transform[] cardPositions; // Positions where the cards will be displayed
    public Upgrade[] upgradesToDisplay; // Array of scriptable objects to display on the cards

    void Start()
    {
        DisplayUpgrades();
    }

    void DisplayUpgrades()
    {
        for (int i = 0; i < Mathf.Min(cardPositions.Length, upgradesToDisplay.Length); i++)
        {
            // Instantiate UpgradeCard prefab
            GameObject card = Instantiate(upgradeCardPrefab, cardPositions[i]);

            // Set upgrade information on the card
            UpgradeCard upgradeCard = card.GetComponent<UpgradeCard>();
            if (upgradeCard != null && upgradesToDisplay[i] != null)
            {
                Upgrade upgrade = upgradesToDisplay[i];
                upgradeCard.SetUpgradeInfo(upgrade);
            }
            else
            {
                Debug.LogWarning("UpgradeCard prefab or upgradesToDisplay is not assigned.");
            }
        }
    }
}
