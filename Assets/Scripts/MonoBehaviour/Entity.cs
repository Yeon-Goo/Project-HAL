using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Entity �� PlayerEntity
 *        �� EnemyEntity �� BossEntity
 *        �� Living Entity
 */
public abstract class Entity : MonoBehaviour
{
    // ���ʷ� ������ HP
    public float init_HP;

    // Current HP
    public HP cur_HP;
    // Max HP
    public float max_HP;

}
