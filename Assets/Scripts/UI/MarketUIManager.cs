using UnityEngine;

public class MarketUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject uiElement; // 충돌 시 표시할 UI 요소

    private void Start()
    {
        // 게임 시작 시 UI를 비활성화
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }

    public void ShowUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }
}
