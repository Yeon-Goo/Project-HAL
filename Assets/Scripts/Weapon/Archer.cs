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
    private bool isBuffActive = false;
    private Coroutine chargingCoroutine;
    private int recovermanaamount = 2;

    private StatManager stat_manager;
    [SerializeField]
    private GameObject afterImagePrefab;
    private GameObject afterImageObject;
    private Coroutine afterImageCoroutine;


    int[] skillMana = new int[] {1, 4, 8, 6, 4, 1, 1, 1, 1, 1, 1, 1 };
    private Vector2 mouseWorldPosition;

    public override int GetMana(int cardnum)
    {
        return skillMana[cardnum];
    }

    public override void BaseAttack()
    {
        //if (playerEntity.is_alive && (playerEntity.is_animation_started ^ playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        //if (playerEntity.is_alive && !(playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        //{
            StartCoroutine(BaseAttackCoroutine());
        //}
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

            if (playerEntity.is_alive && !(playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
            {
                return true;
            }
        }
        return false;
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
            arrow.SetArrowData(damage, 1, 0, arrowSpeed, type, true); // 화살에 대한 추가 정보 설정

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
                manaused = BuffSkill(skilllevel);
                break;
            case 5:
                manaused = ManaArrows(skilllevel);
                break;
            case 6:
                manaused = ArrowRain(skilllevel);
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
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        StartCoroutine(DashCoroutine(slevel));

        return skillMana[0];
    }

    private IEnumerator DashCoroutine(int slevel)
    {
        playerEntity.EnableafterimageSystem();
        float dashDistance = 3.0f;
        float dashTime = 0.1f;
        float dashSpeed = 10.0f;
        Vector2 startPosition = playerObject.transform.position;
        Vector2 dashDirection = (mouseWorldPosition - startPosition).normalized;
        Vector2 targetPosition = startPosition + dashDirection * dashDistance;
        playerEntity.target_pos = targetPosition;

        float temp = playerEntity.velocity;

        playerEntity.velocity = playerEntity.velocity * dashSpeed;

        playerEntity.vectorreset();

        yield return new WaitForSeconds(dashTime);

        playerEntity.DisableafterimageSystem();
        playerEntity.velocity = temp;
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ




    //No.1 갈래 화살, 스택 사용 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public int FanArrows(int slevel)
    {
        //if (playerEntity.is_alive && !(playerEntity.is_animation_playing ^ playerEntity.is_animation_cancelable))
        //{
            StartCoroutine(FanArrowsCoroutine(slevel));
        //}

        return skillMana[1];
    }

    private IEnumerator FanArrowsCoroutine(int slevel)
    {
        playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        while (!BaseAttackAble())
        {
            yield return new WaitForSeconds(0.1f); // 0.1초 대기 후 다시 체크
        }

        playerEntity.PlayAnimation("Attack");
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




    //No.2 화살 세례 / 스택 사용 차징 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    public int ArrowBarrage(int slevel)
    {
        playerEntity.CharacterStop();
        playerEntity.CharacterIdleSet();
        playerEntity.is_animation_started = true;
        playerEntity.is_moveable = false;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();

        float chargingtime = 1.0f;
        isCharging = true;
        SetFillAmount(0.0f);
        ShowCastingBar();

        // chargingtime동안 아무 입력도 들어오지 않으면 실행
        chargingCoroutine = StartCoroutine(ChargingAndExecute(chargingtime, slevel));

        playerEntity.is_moveable = true;
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
            SetFillAmount(elapsedTime / chargingtime);
            yield return null;
        }

        StartCoroutine(ArrowBarrageCoroutine(slevel));

        HideCastingBar();
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
        int arrownum = 5 + 3 * slevel;

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
                arrow.SetArrowData(arrowdamage, 1, 0, arrowSpeed, 1, true); // 화살에 대한 추가 정보 설정
            }
            else
            {
                Debug.Log("Arrow prefab or object pool is not set.");
            }
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }
        playerObject.GetComponent<PlayerDeck>().allLockOff();
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ





    //No.3 꿰뚫는 화살 / 스택 전부 사용 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private int PiercingArrow(int slevel)
    {
        StartCoroutine(PiercingArrowCoroutine(slevel));

        return skillMana[3];
    }

    private IEnumerator PiercingArrowCoroutine(int slevel)
    {
        playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        while (!BaseAttackAble())
        {
            yield return new WaitForSeconds(0.1f); // 0.1초 대기 후 다시 체크
        }

        playerEntity.PlayAnimation("Attack");
        yield return new WaitForSeconds(0.5f);
        int arrowdamage = 1;
        BaseShot(0.0f, 2, arrowdamage);

        playerObject.GetComponent<PlayerDeck>().allLockOff();
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ



    //No.4 잔상 공격 / 버프 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private int BuffSkill(int slevel)
    {
        if (afterImageCoroutine != null)
        {
            StopCoroutine(afterImageCoroutine);
            Destroy(afterImageObject);
        }
        afterImageCoroutine = StartCoroutine(EnableAfterImage(slevel));

        return skillMana[4]; // 버프 스킬에 필요한 마나 반환
    }

    private IEnumerator EnableAfterImage(int slevel)
    {
        // 잔상 오브젝트 생성 및 활성화
        if (afterImagePrefab != null)
        {
            if(playerEntity.is_looking_right)
            {
                afterImageObject = Instantiate(afterImagePrefab, playerObject.transform.position + new Vector3(-0.8f, 1.5f, 0.0f), Quaternion.identity);
            }
            else
            {
                afterImageObject = Instantiate(afterImagePrefab, playerObject.transform.position + new Vector3(0.8f, 1.5f, 0.0f), Quaternion.identity);
            }
            
            afterImageObject.transform.SetParent(playerObject.transform);

            //레벨 별 지속시간 다르게 할 경우 구현할 곳

            for (int i = 0; i < 10; i++) // 10초 동안 잔상 유지 - 레벨 별로 지속시간 다르게 구성 가능
            {
                yield return new WaitForSeconds(1.0f);
            }

            Destroy(afterImageObject); // 10초 후 잔상 오브젝트 제거
        }
    }

    public void FireFromAfterImage(Vector3 position)
    {
        if (afterImageObject != null)
        {
            ArrowObject arrow = _Pool.Get();
            arrow.transform.position = afterImageObject.transform.position;
            arrow.transform.rotation = Quaternion.identity;
            arrow.transform.position += new Vector3(0.0f, 0.0f, 0.0f);

            Vector3 targetVec = position - arrow.transform.position;
            arrow.SetArrowData(1, 1, 0, arrowSpeed, 0, false);
            arrow.Shoot(targetVec.normalized, 0.0f);
        }
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ





    //No.5 푸른 화살 / 마나 충전 스킬ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    private int ManaArrows(int slevel)
    {
        StartCoroutine(ManaArrowsCoroutine(slevel));
        return skillMana[5];
    }

    private IEnumerator ManaArrowsCoroutine(int slevel)
    {
        playerEntity.CharacterStop();
        playerObject.GetComponent<PlayerDeck>().allLockOn();
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        while (!BaseAttackAble())
        {
            yield return new WaitForSeconds(0.1f);
        }

        playerEntity.PlayAnimation("Attack");
        yield return new WaitForSeconds(0.5f);

        int arrowdamage = 1;
        int arrownum = 3;

        for (int i = 0; i < arrownum; i++)
        {
            if (arrowPrefab != null && _Pool != null)
            {
                BaseShot(0.0f, 3, arrowdamage);
            }
            else
            {
                Debug.Log("Arrow prefab or object pool is not set.");
            }
            yield return new WaitForSeconds(0.05f);
        }
        playerObject.GetComponent<PlayerDeck>().allLockOff();
    }

    public void RecoverMana()
    {
        stat_manager = Resources.Load<StatManager>("ScriptableObjects/StatManager");
        stat_manager.Cur_mp += recovermanaamount;
        Debug.Log($"Recovered {recovermanaamount} mana. Current mana: {stat_manager.Cur_mp}");
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ



    private int ArrowRain(int slevel)
    {
        Debug.Log("Performing Arrow Rain (범위형 화살 공격 + 방깎 시너지)");
        // Arrow Rain implementation
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