using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowObject : MonoBehaviour
{
    // 화살 설정
    private int dmg;
    private float armor_de, speed;
    private Vector3 shootvector = Vector3.zero;

    // 화살 객체 풀링
    private IObjectPool<ArrowObject> _ManagedPool;
    bool is_released = false;

    // 화살이 enemy와 충돌할 경우 호출되는 메서드
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "enemy")
        {
            //coll.gameObject.GetComponent<EnemyObject>().Damaged(dmg, armor_de);
            DestroyArrow();
        }
    }

    // 화살이 발사되는 순간에 호출되는 메서드
    public void Shoot(Vector3 dir)
    {
        is_released = false;
        shootvector = dir;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90); // 화살의 회전 각도 조정
        //StopAllCoroutines();
        //StartCoroutine(DestroyArrowAfterTime(4.0f)); // 일정 시간 후 자동 파괴
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

    public void SetArrowData(int damage, float decrease, float shootspeed)
    {
        dmg = damage;
        armor_de = decrease;
        speed = shootspeed;
    }

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
