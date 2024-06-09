using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using System;

public class PlayerEntity : Entity
{
    // Entity의 Stat을 관리하는 변수
    //public StatManager stat_manager;

    // String Variables
    private static string animationState = "AnimationState";
    private static string pickable_objects = "Pickable_Objects";

    // Player's UIs
    private InventoryUI inventory_prefab;
    private InventoryUI inventory_ui;
    private GameObject inventory;
    private GameObject HPBar;
    private GameObject MPBar;
    private TextMeshProUGUI HPBarText;
    private TextMeshProUGUI MPBarText;
    private UnityEngine.UI.Image HPBarFill;
    private UnityEngine.UI.Image MPBarFill;
    private PlayerDeck card_prefab;
    private PlayerDeck card_ui;


    /***
     * Player의 좌표 관련 변수들
     ***/
    [SerializeField]
    // Player의 속력
    public float velocity = 3.0f;


    // Player가 움직일 목표 좌표
    public Vector2 target_pos = new Vector2();


    [SerializeField]
    // Player가 움직일 방향
    private Vector2 vector = new Vector2();
    [SerializeField]
    // Player가 움직일 방향
    private float vector_;
    // Player 크기 설정
    [SerializeField]
    private float playerscale = 1.5f;


    /***
    * Player의 Animation 관련 변수들
    ***/
    public Coroutine player_damage_coroutine = null;
    Coroutine animation_coroutine = null;
    // Player가 살아 있는지
    public bool is_alive = true;
    [SerializeField]
    // Player가 현재 움직일 수 있는 상황인지
    public bool is_moveable = false;
    // Player가 현재 오른쪽을 바라보는지
    public bool is_looking_right = true;
    [SerializeField]
    // Player의 애니메이션이 시작했는지
    public bool is_animation_started = false;
    [SerializeField]
    // Player의 애니메이션이 재생 중인지
    public bool is_animation_playing = false;
    [SerializeField]
    // Player의 애니메이션을 끝낼 수 있는지 (후딜 캔슬)
    public bool is_animation_cancelable = false;
    //[SerializeField]
    // Player가 Idle인지
    //public bool is_idle = true;
    [SerializeField]
    // Player가 무적인지
    private bool is_invincible = false;
    [SerializeField]
    // Player의 무적 시간
    private float interval = 0.0f;


    /***
     * Player의 Components
     ***/
    private Animator animator;
    private new Rigidbody2D rigidbody;
    private ParticleSystem particleSystem;

    // Player가 현재 어떤 애니메이션을 재생해야 하는지 저장하는 변수
    enum AnimationStateEnum
    {
        idle = 0,
        walk = 1,
        attack = 2,
        dead = 3,
        damaged = 4
    }
    void Start()
    {
        // Load HPManager
        stat_manager = Resources.Load<StatManager>("ScriptableObjects/StatManager");
        if (stat_manager == null) return;

        // Load StatManager
        //stat_manager = Resources.Load<StatManager>("ScriptableObjects/StatManager");
        //if (stat_manager == null) return;

        // Load HPBarUI Prefab
        //hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/PlayerHPBarUI");
        //if (hpbar_prefab == null) return;
        // Instantiate HPBarUI
        /*hpbar_ui = Instantiate(hpbar_prefab);
        // HPBarUI를 this의 자식으로 생성
        hpbar_ui.transform.SetParent(this.transform, false);
        if (hpbar_ui == null) return;
        hpbar_ui.Init(this);*/

        // Load InventoryUI Prefab
        inventory_prefab = Resources.Load<InventoryUI>("Prefabs/UI/Inventory/InventoryUI");
        if (inventory_prefab == null) return;
        // Instantiate InventoryUI
        inventory_ui = Instantiate(inventory_prefab);
        // InventoryUI를 this의 자식으로 생성
        inventory_ui.transform.SetParent(this.transform, false);
        if (inventory_ui == null) return;
        inventory = this.transform.GetChild(3).gameObject;
        if (inventory == null) return;

        /*
        // Load CardUI Prefab
        card_prefab = Resources.Load<PlayerDeck>("Prefabs/UI/Card/CardUI");
        if (card_prefab == null) return;
        // Instantiate CardUI
        card_ui = Instantiate(card_prefab);
        // CardUI를 this의 자식으로 생성
        card_ui.transform.SetParent(this.transform, false);
        if (card_ui == null) return;
        */

        StartCoroutine(Manaregen());
        // Get Components
        //animator = GetComponent<Animator>();
        HPBar = GameObject.Find("HPBar");
        MPBar = GameObject.Find("MPBar");
        HPBarText = HPBar.transform.Find("HPPercent").GetComponent<TextMeshProUGUI>();
        MPBarText = MPBar.transform.Find("MPPercent").GetComponent<TextMeshProUGUI>();
        HPBarFill = HPBar.transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
        MPBarFill = MPBar.transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
        animator = GetComponentsInChildren<Animator>()[0];
        rigidbody = GetComponent<Rigidbody2D>();
        particleSystem = transform.Find("afterimage System").GetComponent<ParticleSystem>();
        if (particleSystem == null)
            Debug.LogError("Particle System not found!");
        DisableafterimageSystem();
        //ResetEntity();
        statInit();
    }

    /*
    private void OnEnable()
    {
        ResetEntity();
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ResetEntity();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillEntity();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventory.SetActive(inventory.activeSelf ? false : true);
        }

        vector_ = vector.magnitude;
        HPMPUpdate();
    }

    void FixedUpdate()
    {
        // 무적 시간 계산
        if (is_invincible)
        {
            interval -= Time.deltaTime;
            if (interval < float.Epsilon)
            {
                is_invincible = false;
                this.interval = 0.0f;
            }
        }

        if (is_alive && !(is_animation_started ^ is_animation_cancelable))
        {
            // Stop Player
            if (Input.GetKey(KeyCode.S))
            {
                CharacterStop();
            }
            // Walk
            else
            {
                MoveCharacter_Mouse();
                vectorreset();
            }
        }

        UpdateMovement();
        UpdateAnimationState();
    }

    public void vectorreset()
    {
        vector = target_pos - new Vector2(transform.position.x, transform.position.y);
    }

    private void UpdateMovement()
    {
        // Player의 벡터 정규화
        if (vector.magnitude < 0.1f)
        {
            vector = Vector2.zero;
        }
        else
        {
            vector.Normalize();
        }

        // 캐릭터의 벡터가 0이 아니면 움직이고 있는 상태
        if (!vector.Equals(Vector2.zero))
        {
            // x 방향으로 움직였을 때, x 성분의 값으로 오른쪽 왼쪽을 판단
            if (vector.x != 0) is_looking_right = vector.x > 0 ? true : false;
            // x 방향으로 움직이지 않았을 때, transform.localScale.x의 값으로 오른쪽 왼쪽을 판단
            else is_looking_right = transform.localScale.x.Equals(1.0f) ? true : false;
            // 캐릭터가 바라보는 방향을 움직이는 방향에 맞게 바꿈
            transform.localScale = is_looking_right ? new Vector3(playerscale, playerscale, 1.0f) : new Vector3(-playerscale, playerscale, 1.0f);
        }

        // 캐릭터를 vector와 velocity에 맞게 움직임
        rigidbody.velocity = vector * velocity;
    }

    private void statInit()
    {
        stat_manager.Cur_hp = 20;
        stat_manager.Max_hp = 20;
        stat_manager.Cur_mp = 20;
        stat_manager.Max_mp = 20;
        stat_manager.Armor = 0;
        stat_manager.Movement_speed = 3.0f;
        velocity = stat_manager.Movement_speed;
        stat_manager.Mana_regen = 1;
    }

    private void HPMPUpdate()
    {
        if(HPBarText == null || MPBarText == null)
        {
            Debug.Log("error");
            return;
        }
        HPBarText.text = $"{stat_manager.Cur_hp} / {stat_manager.Max_hp}";
        MPBarText.text = $"{stat_manager.Cur_mp} / {stat_manager.Max_mp}";
        HPBarFill.fillAmount = (float)(stat_manager.Cur_hp / stat_manager.Max_hp);
        MPBarFill.fillAmount = (float)(stat_manager.Cur_mp / stat_manager.Max_mp);
    }

    private IEnumerator Manaregen()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기
            stat_manager.Cur_mp += stat_manager.Mana_regen;
            if (stat_manager.Cur_mp > stat_manager.Max_mp)
            {
                stat_manager.Cur_mp = stat_manager.Max_mp; // 최대 마나를 초과하지 않도록 설정
            }
        }
    }


    private void UpdateAnimationState()
    {
        if (!is_animation_started)
        {
            // WALK
            if (!vector.Equals(Vector2.zero))
            {

                animator.SetInteger(animationState, (int)AnimationStateEnum.walk);
            }
            // IDLE
            else
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
            }
        }
    }


    // 파티클 시스템을 켜는 메서드
    public void EnableafterimageSystem()
    {
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }
    // 파티클 시스템을 끄는 메서드
    public void DisableafterimageSystem()
    {
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }

    public void CharacterIdleSet()
    {
        animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
        is_animation_started = false;
        is_animation_playing = false;
        is_animation_cancelable = false;
    }

    public void CharacterStop()
    {
        is_moveable = false;
        target_pos = transform.position;
        vector = Vector2.zero;
    }

    public void PlayAnimation(string action)
    {
        if (is_animation_cancelable)
        {
            CharacterIdleSet();
            is_animation_started = true;
            is_animation_cancelable = true;
            StopCoroutine(animation_coroutine);
            animation_coroutine = null;

            if (animation_coroutine == null)
            {
                animation_coroutine = StartCoroutine(action, animator);
            }

            transform.localScale = (GetMousePos().x - GetPos().x) > 0 ? new Vector3(playerscale, playerscale, 1.0f) : new Vector3(-playerscale, playerscale, 1.0f);
        }
        else
        {
            is_animation_started = true;

            if (animation_coroutine == null)
            {
                animation_coroutine = StartCoroutine(action, animator);
            }

            transform.localScale = (GetMousePos().x - GetPos().x) > 0 ? new Vector3(playerscale, playerscale, 1.0f) : new Vector3(-playerscale, playerscale, 1.0f);
        }
    }

    public IEnumerator Attack(Animator animator)
    {
        if (!is_animation_playing)
        {
            CharacterStop();

            // 이전에 재생되던 애니메이션이 존재 (캔슬한 경우)
            if (is_animation_cancelable)
            {
                while (!animator.GetCurrentAnimatorStateInfo(0).IsName("0_idle"))
                {
                    animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
                    yield return null;
                }
                is_animation_cancelable = false;
            }

            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("2_Attack_Bow"))
            {
                animator.SetInteger(animationState, (int)AnimationStateEnum.attack);
                yield return null;
            }

            while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                is_animation_playing = true;
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
                {
                    is_animation_cancelable = true;
                    is_moveable = true;
                }
                yield return null;
            }

            if (is_animation_playing)
            {
                CharacterIdleSet();
            }
        }
        is_animation_started = false;
        animation_coroutine = null;
    }

    public IEnumerator Dead(Animator animator)
    {
        CharacterStop();

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("0_idle"))
        {
            animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
            yield return null;
        }

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("4_Death"))
        {
            animator.SetInteger(animationState, (int)AnimationStateEnum.dead);
            yield return null;
        }
        animation_coroutine = null;
    }

    public IEnumerator Revive(Animator animator)
    {
        CharacterStop();

        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("0_idle"))
        {
            animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
            yield return null;
        }

        is_alive = true;
        is_animation_started = false;
        animation_coroutine = null;
    }

    IEnumerator Damaged(Animator animator)
    {
        is_animation_started = true;
        
        animator.SetInteger(animationState, (int)AnimationStateEnum.damaged);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            is_animation_started = false;
            yield return null;
        }

        CharacterStop();
        
    }

    private void MoveCharacter_Mouse()
    {
        // 마우스 오른쪽 버튼을 뗐을 때 || 마우스 오른쪽 버튼을 누르고 있는 상태에서 마우스를 움직였을 때
        if (!is_animation_started && (!Input.GetMouseButton(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)))
        //if (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)
        {
            is_moveable = true;
        }

        // S키를 누르지 않았고 마우스 오른쪽 버튼을 눌렀을 때
        if (is_moveable && Input.GetMouseButton(1))
        //if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        {
            // Player의 벡터 계산
            target_pos = FindPath();
        }
    }

    // 마우스 오른쪽 버튼을 눌렀을 때의 좌표까지 도달할 수 있는 최단 거리 탐색
    private Vector2 FindPath()
    {
        // 지금은 단순히 마우스 오른쪽 버튼을 눌렀을 때의 좌표를 return
        return GetMousePos();

        // 나중에 길찾기 알고리즘을 구현해야 함
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (is_alive && collision.gameObject.CompareTag(pickable_objects))
        {
            PickableObjects hitObject = collision.gameObject.GetComponent<PickableObjects>();

            if (hitObject != null)
            {
                bool should_disappear = false;

                //print("Hit: " + hitObject.GetName());
                switch (hitObject.Item.ItemType)
                {
                    case Item.ItemTypeEnum.GRASS:
                    case Item.ItemTypeEnum.STONE:
                    case Item.ItemTypeEnum.COIN:
                        should_disappear = inventory_ui.AddItem(hitObject);
                        break;
                    case Item.ItemTypeEnum.HEALTH:
                        should_disappear = stat_manager.AdjustHP(hitObject.Quantity);
                        break;
                }

                if (should_disappear)
                {
                    Destroy(collision.gameObject, .0f);
                }
            }
        }
    }

    // 플레이어가 대미지를 받는 함수
    public override IEnumerator DamageEntity(int damage, float interval, GameObject entity)
    {
        //float cur_hp = hp_manager.GetCurrentHP();

        while (is_alive)
        {
            if (!is_invincible)
            {
                is_invincible = true;
                this.interval = interval;

                // this는 entity로부터 damage만큼의 피해를 interval초마다 받는다
                CharacterStop();

                //GetComponent<CinemachineVirtualCamera>().VibrateForTimeAndAmount();

                stat_manager.Cur_hp -= damage;
            }

            // this의 체력이 0일 때
            //if (cur_hp <= float.Epsilon)
            if (stat_manager.Cur_hp <= float.Epsilon)
            {
                KillEntity();
                break;
            }

            // this의 체력이 0보다 크면 interval만큼 실행을 양보(멈춤)
            if (this.interval > float.Epsilon)
            {
                yield return new WaitForSeconds(this.interval);
            }
            else
            {
                break;
            }
        }
    }

    // 플레이어가 사망하는 코드
    override public void KillEntity()
    {
        CharacterIdleSet();
        is_animation_started = true;
        //is_animation_cancelable = true;
        if (animation_coroutine != null)
        {
            StopCoroutine(animation_coroutine);
        }
        animation_coroutine = null;

        if (animation_coroutine == null)
        {
            animation_coroutine = StartCoroutine("Dead", animator);
        }
        is_alive = false;
        stat_manager.Cur_hp = 0;
    }

    // 플레이어가 부활하는 코드
    public override void ResetEntity()
    {
        is_alive = true;
        CharacterIdleSet();
        if (animation_coroutine != null)
        {
            StopCoroutine(animation_coroutine);
        }
        animation_coroutine = null;

        if (animation_coroutine == null)
        {
            animation_coroutine = StartCoroutine("Revive", animator);
        }
        
        stat_manager.Cur_hp = stat_manager.Max_hp;
        stat_manager.Cur_mp = stat_manager.Max_mp;
        //GetComponent<SpriteRenderer>().enabled = true;
        // 미완성
    }
}
