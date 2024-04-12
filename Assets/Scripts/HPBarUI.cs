using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;

public class HPBarUI : MonoBehaviour
{
    //[HideInInspector]
    private Entity entity;
    private HPManager hp_manager;
    private Image hpbar_meter;

    // PlayerEntity HPBarUI Property
    private TMP_Text hpbar_text;

    // Called by PlayerEntity::Start()
    public void Init(Entity entity)
    {
        if (entity == null)
        {
            Debug.Log("HPBarUI::Init() Error! Entity is NULL");
            return;
        }

        this.entity = entity;
        hp_manager = entity.GetHPManager();
        hpbar_meter = GetComponentsInChildren<Image>()[2];

        switch (entity)
        {
            case PlayerEntity :
                hpbar_text = GetComponentInChildren<TMP_Text>();
                break;
            case EnemyEntity :
                break;
            //case BossEntity:
                //break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpbar_meter.fillAmount = hp_manager.GetCurrentHP() / hp_manager.GetMaxHP();

        if (entity != null)
        {
            switch (entity)
            {
                case PlayerEntity:
                    hpbar_text.text = "HP:" + hp_manager.GetCurrentHP();
                    break;
                case EnemyEntity:
                    break;
                    //case BossEntity:
                    //break;
            }
        }
    }
}
