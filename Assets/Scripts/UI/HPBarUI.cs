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
    [SerializeField]
    private Image hpbar_meter;
    private Image mpbar_meter;

    // PlayerEntity HPBarUI Property
    private TMP_Text hpbar_text;
    private TMP_Text mpbar_text;

    // EnemyEntity HPBarUI Property
    private float width;
    private float height;
    private Image hpbar_mask;

    // Called by PlayerEntity::Start()
    public void Init(Entity entity)
    {
        if (entity == null)
        {
            Debug.Log("HPBarUI::Init() Error! Entity is NULL");
            return;
        }

        this.entity = entity;
        hp_manager = entity.hp_manager;

        switch (entity)
        {
            case PlayerEntity :
                hpbar_meter = GetComponentsInChildren<Image>()[2];
                mpbar_meter = GetComponentsInChildren<Image>()[5];

                hpbar_text = GetComponentsInChildren<TMP_Text>()[0];
                mpbar_text = GetComponentsInChildren<TMP_Text>()[1];
                break;
            case EnemyEntity :
                Sprite sprite = entity.GetComponent<SpriteRenderer>().sprite;
                width = sprite.rect.width;
                height = sprite.rect.height;
                hpbar_mask = GetComponentsInChildren<Image>()[1];
                hpbar_meter = GetComponentsInChildren<Image>()[2];
                break;
            //case BossEntity:
                //break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        hpbar_meter.fillAmount = hp_manager.Cur_hp / hp_manager.Max_hp;

        if (entity != null)
        {
            switch (entity)
            {
                case PlayerEntity:
                    mpbar_meter.fillAmount = hp_manager.Cur_mp / hp_manager.Max_mp;

                    hpbar_text.text = "HP:" + hp_manager.Cur_hp;
                    mpbar_text.text = "MP:" + hp_manager.Cur_mp;
                    break;
                case EnemyEntity:
                    hpbar_mask.transform.position = Camera.main.WorldToScreenPoint(entity.transform.position) + new UnityEngine.Vector3(width * -0.5f, height * 0.5f + 30.0f, 0);

                    break;
                    //case BossEntity:
                    //break;
            }
        }
    }
}