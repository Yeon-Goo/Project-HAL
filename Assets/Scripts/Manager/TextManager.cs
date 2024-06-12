using UnityEngine;
using TMPro;
using System.Collections; // TextMeshProUGUI 사용을 위해 필요

public class TextManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI centralText; // 중앙에 표시할 텍스트

    private void Awake()
    {
        if (centralText != null)
        {
            centralText.gameObject.SetActive(false); // 시작할 때 텍스트 비활성화
        }
        else
        {
            Debug.LogError("CentralText is not assigned!");
        }
    }

    public void ShowCentralText(string message)
    {
        if (centralText != null)
        {
            centralText.text = message;
            centralText.gameObject.SetActive(true);
            StartCoroutine(HideTextAfterDelay(5f)); // 3초 후에 텍스트 숨기기
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        centralText.gameObject.SetActive(false);
    }
}
