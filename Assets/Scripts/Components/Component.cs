using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : UnityEngine.Component
{
    //
    // 요약:
    //     컴포넌트 활성화 여부를 확인합니다.
    bool Enable;
    //
    // 요약:
    //     계층 구조상에서 이 컴포넌트가 Enable 상태인지를 반환합니다.Enable이 true일지라도 엔티티의 Enable이 false라면 false를 반환합니다.
    bool EnableInHierarchy;
    //
    // 요약:
    //     이 컴포넌트를 소유한 엔티티입니다.
    Entity Entity;
}
