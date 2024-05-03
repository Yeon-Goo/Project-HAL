using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Tilemaps.Tile;

[CreateAssetMenu(menuName = "Scriptable Objects/StatManager", fileName = "StatManager")]

//
// 요약:
//    Player의 Stat를 관리하는 StatManager입니다.
public class StatManager : ScriptableObject
{
    //
    // 요약:
    //     Player의 최대 레벨입니다.
    private const int max_level = 30;
    //
    // 요약:
    //     Player의 현재 레벨입니다.
    private int level;
    //
    // 요약:
    //     Player가 레벨 업하기 위해 필요로 하는 경험치입니다. max_exp를 초과하는 exp는 다음 레벨로 넘어갑니다.
    private int max_exp;
    //
    // 요약:
    //     Player의 경험치입니다.
    private int exp;
    //
    // 요약:
    //     Player가 가질 수 있는 최대 마나입니다.
    private float max_hp;
    //
    // 요약:
    //     Player의 체력입니다.
    private float hp;
    //
    // 요약:
    //     Player의 체력 재생입니다. 1초에 hp_regen의 체력을 회복합니다.
    private float hp_regen;
    //
    // 요약:
    //     Player가 가질 수 있는 최대 마나입니다.
    private float max_mana;
    //
    // 요약:
    //     Player의 마나입니다.
    private float mana;
    //
    // 요약:
    //     Player의 마나 재생입니다. 1초에 mana_regen의 체력을 회복합니다.
    private float mana_regen;
    
    //
    // 요약:
    //     Player의 공격력입니다.
    private float attack_damage;
    //
    // 요약:
    //     Player의 주문력입니다.
    private float ability_power;
    //
    // 요약:
    //     Player의 공격 속도입니다.
    private float attack_speed;
    //
    // 요약:
    //     Player의 치명타 확률입니다.
    private float critical_strike_chance;
    //
    // 요약:
    //     Player의 치명타 피해량입니다. (1 + critical_strike_damage)의 피해량을 입힙니다.
    private float critical_strike_damage;
    //
    // 요약:
    //     Player의 방어력입니다.
    private float armor;
    //
    // 요약:
    //     Player의 마법 방어력입니다.
    private float magic_registance;
    //
    // 요약:
    //     Player의 사거리입니다.
    private int range;
    //
    // 요약:
    //     Player의 이동 속도입니다. 1초에 movement_speed의 거리를 이동합니다.
    private float movement_speed;


    //
    // 요약:
    //     Player의 최대 레벨을 반환하거나 설정합니다. 이 값은 변경할 수 없습니다.
    public int Max_level
    {
        get;
    }
    //
    // 요약:
    //     Player의 레벨을 반환하거나 설정합니다. 이 값은 Level_UP()에 의해서만 변경할 수 있습니다.
    public int Level
    {
        get;
    }
    //
    // 요약:
    //     Player의 최대 경험치를 반환하거나 설정합니다. 이 값은 Level_UP()에 의해서만 변경할 수 있습니다.
    public int Max_exp
    {
        get;
    }
    //
    // 요약:
    //     Player의 경험치를 반환하거나 설정합니다.
    public int Exp
    {
        get
        {
            return exp;
        }
        set
        {
            int new_exp = exp + value;
            int max_exp = this.max_exp;
            
            if (new_exp < max_exp)
            {
                exp = new_exp;
            }
            else
            {
                if (level < max_level)
                {
                    Level_UP(new_exp - max_exp);
                }
            }
        }
    }
    //
    // 요약:
    //     Player의 최대 체력을 반환하거나 설정합니다.
    public float Max_hp
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 체력을 반환하거나 설정합니다.
    public float Hp
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 체력 재생을 반환하거나 설정합니다.
    public float Hp_regen
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 최대 마나를반환하거나 설정합니다.
    public float Max_mana
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 마나를 반환하거나 설정합니다.
    public float Mana
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 마나 재생을 반환하거나 설정합니다.
    public float Mana_regen
    {
        get;
        set;
    }
    
    //
    // 요약:
    //     Player의 공격력을 반환하거나 설정합니다.
    public float Attack_damage
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 주문력을 반환하거나 설정합니다.
    public float Ability_power
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 공격 속도를 반환하거나 설정합니다.
    public float Attack_speed
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 치명타 확률을 반환하거나 설정합니다.
    public float Critical_strike_chance
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 치명타 피해량을 반환하거나 설정합니다.
    public float Critical_strike_damage
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 방어력을 반환하거나 설정합니다.
    public float Armor
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 마법 방어력을 반환하거나 설정합니다.
    public float Magic_registance
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 사거리를 반환하거나 설정합니다.
    public int Range
    {
        get;
        set;
    }
    //
    // 요약:
    //     Player의 이동 속도를 반환하거나 설정합니다.
    public float Movement_speed
    {
        get;
        set;
    }


    //
    // 요약:
    //     Player의 Stat을 초기화합니다.
    public void Init()
    {
        this.level = 1;
        this.max_exp = 180 + level * 100;
        this.exp = 0;
        this.max_hp = 640f;
        this.hp = max_hp;
        this.hp_regen = 3.5f;
        this.max_mana = 280f;
        this.mana = max_mana;
        this.mana_regen = 6.97f;
        this.attack_damage = 59f;
        this.ability_power = 0f;
        this.attack_speed = 0.658f;
        this.critical_strike_chance = 0f;
        this.critical_strike_damage = 0.75f;
        this.armor = 26f;
        this.magic_registance = 30f;
        this.range = 600;
        this.movement_speed = 325f;
    }
    //
    // 요약:
    //     Level이 1 올라갈 때 Player의 stat을 재조정합니다.
    public void Level_UP(int left_exp)
    {
        int level_delta = 1;
        //int max_exp_delta = ;
        //int exp_delta = ;
        float max_hp_delta = 101f;
        //float hp_delta = ;
        float hp_regen_delta = 0.55f;
        float max_mana_delta = 35f;
        //float mana_delta = ;
        float mana_regen_delta = 0.65f;
        float attack_damage_delta = 2.96f;
        //float ability_power_delta = ;
        float attack_speed_delta = 0.0333f;
        //float critical_strike_chance_delta = ;
        //float critical_strike_damage_delta = ;
        float armor_delta = 4.6f;
        float magic_registance_delta = 1.3f;
        //int range_delta = ;
        //float movement_speed_delta = ;

        this.level += level_delta;
        this.max_exp = 180 + level * 100;
        this.exp = left_exp;
        this.max_hp += max_hp_delta;
        this.hp += max_hp_delta;
        this.hp_regen += hp_regen_delta;
        this.max_mana += max_mana_delta;
        this.mana += max_mana_delta;
        this.mana_regen += mana_regen_delta;
        this.attack_damage += attack_damage_delta;
        //this.ability_power += ;
        this.attack_speed += attack_speed_delta;
        //this.critical_strike_chance += ;
        //this.critical_strike_damage += ;
        this.armor += armor_delta;
        this.magic_registance += magic_registance_delta;
        //this.range += ;
        //this.movement_speed = ;
    }
}
