using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ArrowObject : MonoBehaviour
{
    private GameObject weapon;
    // 화살 설정
    private int dmg;
    private float armor_de, speed;
    private Vector3 shootvector = Vector3.zero;
    private bool originalarrow = true;
    //  0-스택 충전    1-스택 사용     2-스택 전체 사용   3-마나 회복  
    private int attacktype = 0;
    private int damage_per_stack = 1;

    // 화살 객체 풀링
    private IObjectPool<ArrowObject> _ManagedPool;
    bool is_released = false;

    // 화살이 enemy와 충돌할 경우 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Enemy") || coll.gameObject.CompareTag("Boss_Enemy"))
        {
            //coll.gameObject.GetComponent<EnemyObject>().Damaged(dmg, armor_de);
            EnemyEntity enemy = coll.gameObject.GetComponent<EnemyEntity>();
            Coroutine damage_coroutine = enemy.GetDamageCoroutine();
            if (damage_coroutine == null)
            {
                Vector2 effectPosition = transform.position;
                Vector2 effectScale = new Vector2(3.0f, 3.0f); // 이펙트 크기 설정
                if (attacktype == 0)
                {
                    enemy.arrowstack += 1;
                    damage_coroutine = StartCoroutine(enemy.DamageEntity(dmg, 0.0f, this.gameObject));
                    EffectManager.Instance.PlayEffect("attackanim", effectPosition, effectScale);
                    DestroyArrow();
                    SoundManager.Instance.PlaySound("arrowhitsound");
                }
                else if(attacktype == 1)
                {
                    if (enemy.arrowstack > 0)
                    {
                        enemy.arrowstack -= 1;
                        damage_per_stack = dmg / 2;
                        dmg += damage_per_stack;
                    }
                    damage_coroutine = StartCoroutine(enemy.DamageEntity(dmg, 0.0f, this.gameObject));
                    effectScale = new Vector2(4.5f, 4.5f);
                    EffectManager.Instance.PlayEffect("ani_hitFire_01", effectPosition, effectScale);
                    DestroyArrow();
                    SoundManager.Instance.PlaySound("arrowhitsound2");
                }
                else if(attacktype == 2)
                {
                    damage_per_stack = dmg / 2;
                    dmg += damage_per_stack * enemy.arrowstack;
                    enemy.arrowstack = 0;
                    damage_coroutine = StartCoroutine(enemy.DamageEntity(dmg, 0.0f, this.gameObject));
                    EffectManager.Instance.PlayEffect("ani_slashSpecial2_01", effectPosition, effectScale);
                    SoundManager.Instance.PlaySound("arrowhitsound3");
                }
                else if(attacktype == 3)
                {
                    weapon.GetComponent<Archer>().RecoverMana();
                    damage_coroutine = StartCoroutine(enemy.DamageEntity(dmg, 0.0f, this.gameObject));
                    effectScale = new Vector2(4.5f, 4.5f);
                    EffectManager.Instance.PlayEffect("ani_hitIce_01", effectPosition, effectScale);
                    DestroyArrow();
                    SoundManager.Instance.PlaySound("arrowhitsound");
                }
                
                
                if(originalarrow)
                {
                    weapon.GetComponent<Archer>().FireFromAfterImage(new Vector3(coll.transform.position.x, coll.transform.position.y, 0.0f));
                }
                
            }
        }
    }

    // 화살이 발사되는 순간에 호출되는 메서드
    public void Shoot(Vector3 dir, float angleadd)
    {
        is_released = false;
        shootvector = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleadd;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 화살의 회전 각도 조정
        StopAllCoroutines();
        StartCoroutine(DestroyArrowAfterTime(0.5f)); // 일정 시간 후 자동 파괴
    }

    private IEnumerator DestroyArrowAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyArrow();
    }

    void Update()
    {
        // 화살이 회전한 축의 y 방향으로 시간*속도 만큼 이동함
        transform.Translate(new Vector3(0.0f, 1.0f, 0.0f) * Time.deltaTime * speed);
    }

    void Awake()
    {
        weapon = GameObject.Find("Archer");
    }

    public void SetArrowData(int damage, int dstack, float decrease, float shootspeed, int arrowtype, bool realarrow)
    {
        dmg = damage;
        damage_per_stack = dstack;
        armor_de = decrease;
        speed = shootspeed;
        attacktype = arrowtype;
        originalarrow = realarrow;
    }


    //------------------- DONT EDIT ------------------------
    public void SetManagedPool(IObjectPool<ArrowObject> pool)
    {
        _ManagedPool = pool;
    }

    public void DestroyArrow()
    {
        if (!is_released)
        {
            _ManagedPool.Release(this);
            is_released = true;
        }
    }
}
