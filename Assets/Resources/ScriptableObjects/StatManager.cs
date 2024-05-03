using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Tilemaps.Tile;

[CreateAssetMenu(menuName = "Scriptable Objects/StatManager", fileName = "StatManager")]

// Entity의 Stat를 관리하는 StatManager
public class StatManager : ScriptableObject
{
    //
    // 요약:
    //     크리티컬 공격 여부를 결정합니다.
    [SerializeField]
    private int level;
    {
        get
        {
            return gameObject.tag;
        }
        set
        {
            gameObject.tag = value;
        }
    }

    max_level
    exp

    hp
    hp_regen
    max_hp
    mana
    mana_regen
    max_mana

    attack_damage
    ability_power
    attack_speed
    armor
    magic_registance
    critical_strike_chance
    critical_strike_damage

    movement_speed

    range
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

    //
    // 요약:
    //     Sprite to be rendered at the Tile.
    public Sprite sprite
    {
        get
        {
            return m_Sprite;
        }
        set
        {
            m_Sprite = value;
        }
    }

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
