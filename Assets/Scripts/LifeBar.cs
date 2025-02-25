using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    Rigidbody2D rigidbody2;
    Image lifeBarUI;
    float maxLife, currentLife;

    // Start is called before the first frame update
    void Start()
    {
        lifeBarUI = GetComponent<Image>();
    }



    public void UpdateLifeBarUI(float maxLife, float currentLife)
    {
        float lifePercent = currentLife / maxLife;
        lifeBarUI.fillAmount = lifePercent;
    }
}
