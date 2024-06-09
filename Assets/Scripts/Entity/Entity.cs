using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Entity : MonoBehaviour
{
    // Entity's UIs
    public HPBarUI hpbar_prefab;
    public HPBarUI hpbar_ui;
    public GameObject damageTextPrefab;
    public GameObject stackTextPrefab; // StackText 프리팹
    public Canvas canvas;
    public event Action OnDeath;

    // 현재 생성된 StackText 오브젝트를 저장할 변수
    private GameObject currentStackTextObj;

    // Entity의 HP를 관리하는 변수
    public StatManager stat_manager;

    public StatManager Stat_manager
    {
        get;
        set;
    }

    public void Start()
    {
        // "DamageTextUI" 이름의 오브젝트를 찾아 canvas에 할당
        GameObject canvasObject = GameObject.Find("DamageTextUI");
        if (canvasObject != null)
        {
            canvas = canvasObject.GetComponent<Canvas>();
        }
        else
        {
            Debug.LogError("DamageTextUI라는 이름의 오브젝트를 찾을 수 없습니다. 하이어라키에 DamageTextUI를 추가하세요.");
        }
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
        float cur_hp = stat_manager.Cur_hp;
        GameObject damageTextObj = Instantiate(damageTextPrefab, canvas.transform);
        DamageText damageText = damageTextObj.GetComponent<DamageText>();
        damageText.Setup(transform.position, damage);

        while (true)
        {
            StartCoroutine(FlickEntity());

            // DamageText 생성 및 정보 전달
            
            if (this is EnemyEntity enemy)
            {
                int arrowstack = enemy.arrowstack;

                // 현재 생성된 StackText 오브젝트가 있으면 삭제
                if (currentStackTextObj != null)
                {
                    Destroy(currentStackTextObj);
                }

                // 새로운 StackText 생성 및 정보 전달
                currentStackTextObj = Instantiate(stackTextPrefab, canvas.transform);
                StackText stackText = currentStackTextObj.GetComponent<StackText>();
                stackText.Setup(transform, arrowstack);
            }

            // this는 entity로부터 damage만큼의 피해를 interval초마다 받는다
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

    public virtual IEnumerator FlickEntity()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            meshRenderer.material.color = Color.white;
        }
        else
        {
            Debug.LogError("MeshRenderer를 찾을 수 없습니다. MeshRenderer를 추가하세요.");
        }
    }

    public virtual void KillEntity()
    {
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
        stat_manager.Cur_hp = 0;

        // 현재 생성된 StackText 오브젝트가 있으면 삭제
        if (currentStackTextObj != null)
        {
            Destroy(currentStackTextObj);
        }

        Destroy(gameObject);
    }

    public abstract void ResetEntity();
}