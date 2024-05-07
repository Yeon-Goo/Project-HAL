using UnityEngine;
using UnityEngine.Pool;


public class Archer : Weapon
{
    [SerializeField]
    private GameObject arrowPrefab;
    private GameObject playerObject;
    public float arrowSpeed = 10f;
    private IObjectPool<ArrowObject> _Pool;

    public override int GetMana(int cardnum)
    {
        return 1;
        //Debug.Log("archer get mana called");
    }

    public override void BaseAttack()
    {
        BaseShot(0.0f);
        playerEntity.CharacterAttack();
    }



    //기본 공격이 가능한지 반환
    public override bool BaseAttackAble()
    {
        //weapon별로 구현
        return true;
    }


    public override int Skill(int num, int level)
    {
        BaseShot(0.0f);
        playerEntity.CharacterStop();
        /*switch (num)
        {
            case 1:
                BaseShot();
                break;
            case 2:
                Barrage();
                break;
            case 3:
                PiercingArrow();
                break;
            case 4:
                BombArrow();
                break;
            case 5:
                NormalShot();
                break;
            case 6:
                ArrowRain();
                break;
            case 7:
                RapidFire();
                break;
            case 8:
                GuidedArrow();
                break;
            case 9:
                SwingBow();
                break;
            default:
                Debug.Log("Unknown skill.");
                break; 
        } */
        return 0;
    }

    private void BaseShot(float angleadd)
    {
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (arrowPrefab != null && _Pool != null)
        {
            // 화살 발사 방향 계산 (Vector2 to Vector3 변환)
            Vector3 targetVec = mouseWorldPosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y);
            targetVec.z = 0; // Vector3 변환 시 z 축 값은 0으로 설정

            // 오브젝트 풀에서 화살 객체 가져오기
            ArrowObject arrow = _Pool.Get();

            // 화살 위치 설정
            arrow.transform.position = playerObject.transform.position; // Archer의 현재 위치를 기준으로 설정
            arrow.transform.rotation = Quaternion.identity; // 초기 회전 상태를 기본 값으로 설정

            // 화살 발사 (Shoot 메서드 내부에서 화살의 방향과 속도 처리)
            arrow.Shoot(targetVec.normalized * arrowSpeed, angleadd); // Shoot 메서드가 방향 벡터를 받아 처리하도록 가정

            // 필요한 경우, 화살의 데이터 설정 메서드 호출
            arrow.SetArrowData(1, 0, arrowSpeed); // 화살에 대한 추가 정보 설정
        }
        else
        {
            Debug.Log("Arrow prefab or object pool is not set.");
        }
    }


    void Barrage()
    {
        Debug.Log("Launching Barrage: Rapid fire at a single target with charge and bonus damage.");
    }

    void PiercingArrow()
    {
        Debug.Log("Launching Piercing Arrow: Fires a projectile that pierces through enemies in a line.");
    }

    void BombArrow()
    {
        Debug.Log("Launching Bomb Arrow: Arrow explodes at the target location, dealing area damage.");
    }

    void NormalShot()
    {
        Debug.Log("Launching Normal Shot: A basic attack.");
    }

    void ArrowRain()
    {
        Debug.Log("Launching Arrow Rain: Area attack with multiple arrows.");
    }

    void RapidFire()
    {
        Debug.Log("Launching Rapid Fire: Shoots 2-3 arrows rapidly.");
    }

    void GuidedArrow()
    {
        Debug.Log("Launching Guided Arrow: Arrow seeks the nearest enemy.");
    }

    void SwingBow()
    {
        Debug.Log("Swinging Bow: Knocks back nearby enemies.");
    }

    void Awake()
    {
        _Pool = new ObjectPool<ArrowObject>(CreateArrow, OnGetArrow, OnReleaseArrow, OnDestroyArrow, maxSize: 30);
        playerObject = GameObject.Find("PlayerObject");
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