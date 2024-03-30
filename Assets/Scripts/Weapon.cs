using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Weapon : MonoBehaviour
{
    // 카드 번호에 따른 스킬 실행
    public virtual void Skill(int num, int level)
    {
        // 여기서 간단한 조건문을 사용하여 num에 따른 다른 스킬을 실행할 수 있습니다.
        // 예시:
        switch (num)
        {
            case 0:
                Debug.Log("Card 0 activated.");
                break;
            case 1:
                Debug.Log("Card 1 activated.");
                break;
            // 추가적인 스킬 케이스
            default:
                Debug.Log($"Card {num} not implemented.");
                break;
        }
    }
}
