using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private MarketUIManager uiManager;

    private void Start()
    {
        // UIManager 컴포넌트를 찾습니다. 이 스크립트를 UIManager와 같은 게임 오브젝트에 붙입니다.
        uiManager = FindObjectOfType<MarketUIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌 시
        {
            uiManager.ShowUI(); // UI 표시
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌이 끝날 시
        {
            uiManager.HideUI(); // UI 비활성화
        }
    }
}
