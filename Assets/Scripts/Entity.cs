using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Entity → PlayerEntity
 *        → EnemyEntity → BossEntity
 *        → Living Entity
 */
public abstract class Entity : MonoBehaviour
{
    // Entity의 HP를 관리하는 변수
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
