using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyEntity : Entity
{
    private int damage_scale = 1;
    Coroutine damage_coroutine;

    public Coroutine GetDamageCoroutine()
    {
        return damage_coroutine;
    }

    //private void OnEnable()
    void Start()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/DummyHPManager");
        if (hp_manager == null) return;

        // Load HPBarUI Prefab
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/EnemyHPBarUI");
        if (hpbar_prefab == null) return;
        // Instantiate HPBarUI
        hpbar_ui = Instantiate(hpbar_prefab);
        // HPBarUI를 this의 자식으로 생성
        hpbar_ui.transform.SetParent(this.transform, false);

        if (hpbar_ui == null) return;
        hpbar_ui.Init(this);

        ResetEntity();
    }

    // 미완성
    public override void ResetEntity()
    {
        hp_manager.SetCurrentHP(hp_manager.GetMaxHP());
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // 플레이어가 Enemy의 Collision에 들어감
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();

            if (damage_coroutine == null)
            {
                // interval의 딜레이마다 damage_scale의 피해를 입힌다
                damage_coroutine = StartCoroutine(player.DamageEntity(damage_scale, 1.0f, this.gameObject));
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
}