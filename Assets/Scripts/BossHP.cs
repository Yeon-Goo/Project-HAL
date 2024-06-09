using UnityEngine;
using UnityEngine.UI;

public class BossHP : MonoBehaviour
{
    public Image filledSlider;  // 채워진 슬라이더 이미지
    public Image emptySlider;   // 빈 슬라이더 이미지

    private StatManager stat_manager;

    void Start()
    {
        // StatManager 컴포넌트를 가져옵니다.
        stat_manager = GetComponent<StatManager>();

        // 초기 체력 바 업데이트
        UpdateHealthBar();

        // 체력 바 위치 설정
        SetHealthBarPosition();
    }

    // 체력 바를 업데이트하는 메서드
    void Update()
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        if (stat_manager != null)
        {
            // 현재 체력 비율을 계산
            float fillAmount = stat_manager.Cur_hp / stat_manager.Max_hp;

            // 채워진 슬라이더의 fillAmount를 설정
            filledSlider.fillAmount = fillAmount;
        }
    }

    private void SetHealthBarPosition()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();

        // 부모를 Canvas로 설정
        rectTransform.SetParent(GameObject.Find("Canvas").transform, false);

        // 화면 상단 중앙에 위치를 고정
        rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
        rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
        rectTransform.anchoredPosition = new Vector2(0, -30); // 화면 상단에서 30px 아래
        rectTransform.pivot = new Vector2(0.5f, 1.0f);
    }

    // 체력을 감소시키는 예시 메서드 (테스트용)
    public void TakeDamage(float damage)
    {
        if (stat_manager != null)
        {
            stat_manager.Cur_hp -= damage;
            if (stat_manager.Cur_hp < 0)
            {
                stat_manager.Cur_hp = 0;
            }

            // 체력 바 업데이트
            UpdateHealthBar();
        }
    }
}