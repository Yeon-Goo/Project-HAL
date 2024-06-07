using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemyEntity : Entity
{
    private int damage_scale = 1;
    public int arrowstack = 0;
    Coroutine damage_coroutine;



    public Coroutine GetDamageCoroutine()
    {
        return damage_coroutine;
    }

    //private void OnEnable()
    void Start()
    {
        // Load HPManager
        if (this.gameObject.name.Contains("Dummy"))
        {
            StatManager originalStatManager = Resources.Load<StatManager>("ScriptableObjects/DummyHPManager");
            stat_manager = Instantiate(originalStatManager);
        }
        else if (this.gameObject.name.Contains("king_slime"))
        {
            StatManager originalStatManager = Resources.Load<StatManager>("ScriptableObjects/king_slimeHPManager");
            stat_manager = Instantiate(originalStatManager);
        }
        else if (this.gameObject.name.Contains("rupin"))
        {
            StatManager originalStatManager = Resources.Load<StatManager>("ScriptableObjects/rupinHPManager");
            stat_manager = Instantiate(originalStatManager);
        }
        else if (this.gameObject.name.Contains("green_slime"))
        {
            StatManager originalStatManager = Resources.Load<StatManager>("ScriptableObjects/green_slimeHPManager");
            stat_manager = Instantiate(originalStatManager);
        }
        else if (this.gameObject.name.Contains("Green_Slime"))
        {
            StatManager originalStatManager = Resources.Load<StatManager>("ScriptableObjects/Green_SlimeHPManager");
            stat_manager = Instantiate(originalStatManager);
        }
        if (stat_manager == null) return;

        // Load HPBarUI Prefab
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/EnemyHPBarUI");
        if (hpbar_ui == null)
        {
            // Instantiate HPBarUI if it doesn't exist
            hpbar_ui = Instantiate(hpbar_prefab);
            // Set HPBarUI as a child of this GameObject
            hpbar_ui.transform.SetParent(this.transform, false);
        }
        hpbar_ui.Init(this);

        base.Start();
        ResetEntity();
    }

    // 미완성
    public override void ResetEntity()
    {
        stat_manager.Cur_hp = stat_manager.Max_hp;
    }

    void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        // 플레이어가 Enemy의 Collision에 들어감
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerEntity player = collision.gameObject.GetComponent<PlayerEntity>();

                // interval의 딜레이마다 damage_scale의 피해를 입힌다
            StartCoroutine(player.DamageEntity(damage_scale, 1.0f, this.gameObject));
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