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
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Enter\n");
            PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();

            if (damage_coroutine == null)
            {
                // 1.0f의 딜레이마다 damage_scale의 피해를 입힌다
                damage_coroutine = StartCoroutine(player.DamageEntity(damage_scale, 1.0f));
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Exit\n");
            if (damage_coroutine != null)
            {
                StopCoroutine(damage_coroutine);
                damage_coroutine = null;
            }
        }
    }

    public override IEnumerator DamageEntity(int damage, float interval)
    {
        float cur_hp = hp_manager.GetCurrentHP();

        while (true)
        {
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

    public override void ResetEntity()
    {
        hp_manager.SetCurrentHP(hp_manager.GetMaxHP());
    }
}