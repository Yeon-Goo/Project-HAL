using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine.Unity;

public class king_slimeEntity : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameObject rupinPrefab;
    public GameObject BananaPrefab;
    public GameObject BoomPrefab;
    public Transform player;
    public GameObject bossHPUI; // Boss_HP UI 오브젝트 참조
    public Image filledSlider;  // 채워진 슬라이더 이미지
    public Image emptySlider;   // 빈 슬라이더 이미지
    public TextMeshProUGUI hpText; // HP 텍스트 컴포넌트 참조
    public float speed = 2f;
    public float range = 10f;
    public float barrage_time = 4f;
    public float summon_time = 3f;
    private int monster_state = 0;
    private float tempTime = 0f;
    private Vector2 initial_pos;
    private Vector2 wanderDirection;
    private Vector2 move_direction;
    private Vector3 target_pos;
    int barrage_count = 4;
    float r;
    int pattern_rand = 0;
    float rand_barrage = 0;

    private StatManager stat_manager;
    private SkeletonAnimation skeletonAnimation;
    private Coroutine barrageCoroutine;

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

    private static string animationState = "AnimationState";

    enum StateEnum
    {
        idle = 0,
        move = 1,
        barrage = 2,
        rush = 3,
        summon = 4,
        boom = 5,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        monster_state = (int)StateEnum.idle;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        SetAnimation("Idle", true);

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
                yield return null; // 다음 프레임까지 대기
            }
            else
            {
                break;
            }
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

        if (player.position.y > -10f)
        {
            DisableHPBarUI();
            stat_manager.Cur_hp = stat_manager.Max_hp;
            monster_state = (int)StateEnum.idle;
            transform.position = new Vector3(11, -32, 0);
            SetAnimation("Idle", true);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (monster_state == (int)StateEnum.idle) // 평상시
        {
            if (distance < range) // 거리가 가까워지면 추격상태로 전환
            {
                if (bossHPUI != null)
                {
                    bossHPUI.SetActive(true);
                }

                if (filledSlider != null)
                {
                    filledSlider.gameObject.SetActive(true);
                }

                if (emptySlider != null)
                {
                    emptySlider.gameObject.SetActive(true);
                }

                if (hpText != null)
                {
                    hpText.gameObject.SetActive(true);
                }
                monster_state = (int)StateEnum.move;
                UpdateScaleTowardsPlayer(); // 플레이어 방향에 따른 Scale 업데이트
                SetAnimation("Idle", true);
                tempTime = Time.time;
            }
        }

        if (monster_state == (int)StateEnum.move) // 추격 중
        {
            UpdateScaleTowardsPlayer(); // 플레이어 방향에 따른 Scale 업데이트
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if (Time.time - tempTime > 3.0f)
            {
                int rand = Random.Range(0, 16);
                if (rand < 0)
                {
                    pattern_rand = Random.Range(0, 2);
                    rand_barrage = Random.Range(0, 90);
                    monster_state = (int)StateEnum.barrage;
                    SetAnimation("Walk", true);
                    barrageCoroutine = StartCoroutine(BarrageMovement());
                    tempTime = Time.time;
                }
                else if (rand < 0)
                {
                    monster_state = (int)StateEnum.rush;
                    StartCoroutine(RushSequence());
                }
                else if (rand < 16)
                {
                    pattern_rand = Random.Range(0, 2);
                    monster_state = (int)StateEnum.boom;
                    if(pattern_rand == 0)
                    {
                        StartCoroutine(BoomSequence());
                    }
                    else
                    {
                        StartCoroutine(GatlingSequence());
                    }
                }
                else
                {
                    monster_state = (int)StateEnum.summon;
                    SetAnimation("Walk", true);
                    tempTime = Time.time;
                }
            }
        }

        if (monster_state == (int)StateEnum.barrage) // 탄막 피하기
        {
            UpdateScale(); // x 속도에 따른 Scale 업데이트
            if (Time.time - tempTime >= 0.3f * barrage_count && pattern_rand == 0)
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
            if (Time.time - tempTime >= 0.1f * barrage_count && pattern_rand == 1)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameObject Banana = Instantiate(BananaPrefab);
                    Banana.transform.position = transform.position;
                    Vector3 NewDirection = AngleToVector(90 * i + 17 * barrage_count + rand_barrage);
                    Banana.GetComponent<Rigidbody2D>().AddForce((NewDirection).normalized * 900);
                }
                barrage_count++;
            }
            if (Time.time - tempTime >= barrage_time) // 탄막 지속시간이 지났으면 추격 상태로 전환
            {
                barrage_count = 4;
                tempTime = Time.time;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                monster_state = (int)StateEnum.move;
                SetAnimation("Idle", true);
                if (barrageCoroutine != null)
                {
                    StopCoroutine(barrageCoroutine);
                }
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
                SetAnimation("Idle", true);
            }
        }
    }
    private IEnumerator GatlingSequence()
    {
        for (int i = 0; i < 3; i++)
        {
            SetAnimation("Attack", false);
            UpdateScaleTowardsPlayer();
            yield return new WaitForSeconds(0.5f); // 공격 모션 0.5초 대기
            Vector3 poscorrection;
            if (transform.position.x > player.position.x)
            {
                poscorrection = new Vector3(-1.7f, 2.2f, 0f);
            }
            else
            {
                poscorrection = new Vector3(1.7f, 2.2f, 0f);
            }
            for (int j = 0; j < 6; j++)
            {
                GameObject Banana = Instantiate(BananaPrefab);
                Banana.transform.position = transform.position + poscorrection;
                Vector2 direction = (player.position - Banana.transform.position).normalized;
                Banana.GetComponent<Rigidbody2D>().AddForce(direction * 1200);
                yield return new WaitForSeconds(0.1f); // 바나나 발사 간격 0.1초
            }
            UpdateScaleTowardsPlayer();
            yield return new WaitForSeconds(0.2f); // 공격 모션 후 0.5초 대기
        }

        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
        monster_state = (int)StateEnum.move;
        SetAnimation("Idle", true);
        tempTime = Time.time;
    }
    IEnumerator RushSequence()
    {
        int rand = Random.Range(0, 10);
        int n;
        if(rand < 1)
        {
            n = 4;
        }
        else
        {
            n = 3;
        }

        for (int i = 0; i < n; i++)
        {
            SetAnimation("Attack", false);
            yield return new WaitForSeconds(0.25f); // 공격 모션 0.3초 대기

            Vector2 direction = (player.position - transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = direction * 8; // 돌진 시작
            UpdateScale();

            float elapsed = 0f;
            while (elapsed < 1.2f)
            {
                elapsed += Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(direction * 8, Vector2.zero, elapsed / 1.2f); // 점차 느려짐
                yield return null;
            }

            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 완전히 멈춤
            yield return new WaitForSeconds(0.3f); // 다음 공격 모션 전 0.3초 대기
        }
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
        monster_state = (int)StateEnum.move;
        SetAnimation("Idle", true);
        tempTime = Time.time;
    }
    IEnumerator BoomSequence()
    {
        SetAnimation("Attack", false);
        Vector2 startPosition = transform.position;
        Vector2 targetPosition = player.position; // 플레이어의 현재 위치 저장
        yield return new WaitForSeconds(0.5f); // 1초 대기

        GameObject boom = Instantiate(BoomPrefab);
        if (transform.position.x > player.position.x)
        {
            startPosition = startPosition + new Vector2(-1.8f, 2.3f);
        }
        else
        {
            startPosition = startPosition + new Vector2(1.8f, 2.3f);
        }
        
        boom.transform.position = startPosition;

        Vector2 direction = (targetPosition - startPosition).normalized;
        float distance = Vector2.Distance(startPosition, targetPosition);
        float flightDuration = 1f; // 포물선 비행 시간

        float elapsed = 0f;
        while (elapsed < flightDuration)
        {
            float t = elapsed / flightDuration;
            float height = 2.0f * Mathf.Sin(t * Mathf.PI); // 포물선 효과를 위한 높이 계산
            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, t);
            boom.transform.position = new Vector3(currentPosition.x, currentPosition.y + height, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        monster_state = (int)StateEnum.move;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 정지
        SetAnimation("Idle", true);
        tempTime = Time.time;
    }
    void UpdateScale()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
        }
    }

    void UpdateScaleTowardsPlayer()
    {
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
        }
    }
    private IEnumerator BarrageMovement()
    {
        while (monster_state == (int)StateEnum.barrage)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y); // 오른쪽으로 이동
            yield return new WaitForSeconds(0.5f);
            rb.velocity = new Vector2(-speed, rb.velocity.y); // 왼쪽으로 이동
            yield return new WaitForSeconds(0.5f);
        }
        rb.velocity = Vector2.zero; // 멈춤
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

    private void DisableHPBarUI()
    {
        if (bossHPUI != null)
        {
            bossHPUI.SetActive(false);
        }

        if (filledSlider != null)
        {
            filledSlider.gameObject.SetActive(false);
        }

        if (emptySlider != null)
        {
            emptySlider.gameObject.SetActive(false);
        }

        if (hpText != null)
        {
            hpText.gameObject.SetActive(false);
        }
    }

    private void SetAnimation(string animationName, bool loop)
    {
        skeletonAnimation.state.SetAnimation(0, animationName, loop);
    }
}