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
        hp_manager = playerEntity.GetHPManager();

        InitializeDeck();
        UpdateCardDisplay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) UseCard(0);
        else if (Input.GetKeyDown(KeyCode.W)) UseCard(1);
        else if (Input.GetKeyDown(KeyCode.E)) UseCard(2);
        else if (Input.GetKeyDown(KeyCode.R)) UseCard(3);
    }

    private void InitializeDeck()
    {
        for (int i = 0; i < 8; i++)
        {
            deck.Add(new Card(i, 1, 1, true));
            private Image new_card = Resources.Load<Image>("Prefabs/UI/HPBar/PlayerHPBarUI");
            cardImages.Add(new Image());
            cardTexts.Add(new TextMeshProUGUI());
        }
    }

    private void WeaponUse()
    {
       
    }
    private void UseCard(int index)
    {
        if (deck.Count <= index)
        {
            Debug.LogWarning("Invalid card selection.");
            return;
        }

        if (weapon != null)
        {
            weapon.GetComponent<Weapon>().Skill(deck[index].num, deck[index].level);
        }
        else
        {
            Debug.LogError("Weapon object not found.");
        }

        // 나중에 0 말고 각 스킬의 마나로 바꿔야 함
        if (hp_manager.GetCurrentMP() > 0)
        {
            playerEntity.is_looking_right = (playerEntity.GetMousePos().x > playerEntity.GetPos().x) ? true : false;
            playerEntity.CharacterStop();

            // 나중에 1 말고 각 스킬의 마나로 바꿔야 함
            hp_manager.SetCurrentMP(hp_manager.GetCurrentMP() - 1);

            // Move the selected card to the end of the deck.
            Card selectedCard = deck[index];
            deck.RemoveAt(index);
            deck.Add(selectedCard);

            /* Shift the next card (5th card, if available) to the selected position.
            if (deck.Count > 4)
            {
                Card nextCard = deck[4];
                deck.Insert(index, nextCard);
                deck.RemoveAt(5); // Remove the shifted card from its original position.
            }*/

            UpdateCardDisplay();
            //Debug.Log($"Card used: Num={selectedCard.num}, Level={selectedCard.level}. Shifted to the end.");
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
    }
}
