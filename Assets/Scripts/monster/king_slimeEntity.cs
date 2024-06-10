using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class king_slimeEntity : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject rupinPrefab;
    public GameObject BananaPrefab;
    public Transform player;
    public GameObject bossHPUI; // Boss_HP UI 오브젝트 참조
    public Image filledSlider;  // 채워진 슬라이더 이미지
    public Image emptySlider;   // 빈 슬라이더 이미지
    public TextMeshProUGUI hpText;         // HP 텍스트 컴포넌트 참조
    public float speed = 2f;
    public float range = 10f;
    public float barrage_time = 4f;
    public float rush_time = 5f;
    public float summon_time = 3f;
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;
    int barrage_count = 4;
    float r;

    private StatManager stat_manager;

    public static Vector3 AngleToVector(float angleDegrees)
    {
        // 각도를 라디안으로 변환
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // 각도를 사용하여 벡터 계산
        float x = Mathf.Cos(angleRadians);
        float y = Mathf.Sin(angleRadians);

        // 2D 벡터 생성 (Z는 0으로 설정)
        return new Vector3(x, y, 0);
    }

    private Animator animator;
    private static string animationState = "AnimationState";

    enum StateEnum
    {
        idle = 0,
        move = 1,
        barrage = 2,
        rush = 3,
        summon = 4,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        monster_state = (int)StateEnum.idle;
        transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
        animator.SetInteger(animationState, monster_state);

        // Boss_HP UI 오브젝트를 찾아 비활성화
        bossHPUI = GameObject.Find("BossHPBar");
        if (bossHPUI != null)
        {
            bossHPUI.SetActive(false);
        }
    }

    public IEnumerator SetStatManagerCoroutine()
    {
        while (true)
        {
            stat_manager = this.GetComponent<Entity>().stat_manager;
            if (stat_manager == null)
            {
                Debug.Log("StatManager is not yet assigned.");
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                Debug.Log("StatManager assigned successfully.");
                break;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("UpWall"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(210, 330);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("DownWall"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(30, 150);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("LeftWall"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(-30, 30);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("RightWall"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            r = Random.Range(150, 210);
            Vector3 NewDirection = AngleToVector(r);
            GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
        }
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Blocking") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            Vector2 incomingVector = rb.velocity;
            Vector2 normalVector = collision.contacts[0].normal;
            Vector2 reflectVector = Vector2.Reflect(incomingVector, normalVector).normalized;

            float speedReduction = 0.3f;
            float newSpeed = Mathf.Max(0, incomingVector.magnitude - speedReduction);

            rb.velocity = reflectVector * newSpeed;
        }
    }

    void FixedUpdate()
    {
        if (stat_manager != null)
        {
            UpdateHealthBar();
        }
        else
        {
            StartCoroutine(SetStatManagerCoroutine());
        }

        float distance = Vector2.Distance(transform.position, player.position);
        if (monster_state == (int)StateEnum.idle) // 평상시
        {
            if (distance < range) // 거리가 가까워지면 추격상태로 전환
            {
                if (bossHPUI != null)
                {
                    bossHPUI.SetActive(true); // Boss_HP UI 활성화
                }
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.move) // 추격 중
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (Time.time - tempTime > 3.0f)
            {
                int rand = Random.Range(1, 15);
                if (rand < 8)
                {
                    monster_state = (int)StateEnum.barrage;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
                else if (rand < 14)
                {
                    monster_state = (int)StateEnum.rush;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                    r = Random.Range(0, 360);
                    r = 270;
                    Vector3 NewDirection = AngleToVector(r);
                    GetComponent<Rigidbody2D>().AddForce(NewDirection.normalized * 500000);
                }
                else
                {
                    monster_state = (int)StateEnum.summon;
                    transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                    animator.SetInteger(animationState, monster_state);
                    tempTime = Time.time;
                }
            }
        }

        if (monster_state == (int)StateEnum.barrage) // 탄막 피하기
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
            if (Time.time - tempTime >= 0.3f * barrage_count)
            {
                float rand = Random.Range(12, 24);
                for (int i = 0; i < 10; i++)
                {
                    GameObject Banana = Instantiate(BananaPrefab);
                    Banana.transform.position = transform.position;
                    Vector3 NewDirection = AngleToVector(36 * i + 18 * barrage_count + rand);
                    Banana.GetComponent<Rigidbody2D>().AddForce((NewDirection).normalized * 900);
                }
                barrage_count++;
            }
            if (Time.time - tempTime >= barrage_time) // 탄막 지속시간이 지났으면 추격 상태로 전환
            {
                barrage_count = 4;
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, monster_state);
            }
        }

        if (monster_state == (int)StateEnum.rush) // 돌진
        {
            if (Time.time - tempTime > rush_time) // 돌진이 끝나고 추격상태로 전환
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, (int)StateEnum.idle);
            }
        }

        if (monster_state == (int)StateEnum.summon) // 소환
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            if (Time.time - tempTime > summon_time) // 소환 후 추격상태로 전환
            {
                GameObject rupin = Instantiate(rupinPrefab);
                rupin.transform.position = transform.position;
                tempTime = Time.time;
                monster_state = (int)StateEnum.move;
                transform.localScale = new Vector3(4.0f, 4.0f, 1.0f);
                animator.SetInteger(animationState, (int)StateEnum.idle);
            }
        }
    }

    // 체력 바를 업데이트하는 메서드
    public void UpdateHealthBar()
    {
        float fillAmount = stat_manager.Cur_hp / stat_manager.Max_hp;

        // 채워진 슬라이더의 fillAmount를 설정
        filledSlider.fillAmount = fillAmount;

        // HP 텍스트 업데이트
        if (hpText != null)
        {
            hpText.text = $"{stat_manager.Cur_hp} / {stat_manager.Max_hp}";
        }
    }
    public void DestroyHPBarUI()
    {
        // Boss_HP UI 오브젝트를 비활성화
        if (bossHPUI != null)
        {
            bossHPUI.SetActive(false);
        }

        if (filledSlider != null)
        {
            Destroy(filledSlider.gameObject);
        }

        if (emptySlider != null)
        {
            Destroy(emptySlider.gameObject);
        }

        if (hpText != null)
        {
            Destroy(hpText.gameObject);
        }
    }
}