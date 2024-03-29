using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PlayerEntity : Entity
{
    // String Variables
    private static string animationState = "AnimationState";
    private static string pickable_objects = "Pickable_Objects";

    private HPBarUI hpbar_prefab;
    private HPBarUI hpbar_ui;
    private InventoryUI inventory_prefab;
    private InventoryUI inventory_ui;
    //private CardUI card_prefab;
    //private CardUI card_ui;

    // Player�� �ӷ�
    private float velocity = 3.0f;
    // Player�� ����
    private Vector2 vector = new Vector2();
    // Player�� ������ �� �ִ����� ���� (stop ��ɿ� ����)
    private bool is_moveable = false;
    // Player�� ��ǥ ��ǥ
    private Vector2 target_pos = new Vector2();

    // Component Variables
    private UnityEngine.Transform transform;
    private Animator animator;
    private Rigidbody2D rigidbody;

    // Player�� �ִϸ��̼� ����� ���̴� ����
    enum AnimationStateEnum
    {
        idle = 0,
        walk_right = 1,
        walk_left = 2
    }

    void Start()
    {
        // Load HPManager
        hp_manager = Resources.Load<HPManager>("ScriptableObjects/PlayerHPManager");
        if (hp_manager == null) return;

        // Load HPBarUI Prefab
        hpbar_prefab = Resources.Load<HPBarUI>("Prefabs/UI/HPBar/HPBarUI");
        if (hpbar_prefab == null) return;
        // Instantiate HPBarUI
        hpbar_ui = Instantiate(hpbar_prefab);
        if (hpbar_ui == null) return;
        hpbar_ui.Init(this);

        // Load InventoryUI Prefab
        inventory_prefab = Resources.Load<InventoryUI>("Prefabs/UI/Inventory/InventoryUI");
        if (inventory_prefab == null) return;
        // Instantiate InventoryUI
        inventory_ui = Instantiate(inventory_prefab);
        if (inventory_ui == null) return;
        hpbar_ui.Init(this);

        /*
        // Load CardUI Prefab
        card_prefab = Resources.Load<CardUI>("Prefabs/UI/Card/CardUI");
        if (card_prefab == null) return;
        // Instantiate CardUI
        card_ui = Instantiate(card_prefab);
        if (card_ui == null) return;
        */

        // Get Components
        transform = GetComponent<UnityEngine.Transform>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        if (!vector.Equals(Vector2.zero))
        {
            // x �� �Է��� �����ϴ� ���
            // x �� �Է¿� ���� �������� �ٶ���, ������ �ٶ��� ����
            if (vector.x > 0)
            {
                // �������� �ٶ�
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
            }
            else if (vector.x < 0)
            {
                // ������ �ٶ�
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                animator.SetInteger(animationState, (int)AnimationStateEnum.walk_left);
            }
            // x �� �Է��� �������� �ʴ� ���
            // transform�� localScale�� x ���� ���� �������� �ٶ���, ������ �ٶ��� ����
            else
            {
                if (transform.localScale.x.Equals(1.0f))
                {
                    // �������� �ٶ�
                    animator.SetInteger(animationState, (int)AnimationStateEnum.walk_right);
                }
                else
                {
                    // ������ �ٶ�
                    animator.SetInteger(animationState, (int)AnimationStateEnum.walk_left);
                }
            }
        }
        else
        {
            animator.SetInteger(animationState, (int)AnimationStateEnum.idle);
        }
    }

    public void CharacterStop()
    {
        is_moveable = false;
        target_pos = transform.position;
    }

    void FixedUpdate()
    {
        // SŰ�� ������ �� ĳ���Ͱ� ����
        if (Input.GetKey(KeyCode.S))
        {
            CharacterStop();
        }

        //MoveCharacter_KeyBoard();
        MoveCharacter_Mouse();
        //MoveCharacter_Mouse_STOP_WHEN_RELEASED();
    }

    private void MoveCharacter_Mouse()
    {
        // ���콺 ������ ��ư�� ������ ���� �� || (���콺 ������ ��ư�� ������ ���� �� && ���콺 ��ǥ�� ��ȭ�� ���� ��)
        if (!Input.GetMouseButton(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        //if (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f)
        {
            is_moveable = true;
        }

        // SŰ�� ������ �ʾ��� ��
        if (is_moveable && Input.GetMouseButton(1))
        //if (Input.GetMouseButtonDown(1) || (Input.GetMouseButton(1) && new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")).magnitude > 0.05f))
        {
            target_pos = FindPath();
        }

        vector.x = target_pos.x - transform.position.x;
        vector.y = target_pos.y - transform.position.y;

        if (vector.magnitude < 0.1f)
        {
            vector = Vector2.zero;
        }

        vector.Normalize();

        rigidbody.velocity = vector * velocity;
    }

    private Vector2 FindPath()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(pickable_objects))
        {
            Item hitObject = collision.gameObject.GetComponent<PickableObjects>().item;

            if(hitObject != null)
            {
                bool should_disappear = false;

                //print("Hit: " + hitObject.GetName());
                switch (hitObject.GetItemType())
                {
                    case Item.ItemTypeEnum.COIN:
                        should_disappear = inventory_ui.AddItem(hitObject);
                        break;
                    case Item.ItemTypeEnum.HEALTH:
                        should_disappear = AdjustHP(hitObject.GetQuantity());
                        break;
                }

                if (should_disappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    private bool AdjustHP(int amount)
    {
        if (hp_manager.GetCurrentHP() < hp_manager.GetMaxHP())
        {
            hp_manager.SetCurrentHP(hp_manager.GetCurrentHP() + amount);
            //print("Adjusted HP by : " + amount + ". New value : " + hp_manager.cur_hp);

            return true;
        }

        return true;
    }
}
