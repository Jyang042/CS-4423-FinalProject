using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCard : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Button purchaseButton;

    private Upgrade upgrade;

    // Method to set the upgrade information on the card
    public void SetUpgradeInfo(Upgrade upgrade)
{
    if (upgrade == null)
    {
        Debug.LogError("Upgrade is null.");
        return;
    }

    nameText.text = upgrade.upgradeName;
    descriptionText.text = upgrade.description;
    costText.text = "Cost: " + upgrade.cost.ToString();
}

}
