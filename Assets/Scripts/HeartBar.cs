using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    Image heartBarUI;
    // Start is called before the first frame update
    void Start()
    {
        heartBarUI= GetComponent<Image>();
    }

    public void UpdateHeartBarUI(float maxLife, float currentLife)
    {
        float lifePercent = currentLife / maxLife;
        heartBarUI.fillAmount = 1f - lifePercent;
    }
}
