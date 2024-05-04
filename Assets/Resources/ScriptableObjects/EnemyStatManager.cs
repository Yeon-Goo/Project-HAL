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
}
