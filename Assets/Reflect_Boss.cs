using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reflect_Boss : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 킹슬라임과 충돌하는지 태그로 확인
        if (collision.gameObject.CompareTag("King_Slime"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // 벽의 태그에 따라 충돌 반응을 다르게 처리
            if (gameObject.CompareTag("VerticalWall"))
            {
                // 세로 벽(왼쪽 또는 오른쪽)에 부딪혔을 때, x축의 속도 반전
                rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
            }
            else if (gameObject.CompareTag("HorizontalWall"))
            {
                // 가로 벽(위 또는 아래)에 부딪혔을 때, y축의 속도 반전
                rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
            }
        }
    }
}
