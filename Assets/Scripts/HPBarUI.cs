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
    private Image mpbar_meter;

    // PlayerEntity HPBarUI Property
    private TMP_Text hpbar_text;
    private TMP_Text mpbar_text;

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

        switch (entity)
        {
            case PlayerEntity :
                hpbar_meter = GetComponentsInChildren<Image>()[2];
                mpbar_meter = GetComponentsInChildren<Image>()[5];

                hpbar_text = GetComponentsInChildren<TMP_Text>()[0];
                mpbar_text = GetComponentsInChildren<TMP_Text>()[1];
                break;
            case EnemyEntity :
                hpbar_meter = GetComponentsInChildren<Image>()[1];
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
                    mpbar_meter.fillAmount = hp_manager.GetCurrentMP() / hp_manager.GetMaxMP();

                    hpbar_text.text = "HP:" + hp_manager.GetCurrentHP();
                    mpbar_text.text = "MP:" + hp_manager.GetCurrentMP();
                    break;
                case EnemyEntity:
                    break;
                    //case BossEntity:
                    //break;
            }
        }
    }
}
