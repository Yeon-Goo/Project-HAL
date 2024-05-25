using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private Transform targetTransform;
    private float tempTime;
    private float rand_x, rand_y;
    private Vector3 pos;
    // 텍스트를 설정하는 메소드
    public void Setup(Transform target, int damage)
    {
        targetTransform = target;
        text.text = damage.ToString();
        text.color = Color.red;
        Destroy(gameObject, 1.0f); // 1초 후에 텍스트 오브젝트 파괴
        tempTime = Time.time;
        rand_x = Random.Range(-0.3f, 0.3f);
        rand_y = Random.Range(-0.1f, 0.1f);
        pos = targetTransform.position;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            // 텍스트가 몬스터를 따라다니도록 위치 갱신
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos + new Vector3(2.3f + rand_x, 1f + rand_y, 0) + new Vector3(0, 0.5f, 0) * (Time.time - tempTime));
            transform.position = screenPosition;
        }
    }
}