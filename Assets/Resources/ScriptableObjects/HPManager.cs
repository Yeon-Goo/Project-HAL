using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Scriptable Objects/HPManager", fileName = "HPManager")]
public class HPManager : ScriptableObject
{
    public float cur_hp;
    public float max_hp;

    public void SetCurrentHP(float value)
    {
        cur_hp = value;
    }

    public void SetMaxHP(float value)
    {
        max_hp = value;
    }

    public float GetCurrentHP()
    {
        return cur_hp;
    }

    public float GetMaxHP()
    {
        return max_hp;
    }
}
