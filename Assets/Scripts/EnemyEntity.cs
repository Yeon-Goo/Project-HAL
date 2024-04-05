using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyEntity : Entity
{
    private int damage_scale = 1;
    Coroutine damage_coroutine;

    private void OnEnable()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/DummyHPManager");
        if (hp_manager == null) return;

        ResetEntity();
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // 플레이어가 Enemy의 Collision에 들어감
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();

            if (damage_coroutine == null)
            {
                // 1.0f의 딜레이마다 damage_scale의 피해를 입힌다
                damage_coroutine = StartCoroutine(player.DamageEntity(damage_scale, 1.0f, this));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 플레이어가 Enemy의 Collision에서 벗어남
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damage_coroutine != null)
            {
                // 대미지를 더 이상 받지 않음
                StopCoroutine(damage_coroutine);
                damage_coroutine = null;
            }
        }
    }

    // Enemy가 대미지를 받는 함수
    public override IEnumerator DamageEntity(int damage, float interval, Entity entity)
    {
        float cur_hp = hp_manager.GetCurrentHP();

        while (true)
        {
            // Enemy는 entity로부터 damage만큼의 피해를 interval초마다 받는다
            cur_hp -= damage;
            hp_manager.SetCurrentHP(cur_hp);

            if (cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

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

    // 미완성
    public override void ResetEntity()
    {
        hp_manager.SetCurrentHP(hp_manager.GetMaxHP());
    }
}