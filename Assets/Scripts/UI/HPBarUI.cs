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
    private StatManager stat_manager;
    [SerializeField]
    private Image hpbar_meter;

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
        stat_manager = entity.stat_manager;

        switch (entity)
        {
            case EnemyEntity :
                MeshRenderer meshRenderer = entity.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    Bounds bounds = meshRenderer.bounds;
                    width = bounds.size.x;
                    height = bounds.size.y;
                }
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
        hpbar_meter.fillAmount = stat_manager.Cur_hp / stat_manager.Max_hp;

        if (entity != null)
        {
            switch (entity)
            {
                case EnemyEntity:
                    if (entity.CompareTag("Boss_Enemy"))
                    {
                        hpbar_mask.transform.position = Camera.main.WorldToScreenPoint(entity.transform.position) + new UnityEngine.Vector3(-40f, 230f, 0);
                    }
                    else
                    {
                        hpbar_mask.transform.position = Camera.main.WorldToScreenPoint(entity.transform.position) + new UnityEngine.Vector3(-40f, 100f, 0);
                    }
                    break;
                    //case BossEntity:
                    //break;
            }
        }
    }
}