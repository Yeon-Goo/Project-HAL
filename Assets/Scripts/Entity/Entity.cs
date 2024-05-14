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
    // Entity's UIs
    public HPBarUI hpbar_prefab;
    public HPBarUI hpbar_ui;

    // Entity의 HP를 관리하는 변수
    public StatManager stat_manager;

    /*
    public void SetHPManager(HPManager hp_manager)
    {
        this.hp_manager = hp_manager;
    }

    public HPManager GetHPManager()
    {
        return hp_manager;
    }
    */

    public StatManager Stat_manager
    {
        get;
        set;
    }

    //
    // 요약:
    //     Entity가 현재 위치하고 있는 좌표를 Vector2로 반환합니다.
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
        float cur_hp = stat_manager.Cur_hp;

        while (true)
        {
            StartCoroutine(FlickEntity());

            // this는 entity로부터 damage만큼의 피해를 interval초마다 받는다
            Debug.Log(this.gameObject + " Get " + damage + " Damage From " + entity.name + "(interval : " + interval + ")\n");
            cur_hp -= damage;
            stat_manager.Cur_hp = cur_hp;

            // this의 체력이 0일 때
            if (cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

            // this의 체력이 0보다 크면 interval만큼 실행을 양보(멈춤)
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

    //
    // 요약:
    //     피격 시 Entity를 0.1초 동안 깜빡입니다.
    public virtual IEnumerator FlickEntity()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public virtual void KillEntity()
    {
        stat_manager.Cur_hp = 0;
        Destroy(gameObject);
    }
    public abstract void ResetEntity();
}
