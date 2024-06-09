using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossHPText : MonoBehaviour
{
    public TextMeshProUGUI hpText; // Reference to the Text component

    // Start is called before the first frame update
    void Start()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }

    // Method to update the HP text
    public void UpdateHPText(float currentHP, float maxHP)
    {
        hpText.text = currentHP.ToString() + " / " + maxHP.ToString();
    }
}