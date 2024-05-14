using UnityEngine;
using UnityEngine.Pool;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class Archer : Weapon
{
    [SerializeField]
    private GameObject arrowPrefab;
    private GameObject playerObject;
    private float arrowSpeed = 25.0f;
    private IObjectPool<ArrowObject> _Pool;
    private float baseAttackCooltime = 0.5f;
    [SerializeField]
    private bool isCharging = false;
    private Coroutine chargingCoroutine;

    

    int[] skillMana = new int[] {1, 4, 8, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private Vector2 mouseWorldPosition;

    public override int GetMana(int cardnum)
    {
        return skillMana[cardnum];
    }

    public override void BaseAttack()
    {
        //if (playerEntity.is_alive && (playerEntity.is_animation_started ^ playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        if (playerEntity.is_alive && !(playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        {
            StartCoroutine(BaseAttackCoroutine());
        }
    }

    private IEnumerator BaseAttackCoroutine()
    {
        playerEntity.PlayAnimation("Attack");
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        yield return new WaitForSeconds(baseAttackCooltime); // 0.5초 대기 / 공속에 비례하게 줄어듬

        BaseShot(0.0f, 0, 1);
    }



    //기본 공격이 가능한지 반환
    public override bool BaseAttackAble()
    {
        if (Time.time > lastUseTime + baseAttackCooltime)
        {
            lastUseTime = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }
    


    private void BaseShot(float angleadd, int type, int damage)
    {
        if (arrowPrefab != null && _Pool != null)
        {
            playerEntity.CharacterStop();
            // 화살 발사 방향 계산 (Vector2 to Vector3 변환)
            Vector3 targetVec = mouseWorldPosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);
            targetVec.z = 0; // Vector3 변환 시 z 축 값은 0으로 설정

            // 오브젝트 풀에서 화살 객체 가져오기
            ArrowObject arrow = _Pool.Get();

            // 화살 위치 설정
            arrow.transform.position = playerObject.transform.position; // Archer의 현재 위치를 기준으로 설정
            arrow.transform.rotation = Quaternion.identity; // 초기 회전 상태를 기본 값으로 설정
            arrow.transform.position += new Vector3(0.0f, 0.25f, 0.0f);

            // 필요한 경우, 화살의 데이터 설정 메서드 호출
            arrow.SetArrowData(damage, 0, arrowSpeed, type); // 화살에 대한 추가 정보 설정

            // 화살 발사 (Shoot 메서드 내부에서 화살의 방향과 속도 처리)
            arrow.Shoot(targetVec.normalized, angleadd); // Shoot 메서드가 방향 벡터를 받아 처리하도록 가정

        }
        else
        {
            Debug.Log("Arrow prefab or object pool is not set.");
        }
    }

    public override int Skill(int skillIndex, int skilllevel)
    {
        int manaused = 0;
        switch (skillIndex)
        {
            case 0:
                manaused = Space(skilllevel);
                break;
            case 1:
                manaused = FanArrows(skilllevel);
                break;
            case 2:
                manaused = ArrowBarrage(skilllevel);
                break;
            case 3:
                manaused = PiercingArrow(skilllevel);
                break;
            case 4:
                manaused = BombArrow(skilllevel);
                break;
            case 5:
                manaused = ArrowRain(skilllevel);
                break;
            case 6:
                manaused = ManaArrows(skilllevel);
                break;
            case 7:
                manaused = RapidFire(skilllevel);
                break;
            case 8:
                manaused = BackstepShot(skilllevel);
                break;
            case 9:
                manaused = QuickReload(skilllevel);
                break;
            case 10:
                manaused = ShootArrow(skilllevel);
                break;
            case 11:
                manaused = EmpowerArrow(skilllevel);
                break;
            default:
                Debug.LogWarning("Invalid skill index.");
                break;
        }
        return manaused;
    }



    //No. 0 구르기 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private int Space(int slevel)
    {
        playerEntity.EnableafterimageSystem();

        return 1;
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ




    //No.1 갈래 화살, 스택 사용 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public int FanArrows(int slevel)
    {
        if (playerEntity.is_alive && !(playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        {
            StartCoroutine(FanArrowsCoroutine(slevel));
        }

        return skillMana[1];
    }

    private IEnumerator FanArrowsCoroutine(int slevel)
    {
        playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();
        playerEntity.PlayAnimation("Attack");
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        yield return new WaitForSeconds(0.5f);
        int arrowdamage = 1;

        if (slevel >= 1)
        {
            BaseShot(0.0f, 1, arrowdamage);
            BaseShot(4.0f, 1, arrowdamage);
            BaseShot(-4.0f, 1, arrowdamage);
        }
        if (slevel >= 2)
        {
            BaseShot(8.0f, 1, arrowdamage);
            BaseShot(-8.0f, 1, arrowdamage);
        }
        if (slevel >= 3)
        {
            BaseShot(12.0f, 1, arrowdamage);
            BaseShot(-12.0f, 1, arrowdamage);
        }
        playerObject.GetComponent<PlayerDeck>().allLockOff();
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ




    //No.2 화살 세례 / 스택 사용 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public int ArrowBarrage(int slevel)
    {
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();

        float chargingtime = 1.0f;
        isCharging = true;
        SetFillAmount(0.0f);
        ShowCastingBar();

        // chargingtime동안 아무 입력도 들어오지 않으면 실행
        chargingCoroutine = StartCoroutine(ChargingAndExecute(chargingtime, slevel));

        return skillMana[2];
    }
    private IEnumerator ChargingAndExecute(float chargingtime, int slevel)
    {
        float elapsedTime = 0f;
        bool attackanimationstarted = false;
        if(slevel == 1)
        {
            SetSpellName("화살 세례 I");
        }
        else if(slevel == 2)
        {
            SetSpellName("화살 세례 II");
        }
        else if (slevel == 3)
        {
            SetSpellName("화살 세례 III");
        }

        yield return new WaitForSeconds(0.1f);

        while (elapsedTime < chargingtime)
        {
            if (Input.anyKeyDown)
            {
                CancelCharging();
                HideCastingBar();
                playerEntity.CharacterIdleSet();
                yield break;
            }
            if (elapsedTime >= chargingtime - baseAttackCooltime && !attackanimationstarted)
            {
                playerEntity.PlayAnimation("Attack");
                attackanimationstarted = true;
            }
            elapsedTime += Time.deltaTime;
            SetCastTime(elapsedTime.ToString("F1"));
            SetFillAmount(elapsedTime);
            yield return null;
        }

        StartCoroutine(ArrowBarrageCoroutine(slevel));

        HideCastingBar();
        playerObject.GetComponent<PlayerDeck>().allLockOff();
        isCharging = false;
    }
    private void CancelCharging()
    {
        if (isCharging)
        {
            if (chargingCoroutine != null)
            {
                StopCoroutine(chargingCoroutine);
                chargingCoroutine = null;
            }
            playerObject.GetComponent<PlayerDeck>().allLockOff();
            isCharging = false;
            Debug.Log("Charging cancelled due to input.");
        }
    }
    private IEnumerator ArrowBarrageCoroutine(int slevel)
    {
        playerEntity.CharacterStop();

        int arrowdamage = 1;
        int arrownum = 8;

        for (int i = 0; i < arrownum; i++)
        {
            if (arrowPrefab != null && _Pool != null)
            {
                playerEntity.CharacterStop();
                // 화살 발사 방향 계산 (Vector2 to Vector3 변환)
                Vector3 targetVec = mouseWorldPosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);
                targetVec.z = 0; // Vector3 변환 시 z 축 값은 0으로 설정

                // 오브젝트 풀에서 화살 객체 가져오기
                ArrowObject arrow = _Pool.Get();

                // 화살 위치 설정
                arrow.transform.position = playerObject.transform.position; // Archer의 현재 위치를 기준으로 설정
                arrow.transform.rotation = Quaternion.identity; // 초기 회전 상태를 기본 값으로 설정
                arrow.transform.position += new Vector3(0.0f, 0.25f, 0.0f);

                // 화살 발사 (Shoot 메서드 내부에서 화살의 방향과 속도 처리)
                arrow.Shoot(targetVec.normalized * arrowSpeed, 0.0f); // Shoot 메서드가 방향 벡터를 받아 처리하도록 가정

                // 필요한 경우, 화살의 데이터 설정 메서드 호출
                arrow.SetArrowData(arrowdamage, 0, arrowSpeed, 1); // 화살에 대한 추가 정보 설정
            }
            else
            {
                Debug.Log("Arrow prefab or object pool is not set.");
            }
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ






    private int PiercingArrow(int slevel)
    {
        Debug.Log("Performing Piercing Arrow (장거리 일자 관통형 투사체)");
        // Piercing Arrow implementation
        return 0;
    }

    private int BombArrow(int slevel)
    {
        Debug.Log("Performing Bomb Arrow (도착지점에서 폭발하는 범위형 스킬)");
        // Bomb Arrow implementation
        return 0;
    }

    private int ArrowRain(int slevel)
    {
        Debug.Log("Performing Arrow Rain (범위형 화살 공격 + 방깎 시너지)");
        // Arrow Rain implementation
        return 0;
    }

    private int ManaArrows(int slevel)
    {
        Debug.Log("Performing Mana Arrows (3발 발사, 적중시 2마나씩 회복)");
        // Mana Arrows implementation
        return 0;
    }

    private int RapidFire(int slevel)
    {
        Debug.Log("Performing Rapid Fire (2~3발 연사)");
        // Rapid Fire implementation
        return 0;
    }

    private int BackstepShot(int slevel)
    {
        Debug.Log("Performing Backstep Shot (바라보는 방향에서 백스텝 하며 강한 화살 발사)");
        // Backstep Shot implementation
        return 0;
    }

    private int QuickReload(int slevel)
    {
        Debug.Log("Performing Quick Reload (스킬 사이 재장전 시간 초기화)");
        // Quick Reload implementation
        return 0;
    }

    private int ShootArrow(int slevel)
    {
        Debug.Log("Performing Shoot Arrow (그냥 일반 공격)");
        // Shoot Arrow implementation
        return 0;
    }

    private int EmpowerArrow(int slevel)
    {
        Debug.Log("Performing Empower Arrow (다음 공격 20% 강화 및 마나 1 감소)");
        // Empower Arrow implementation
        return 0;
    }






    void Awake()
    {
        _Pool = new ObjectPool<ArrowObject>(CreateArrow, OnGetArrow, OnReleaseArrow, OnDestroyArrow, maxSize: 30);
        playerObject = GameObject.Find("PlayerObject");
        lastUseTime = -1 * baseAttackCooltime;
    }












    //------------ DONT EDIT -------------

    private ArrowObject CreateArrow()
    {
        ArrowObject arrow = Instantiate(arrowPrefab).GetComponent<ArrowObject>();
        arrow.SetManagedPool(_Pool); // ArrowObject가 오브젝트 풀에 관리되도록 설정
        return arrow;
    }

    private void OnGetArrow(ArrowObject arrow)
    {
        arrow.gameObject.SetActive(true); // 오브젝트 풀에서 ArrowObject를 가져올 때 활성화
    }

    private void OnReleaseArrow(ArrowObject arrow)
    {
        arrow.gameObject.SetActive(false); // ArrowObject를 오브젝트 풀로 반환할 때 비활성화
    }

    private void OnDestroyArrow(ArrowObject arrow)
    {
        Destroy(arrow.gameObject); // 필요하지 않은 ArrowObject를 파괴
    }
}