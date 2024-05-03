using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

// Entity의 피격 기능을 구현하는 컴포넌트
public class HitComponent : MonoBehaviour
{
    //
    // 요약:
    //     충돌 그룹을 설정합니다.
    CollisionGroup CollisionGroup;
    //
    // 요약:
    //     Entity를 기준으로 충돌체의 중심점 위치를 설정합니다.
    Vector2 ColliderOffset;
    //
    // 요약:
    //     충돌체의 타입을 설정합니다.
    ColliderType ColliderType;
    //
    // 요약:
    //     직사각형 충돌체의 너비와 높이를 지정합니다.
    Vector2 BoxSize;
    //
    // 요약:
    //     원형 충돌체의 반지름입니다. ColliderType이 Circle일 때 유효합니다.
    float CircleRadius;
    //
    // 요약:
    //     다각형 충돌체를 이루는 점들의 위치입니다. ColliderType이 Polygon일 때 유효합니다.
    List<Vector2> PolygonPoints;

}
