using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TradingRow : MonoBehaviour
{
    [Header("UI Elements")] public Image resourceIcon;
    public TextMeshProUGUI resourceName;
    public TextMeshProUGUI resourceQuantity;
    public TextMeshProUGUI resourcePrice;
    public TextMeshProUGUI resourceProduct;
    public ResourceData AssociatedResource { get; private set; }

    public void SetupRow(ResourceData resource)
    {
        AssociatedResource = resource;
        resourceName.text = resource.name;
        resourceQuantity.text = resource.quantity.ToString();
        resourcePrice.text = resource.price.ToString();
        UpdateProductValue();
    }

    public int CalculateRowTotal()
    {
        return AssociatedResource.quantity * AssociatedResource.price;
    }

    private void UpdateProductValue()
    {
        int total = CalculateRowTotal();
        resourceProduct.text = total.ToString();
    }
}