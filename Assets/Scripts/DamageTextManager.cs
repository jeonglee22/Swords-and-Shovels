using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public GameObject textCanvas;
    public GameObject panel;

    private List<GameObject> damageTextPool;
    private int poolIndex;
    private float offset = 2f;

    public void ActiveDamage(int damage, Vector3 position)
    {
        damageTextPool[poolIndex].SetActive(true);
        damageTextPool[poolIndex].transform.position = position + Vector3.up * offset;
        damageTextPool[poolIndex].transform.rotation = panel.transform.rotation;

        damageTextPool[poolIndex].GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
        
        poolIndex++;
        if (poolIndex >= damageTextPool.Count)
            poolIndex = 0;
    }

    private void Start()
    {
        damageTextPool = new List<GameObject>();

        for (int i = 0; i < 30; i++)
        {
            damageTextPool.Add(Instantiate(textCanvas, panel.transform.position, panel.transform.rotation));
            damageTextPool[i].SetActive(false);
        }
        poolIndex = 0;
    }
}
