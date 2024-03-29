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
    // Entity�� HP�� �����ϴ� ����
    public HPManager hp_manager;

    public void SetHPManager(HPManager hp_manager)
    {
        this.hp_manager = hp_manager;
    }

    public HPManager GetHPManager()
    {
        return hp_manager;
    }
}
