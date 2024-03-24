using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerEntity : Entity
{
    private static string pickable_Objects = "Pickable_Objects";
    public HPBar hpbar_Prefab;
    private HPBar hpBar;

    void Start()
    {
        cur_HP.value = init_HP;
        hpBar = Instantiate(hpbar_Prefab);
        hpBar.SetPlayer(this);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_Objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if(hitObject != null)
            {
                bool should_disappear = false;

                print("Hit: " + hitObject.GetName());
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
        if (cur_HP.value < max_HP)
        {
            cur_HP.value += amount;
            print("Adjusted HP by : " + amount + ". New value : " + cur_HP.value);

            return true;
        }

        return true;
    }
}
