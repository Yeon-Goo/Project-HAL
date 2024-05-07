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

    public Card(int num, int level, int count = 1, bool isUnlocked = false)
    {
        this.num = num;
        this.level = level;
        this.count = count;
        this.isUnlocked = isUnlocked;
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
    private HPManager hp_manager;
    

    void Start()
    {
        //need to fix - can select weapon
        weapon = GameObject.Find("Archer");
        playerobject = GameObject.Find("PlayerObject");
        playerEntity = playerobject.GetComponent<PlayerEntity>();
        hp_manager = playerEntity.hp_manager;

        InitializeDeck();
        UpdateCardDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) UseCard(0);
        else if (Input.GetKeyDown(KeyCode.W)) UseCard(1);
        else if (Input.GetKeyDown(KeyCode.E)) UseCard(2);
        else if (Input.GetKeyDown(KeyCode.R)) UseCard(3);
        else if (Input.GetMouseButtonDown(0)) BaseAttack();
    }
    private void InitializeDeck()
    {
        for (int i = 0; i < 8; i++)
        {
            deck.Add(new Card(i, 1, 1, true));
        }
    }

    private void BaseAttack()
    {
        if (weapon == null)
        {
            Debug.LogError("Weapon object not found.");
        }

        playerEntity.is_looking_right = (playerEntity.GetMousePos().x > playerEntity.GetPos().x) ? true : false;

        if(weapon.GetComponent<Weapon>().BaseAttackAble())
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
        //Debug.Log(mananeed + " need");

        if (hp_manager.Cur_mp >= mananeed)
        {
            playerEntity.is_looking_right = (playerEntity.GetMousePos().x > playerEntity.GetPos().x) ? true : false;

            hp_manager.Cur_mp -= mananeed;

            weapon.GetComponent<Weapon>().Skill(deck[index].num, deck[index].level);
            //해당 카드와 5번째 카드 스와핑
            Card temp = deck[index];
            deck[index] = deck[4];
            deck[4] = temp;

            //5번째 카드 삭제 후 맨 뒤에 추가
            deck.RemoveAt(4);
            deck.Add(temp);

            UpdateCardDisplay();
            //Debug.Log($"Card used: Num= " + index);
        }
        else
        {
            //Debug.Log(" NO MANA ");
        }
    }

    private void UpdateCardDisplay()
    {
        for (int i = 0; i < cardImages.Count; i++)
        {
            if (i < deck.Count)
            {
                cardTexts[i].text = $"Card {deck[i].num}\nLevel {deck[i].level}";
            }
            else
            {
                cardTexts[i].text = "";
            }
        }
        //UnityEngine.Debug.Log("update good");
    }
}
