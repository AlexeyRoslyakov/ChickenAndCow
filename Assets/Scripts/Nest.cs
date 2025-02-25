using System.Collections;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [Header("Egg Spawn Settings")] public GameObject eggPrefab;
    public float eggYOffset = 0.158f;

    [HideInInspector] public GameObject currentEgg;

    private void Start()
    {
        GameManager.Instance.RegisterNest(this);
        if (currentEgg == null && eggPrefab != null)
        {
            SpawnEgg();
        }
    }

    public void ReplenishEgg()
    {
        if (currentEgg == null)
        {
            SpawnEgg();
        }
    }

    private void SpawnEgg()
    {
        Vector3 spawnPosition =
            new Vector3(transform.position.x, transform.position.y + eggYOffset, transform.position.z);
        currentEgg = Instantiate(eggPrefab, spawnPosition, Quaternion.identity);
        currentEgg.transform.SetParent(transform);

        var resourceScript = currentEgg.GetComponent<ResourceInteraction>();
        if (resourceScript != null)
        {
            resourceScript.nest = this;
        }
    }


    public void EggCollected()
    {
        currentEgg = null;
    }
}