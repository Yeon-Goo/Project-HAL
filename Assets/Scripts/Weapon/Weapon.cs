using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour
{
    protected PlayerEntity playerEntity;
    protected float lastUseTime;
    protected GameObject CastingBar;
    protected TextMeshProUGUI spellNameText;
    protected TextMeshProUGUI castTimeText;
    protected Image fillImage;
    //0~11 Archer 12~23 Gunner



    // 카드 번호에 따른 스킬 실행
    public virtual int Skill(int num, int level)
    {
        //weapon별로 구현
        return 0;
    }



    public virtual void BaseAttack()
    {
        //weapon별로 구현
    }



    //기본 공격이 가능한지 반환
    public virtual bool BaseAttackAble()
    {
        //weapon별로 구현
        return false;
    }



    //스킬 사용에 필요한 마나 반환
    public virtual int GetMana(int cardnum)
    {
        //weapon별로 구현
        return 99;
    }

    //Casting Bar Script ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ
    // SpellName 텍스트를 설정하는 메서드
    public void ShowCastingBar()
    {
        if (CastingBar != null)
        {
            CastingBar.SetActive(true);
        }
    }
    // CastingBar를 비활성화하는 메서드
    public void HideCastingBar()
    {
        if (CastingBar != null)
        {
            CastingBar.SetActive(false);
        }
    }
    // SpellName 텍스트를 설정하는 메서드
    public void SetSpellName(string spellName)
    {
        if (spellNameText != null)
        {
            spellNameText.text = spellName;
        }
    }
    // CastTime 텍스트를 설정하는 메서드
    public void SetCastTime(string castTime)
    {
        if (castTimeText != null)
        {
            castTimeText.text = castTime;
        }
    }
    // Fill 이미지의 채우기 양을 설정하는 메서드
    public void SetFillAmount(float amount)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = amount;
        }
    }
    //ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void Start()
    {
        playerEntity = GameObject.Find("PlayerObject").GetComponent<PlayerEntity>();

        // 초기 캐스팅 바 텍스트 설정
        CastingBar = GameObject.Find("CastingBar");
        SetSpellName("None");
        SetCastTime("99");
        HideCastingBar();
        spellNameText = CastingBar.transform.Find("SpellName").GetComponent<TextMeshProUGUI>();
        castTimeText = CastingBar.transform.Find("CastTime").GetComponent<TextMeshProUGUI>();
        fillImage = CastingBar.transform.Find("Fill").GetComponent<Image>();
    }
}
