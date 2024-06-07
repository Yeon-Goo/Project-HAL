using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StackText : MonoBehaviour
{
    public TextMeshProUGUI text;

    private Transform target;

    void Awake()
    {
        // TextMeshProUGUI 컴포넌트를 초기화
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        // 텍스트 색상 설정
        text.color = Color.black;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + new Vector3(2.3f, -1.5f, 0));
            transform.position = screenPos;
        }
    }

    public void Setup(Transform target, int arrowstack)
    {
        this.target = target;
        text.text = arrowstack.ToString();
    }

    // 몬스터가 죽으면 텍스트 오브젝트를 파괴하는 메서드
    public void DestroyText()
    {
        Destroy(gameObject);
    }
}