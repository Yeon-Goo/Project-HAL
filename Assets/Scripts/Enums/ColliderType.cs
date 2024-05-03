using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//
// 요약:
//     Class for a default Enums in Project.
public class ColliderType
{
    //
    // 요약:
    //     충돌체 모양을 나타냅니다.
    public enum collidertype
    {
        //
        // 요약:
        //     충돌체가 없음을 나타냅니다.
        None,
        //
        // 요약:
        //     직사각형 충돌체를 나타냅니다.
        Box,
        //
        // 요약:
        //     원형 충돌체를 나타냅니다.
        Circle,
        //
        // 요약:
        //     다각형 충돌체를 나타냅니다.
        Polygon
    }

    //
    // 요약:
    //     숫자를 해당 타입으로 변환합니다.
    public static ColliderType CastFrom(byte value)
    {
        
        return 
    }

    // 요약:
    //     문자열을 해당 타입으로 변환합니다.
    public static ColliderType CastFrom(string value)
    {

        return 
    }
}
