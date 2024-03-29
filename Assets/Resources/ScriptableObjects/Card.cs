using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Card", fileName = "Card")]

// Card의 속성을 정의하는 Sciprtable Object 클래스
public class Card : ScriptableObject
{
    // Card의 이름
    public string objectName;
    // Card의 고유 번호
    public int num;
    // Card의 레벨
    public int level;
    // Card의 텍스트
    public TMP_Text text;
    // Card의 sprite
    public Image image;
    // Card의 해금 여부
    public bool is_unlocked;

    public enum CardNameEnum
    {
    }
}
