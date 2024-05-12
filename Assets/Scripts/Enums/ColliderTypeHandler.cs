using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

//
// 요약:
//     Class for a default Enums in Project.
public class ColliderTypeHandler
{
    //
    // 요약:
    //     충돌체 모양을 나타냅니다.
    public enum ColliderType
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
    public static ColliderType CastFrom(int value)
    {
        if (Enum.IsDefined(typeof(ColliderType), value))
        {
            return (ColliderType)value;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Invalid value for ColliderType");
        }
    }

    // 요약:
    //     문자열을 해당 타입으로 변환합니다.
    public static ColliderType CastFrom(string value)
    {
        if (Enum.TryParse<ColliderType>(value, true, out ColliderType result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException("Invalid name for ColliderType", nameof(value));
        }
    }
}
