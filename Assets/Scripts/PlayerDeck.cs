using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
public class Card
{
    public int num; // 카드 고유 번호
    public int level; // 카드 레벨
    public int count; // 해당 카드의 개수
    public bool isUnlocked; // 카드의 해금 여부
    public Sprite image; // 카드의 이미지

    public Card(int num, int level, int count = 1, bool isUnlocked = false, Sprite image = null)
    {
        this.num = num;
        this.level = level;
        this.count = count;
        this.isUnlocked = isUnlocked;
        this.image = image;
    }
}

public class PlayerDeck : MonoBehaviour
{
    [SerializeField]
    private List<Card> deck = new List<Card>(); // 카드 덱
    [SerializeField]
    private List<Image> cardImages = new List<Image>(); // 카드 이미지 오브젝트 리스트
    [SerializeField]
    private List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>(); // 카드 텍스트 오브젝트 리스트

    private GameObject weapon;
    private GameObject playerobject;
    private PlayerEntity playerEntity;
    private StatManager stat_manager;

    private bool allLock = false;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;

    public void allLockOn()
    {
        allLock = true;
    }

    public void allLockOff()
    {
        allLock = false;
    }

    void Start()
    {
        //need to fix - can select weapon-----
        weapon = GameObject.Find("Archer");
        //------------------------------------
        playerobject = GameObject.Find("PlayerObject");
        playerEntity = playerobject.GetComponent<PlayerEntity>();
        stat_manager = Resources.Load<StatManager>("ScriptableObjects/StatManager");

        if (stat_manager == null)
        {
            Debug.LogError("StatManager component not found on PlayerEntity!");
            return;
        }

        InitializeDeck();
        UpdateCardDisplay();

        SoundManager.Instance.PlayMusic("BaseMapMusic");
    }

    // 마우스 포인터의 Y값이 특정 범위 안에 있는지 확인하는 메서드
    public bool IsMouseYWithinRange()
    {
        // 마우스 포인터의 월드 위치 가져오기
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 마우스 포인터의 Y값이 minY와 maxY 사이에 있는지 확인
        if (mouseWorldPosition.y >= this.transform.position.y + minY && mouseWorldPosition.y <= this.transform.position.y + maxY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !allLock) UseCard(0);
        else if (Input.GetKeyDown(KeyCode.W) && !allLock) UseCard(1);
        else if (Input.GetKeyDown(KeyCode.E) && !allLock) UseCard(2);
        else if (Input.GetKeyDown(KeyCode.R) && !allLock) UseCard(3);
        else if (Input.GetMouseButton(0) && !allLock && IsMouseYWithinRange()) BaseAttack();
        //else if (Input.GetMouseButtonDown(0)) BaseAttack();
    }

    private void InitializeDeck()
    {
        for (int i = 0; i < 8; i++)
        {
            // 임시로 동일한 이미지 사용. 실제로는 각 카드마다 다른 이미지를 할당해야 함
            Sprite cardImage = Resources.Load<Sprite>($"CardImages/Card{i}");
            if(cardImage == null)
            {
                Debug.Log("image null error");
            }
            deck.Add(new Card(i, 1, 1, true, cardImage));
        }
    }

    private void BaseAttack()
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon object not found.");
        }

        playerEntity.is_looking_right = (playerEntity.GetMousePos().x > playerEntity.GetPos().x) ? true : false;

        if (weapon.GetComponent<Weapon>().BaseAttackAble())
        {
            weapon.GetComponent<Weapon>().BaseAttack();
        }
    }

    private void UseCard(int index)
    {
        //ERROR CHECK
        if (deck.Count <= index)
        {
            Debug.LogWarning("Invalid card selection.");
            return;
        }
        if (weapon == null)
        {
            Debug.LogError("Weapon object not found.");
        }
        //-----------

        //카드 고유번호에 따른 필요 마나 확인
        int mananeed = weapon.GetComponent<Weapon>().GetMana(deck[index].num);

        if (stat_manager.Cur_mp >= mananeed)
        {
            playerEntity.is_looking_right = (playerEntity.GetMousePos().x > playerEntity.GetPos().x) ? true : false;

            stat_manager.Cur_mp -= weapon.GetComponent<Weapon>().Skill(deck[index].num, deck[index].level);
            //해당 카드와 5번째 카드 스와핑
            Card temp = deck[index];
            deck[index] = deck[4];
            deck[4] = temp;

            //5번째 카드 삭제 후 맨 뒤에 추가
            deck.RemoveAt(4);
            deck.Add(temp);

            UpdateCardDisplay();
        }
        else
        {
            Debug.Log("Not enough mana.");
        }
    }

    private void UpdateCardDisplay()
    {
        for (int i = 0; i < cardImages.Count; i++)
        {
            if (i < deck.Count)
            {
                cardTexts[i].text = "";
                cardImages[i].sprite = deck[i].image; // 카드 이미지 업데이트
                cardImages[i].enabled = true; // 이미지 활성화
            }
            else
            {
                cardTexts[i].text = "";
                cardImages[i].enabled = false; // 이미지 비활성화
            }
        }
    }
}
