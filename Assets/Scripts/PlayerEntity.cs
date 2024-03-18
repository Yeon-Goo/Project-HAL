using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerEntity : Entity
{
    private static string pickable_objects = "Pickable_Objects";
    public HPBarUI hpbar_prefab;
    public HPBarUI hpbar_ui;

    void Start()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/PlayerHPManager");
        if (hp_manager == null)
        {
#if DEBUG
            Debug.Log("PlayerEntity::Start() Error! HPManager is NULL");
#endif
            return;
        }

        // Load HPBarUI
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/HPBarUI");
        if (hpbar_prefab == null)
        {
#if DEBUG
            Debug.Log("PlayerEntity::Start() Error! HPBarUI Prefab is NULL");
#endif
            return;
        }

        hpbar_ui = Instantiate(hpbar_prefab);
        if (hpbar_ui == null)
        {
#if DEBUG
            Debug.Log("PlayerEntity::Start() Error! HPBarUI is NULL");
#endif
            return;
        }
        hpbar_ui.Init(this);      
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if(hitObject != null)
            {
                bool should_disappear = false;

                //print("Hit: " + hitObject.GetName());
                switch (hitObject.GetItemType())
                {
                    case Item.ItemTypeEnum.COIN:
                        should_disappear = true;
                        break;
                    case Item.ItemTypeEnum.HEALTH:
                        should_disappear = AdjustHP(hitObject.GetQuantity());
                        break;
                }

                if (should_disappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool AdjustHP(int amount)
    {
        if (hp_manager.GetCurrentHP() < hp_manager.GetMaxHP())
        {
            hp_manager.SetCurrentHP(hp_manager.GetCurrentHP() + amount);
            //print("Adjusted HP by : " + amount + ". New value : " + hp_manager.cur_hp);

            return true;
        }

        return true;
    }
}
