using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MarketCardUI : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> cardList; // 카드 오브젝트 리스트
    [SerializeField]
    private List<Sprite> cardImages; // 카드 이미지 리스트
    [SerializeField]
    private List<string> skillNames; // 스킬 이름 리스트

    private void Awake()
    {
        // 카드 리스트 초기화
        cardList = new List<GameObject>();

        // 하이어라키에서 CardList 오브젝트의 자식들을 카드 리스트에 추가
        Transform cardListParent = GameObject.Find("MarketUI").transform;
        foreach (Transform card in cardListParent)
        {
            if(card.name != "background image")
            {
                cardList.Add(card.gameObject);
            }
        }

        // 이미지와 스킬 이름 초기화
        cardImages = new List<Sprite>
        {
            Resources.Load<Sprite>("CardImages/Card0"),
            Resources.Load<Sprite>("CardImages/Card1"),
            Resources.Load<Sprite>("CardImages/Card2"),
            Resources.Load<Sprite>("CardImages/Card3"),
            Resources.Load<Sprite>("CardImages/Card4"),
            Resources.Load<Sprite>("CardImages/Card5"),
            Resources.Load<Sprite>("CardImages/Card6"),
            Resources.Load<Sprite>("CardImages/Card7"),
        };

        skillNames = new List<string>
        {
            "Skill 0",
            "Skill 1",
            "Skill 2",
            "Skill 3",
            "Skill 4",
            "Skill 5",
            "Skill 6",
            "Skill 7",
        };

        // 카드 정보를 설정
        for (int i = 0; i < cardList.Count; i++)
        {
            SetCardInfo(cardList[i], i);
        }
    }

    public void SetCardInfo(GameObject cardObject, int index)
    {
        Image cardImage = cardObject.GetComponent<Image>();
        TextMeshProUGUI cardLevelText = cardObject.transform.Find("level").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI skillNameText = cardObject.transform.Find("name").GetComponent<TextMeshProUGUI>();

        if (index < cardImages.Count)
        {
            cardImage.sprite = cardImages[index];
        }

        // 카드 레벨은 임의로 설정 (예제용)
        int cardLevel = index + 1;
        cardLevelText.text = $"Level : {cardLevel}";

        if (index < skillNames.Count)
        {
            skillNameText.text = skillNames[index];
        }
    }
}
