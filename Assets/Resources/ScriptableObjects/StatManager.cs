using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Scriptable Objects/StatManager", fileName = "StatManager")]

// Entity의 Stat를 관리하는 StatManager
public class StatManager : ScriptableObject
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

    // 요약:Player의 마나 재생입니다. 1초에 mana_regen의 체력을 회복합니다.
    private float mana_regen;

    // 요약: Player의 방어력입니다.
    private float armor;

    // 요약: Player의 이동 속도입니다. 1초에 movement_speed의 거리를 이동합니다.
    private float movement_speed;

    public float Max_hp
    {
        get { return max_hp; }
        set { max_hp = value; }
    }
    public float Cur_hp
    {
        get { return cur_hp; }
        set { cur_hp = value; }
    }
    public float Max_mp
    {
        get { return max_mp; }
        set { max_mp = value; }
    }
    public float Cur_mp
    {
        get { return cur_mp; }
        set { cur_mp = value; }
    }

    // 요약: Player의 마나 재생을 반환하거나 설정합니다.
    public float Mana_regen
    {
        get;
        set;
    }
    // 요약: Player의 방어력을 반환하거나 설정합니다.
    public float Armor
    {
        get;
        set;
    }
    // 요약: Player의 이동 속도를 반환하거나 설정합니다.
    public float Movement_speed
    {
        get;
        set;
    }



    public bool AdjustHP(int amount)
    {
        this.cur_hp += amount;
        if (this.cur_hp >= this.max_hp)
        {
            this.cur_hp = this.max_hp;
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

        return false;
    }
}
