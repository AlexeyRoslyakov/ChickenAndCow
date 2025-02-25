using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    private Image energyBarUI;

    void Start()
    {
        energyBarUI = GetComponent<Image>();
    }

    public void UpdateEnergyBarUI(float maxEnergy, float currentEnergy)
    {
        float segmentsFilled = Mathf.Ceil(currentEnergy / 3);
        float totalSegments = maxEnergy / 3;
        energyBarUI.fillAmount = segmentsFilled / totalSegments;
    }
}