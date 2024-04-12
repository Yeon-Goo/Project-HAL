using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Scriptable Objects/HPManager", fileName = "HPManager")]

// Entity의 HP를 관리하는 HPManager
public class HPManager : ScriptableObject
{
    // Entity의 최대 체력
    [SerializeField]
    private float max_hp;
    // Entity의 현재 체력
    [SerializeField]
    private float cur_hp;
    // Entity의 최대 마나
    [SerializeField]
    private float max_mp;
    // Entity의 현재 마나
    [SerializeField]
    private float cur_mp;
    
    

    public void SetCurrentHP(float value)
    {
        cur_hp = value;
    }

    public void SetCurrentMP(float value)
    {
        cur_mp = value;
    }

    public void SetMaxHP(float value)
    {
        max_hp = value;
    }

    public void SetMaxMP(float value)
    {
        max_mp = value;
    }

    public float GetCurrentHP()
    {
        return cur_hp;
    }

    public float GetCurrentMP()
    {
        return cur_mp;
    }

    public float GetMaxHP()
    {
        return max_hp;
    }

    public float GetMaxMP()
    {
        return max_mp;
    }

    public bool AdjustHP(int amount)
    {
        if (this.cur_hp < this.max_hp)
        {
            this.cur_hp = this.cur_hp + amount;
            //print("Adjusted HP by : " + amount + ". New value : " + hp_manager.cur_hp);

            return true;
        }

        return true;
    }

    public bool AdjustMP(int amount)
    {
        if (this.cur_mp < this.max_mp)
        {
            this.cur_mp = this.cur_mp + amount;
            //print("Adjusted MP by : " + amount + ". New value : " + hp_manager.cur_hp);

            return true;
        }

        return true;
    }
}
