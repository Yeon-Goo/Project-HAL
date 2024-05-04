using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/EnemyStatManager", fileName = "EnemyStatManager")]

//
// 요약:
//    Player의 Stat를 관리하는 StatManager입니다.
public class EnemyStatManager : ScriptableObject
{
    //
    // 요약:
    //     Enemy의 현재 레벨입니다.
    private int level;
    //
    // 요약:
    //     Enemy를 처치했을 때 얻을 수 있는 경험치입니다.
    private int exp;
    //
    // 요약:
    //     Enemy의 최대 체력입니다.
    private float max_hp;
    //
    // 요약:
    //     Enemy의 체력입니다.
    private float hp;
    //
    // 요약:
    //     Enemy의 최대 마나입니다.
    private float max_mana;
    //
    // 요약:
    //     Enemy의 마나입니다.
    private float mana;
    //
    // 요약:
    //     Enemy의 공격력입니다.
    private float attack_damage;
    //
    // 요약:
    //     Enemy의 주문력입니다.
    private float ability_power;
    //
    // 요약:
    //     Enemy의 방어력입니다.
    private float armor;
    //
    // 요약:
    //     Enemy의 마법 방어력입니다.
    private float magic_registance;
    //
    // 요약:
    //     Enemy가 다른 Entity를 발견할 수 있는 사거리입니다.
    private int range;
    //
    // 요약:
    //     Enemy의 이동 속도입니다. 1초에 movement_speed의 거리를 이동합니다.
    private float movement_speed;


    //
    // 요약:
    //     Enemy의 레벨을 반환하거나 설정합니다.
    public int Level
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy를 처치했을 때 얻을 수 있는 경험치를 반환하거나 설정합니다.
    public int Exp
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 최대 체력을 반환하거나 설정합니다.
    public float Max_hp
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 체력을 반환하거나 설정합니다.
    public float Hp
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 최대 마나를 반환하거나 설정합니다.
    public float Max_mana
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 마나를 반환하거나 설정합니다.
    public float Mana
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 공격력을 반환하거나 설정합니다.
    public float Attack_damage
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 주문력을 반환하거나 설정합니다.
    public float Ability_power
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 방어력을 반환하거나 설정합니다.
    public float Armor
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 마법 방어력을 반환하거나 설정합니다.
    public float Magic_registance
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 사거리를 반환하거나 설정합니다.
    public int Range
    {
        get;
        set;
    }
    //
    // 요약:
    //     Enemy의 이동 속도를 반환하거나 설정합니다.
    public float Movement_speed
    {
        get;
        set;
    }


    //
    // 요약:
    //     Enemy의 Stat을 초기화합니다.
    public void Init()
    {
        level;
    //
    // 요약:
    //     Enemy를 처치했을 때 얻을 수 있는 경험치입니다.
    private int exp;
    //
    // 요약:
    //     Enemy의 최대 체력입니다.
    private float max_hp;
    //
    // 요약:
    //     Enemy의 체력입니다.
    private float hp;
    //
    // 요약:
    //     Enemy의 최대 마나입니다.
    private float max_mana;
    //
    // 요약:
    //     Enemy의 마나입니다.
    private float mana;
    //
    // 요약:
    //     Enemy의 공격력입니다.
    private float attack_damage;
    //
    // 요약:
    //     Enemy의 주문력입니다.
    private float ability_power;
    //
    // 요약:
    //     Enemy의 방어력입니다.
    private float armor;
    //
    // 요약:
    //     Enemy의 마법 방어력입니다.
    private float magic_registance;
    //
    // 요약:
    //     Enemy가 다른 Entity를 발견할 수 있는 사거리입니다.
    private int range;
    //
    // 요약:
    //     Enemy의 이동 속도입니다. 1초에 movement_speed의 거리를 이동합니다.
    private float movement_speed;
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
