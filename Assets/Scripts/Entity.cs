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

    public Vector2 GetPos()
    {
        return new Vector2(transform.position.x, transform.position.y);
    }

    public abstract IEnumerator DamageEntity(int damage, float interval, Entity entity);
    public virtual void KillEntity()
    {
        hp_manager.SetCurrentHP(0);
        Destroy(gameObject);
    }
    public abstract void ResetEntity();
}
