using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Unity.VisualScripting;

public class HPBarUI : MonoBehaviour
{
    //[HideInInspector]
    private PlayerEntity player;
    private HPManager hp_manager;
    private Image hpbar_meter;
    private TMP_Text hpbar_text;

    // Called by PlayerEntity::Start()
    public void Init(PlayerEntity player)
    {
        if (player == null)
        {
            Debug.Log("HPBarUI::Init() Error! Player is NULL");
            return;
        }

        this.player = player;
        hp_manager = player.GetHPManager();
        hpbar_meter = GetComponentsInChildren<Image>()[2];
        hpbar_text = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            hpbar_meter.fillAmount = hp_manager.GetCurrentHP() / hp_manager.GetMaxHP();
            hpbar_text.text = "HP:" + hp_manager.GetCurrentHP();
        }
    }

    
}
