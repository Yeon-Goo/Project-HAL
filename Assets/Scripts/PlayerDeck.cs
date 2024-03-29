using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Card
{
    public int num; // ī�� ���� ��ȣ
    public int level; // ī�� ����
    public int count; // �ش� ī���� ����
    public bool isUnlocked; // ī���� �ر� ����

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
    private List<Card> deck = new List<Card>(); // ī�� ��
    [SerializeField]
    private List<Image> cardImages = new List<Image>(); // ī�� �̹��� ������Ʈ ����Ʈ
    [SerializeField]
    private List<TextMeshProUGUI> cardTexts = new List<TextMeshProUGUI>(); // ī�� �ؽ�Ʈ ������Ʈ ����Ʈ

    private GameObject weapon;

    void Start()
    {
        //need to fix - can select weapon
        weapon = GameObject.Find("Archer");
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
        }
    }

    private void WeaponUse()
    {
       
    }
    private void UseCard(int index)
    {
        this.GetComponent<PlayerEntityMovementController>().CharacterStop();

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
#if DEBUG
        Debug.Log($"Card used: Num={selectedCard.num}, Level={selectedCard.level}. Shifted to the end.");
#endif
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
