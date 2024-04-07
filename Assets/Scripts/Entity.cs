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

    public Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public virtual IEnumerator DamageEntity(int damage, float interval, GameObject entity)
    {
        float cur_hp = hp_manager.GetCurrentHP();

        while (true)
        {
            // 플레이어는 entity로부터 damage만큼의 피해를 interval초마다 받는다
            Debug.Log("Player Get " + damage + " Damage From " + entity.name + "(interval : " + interval + ")\n");
            cur_hp -= damage;
            hp_manager.SetCurrentHP(cur_hp);

            // 플레이어의 체력이 0일 때
            if (cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

            // 플레이어의 체력이 0보다 크면 interval만큼 실행을 양보(멈춤)
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public virtual void KillEntity()
    {
        hp_manager.SetCurrentHP(0);
        Destroy(gameObject);
    }
    public abstract void ResetEntity();
}
