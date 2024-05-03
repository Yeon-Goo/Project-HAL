using UnityEngine;
using UnityEngine.Events;

// Entity의 공격 기능을 구현하는 컴포넌트
public class AttackComponent : MonoBehaviour
{
    //
    // 요약:
    //     shape 영역 안에 있는 HitComponent의 OnHit(Entity, Integer, boolean, string, int32) 함수를 호출하고 HitEvent를 발생시킵니다.
    //     공격 대상으로 판정된 HitComponent를 모두 반환합니다. attackInfo는 사용자 정의 데이터로 공격을 직접 구현할 때 크리에이터가 원하는 용도에 맞게 활용할 수있습니다.
    //     활용 시 함수를 재정의해야 합니다.
    table<Component> Attack(ColliderType shape, string attackInfo, CollisionGroup collisionGroup = null)
    {
        return;
    }
    //
    // 요약:
    //     사각형 영역을 지정할 수 있는 Attack 함수입니다. size는 사각형의 크기, offset은 엔티티 사각형의 중심 위치입니다.
    table<Component> Attack(Vector2 size, Vector2 offset, string attackInfo, CollisionGroup collisionGroup = null)
    {
        return;
    }
    //
    // 요약:
    //     반환 값이 없는 Attack 함수입니다.불필요한 table 객체 생성을 줄여 월드 성능을 개선할 수 있습니다.
    void AttackFast(ColliderType shape, string attackInfo, CollisionGroup collisionGroup = null)
    {
        return;
    }
    //
    // 요약:
    //     사각형 영역을 지정할 수 있는 Attack 함수입니다.size는 사각형의 크기, position은 월드 좌표 기준 사각형의 중심 위치입니다.
    table<Component> AttackFrom(Vector2 size, Vector2 position, string attackInfo, CollisionGroup collisionGroup = null)
    {
        return;
    }
    //
    // 요약:
    //     크리티컬 공격 여부를 결정합니다.
    bool CalcCritical(Entity attacker, Entity defender, string attackInfo)
    {
        return;
    }
    //
    // 요약:
    //     대미지 값을 결정합니다.
    int CalcDamage(Entity attacker, Entity defender, string attackInfo)
    {
        return;
    }
    //
    // 요약:
    //     크리티컬 공격일 경우, 기본 대미지 대비 몇 배의 대미지를 줄 것인지 결정합니다.
    float GetCriticalDamageRate()
    {
        return;
    }
    //
    // 요약:
    //     한 번의 공격을 몇 개의 히트로 분할하여 표시할 지 결정합니다.
    int GetDisplayHitCount(string attackInfo)
    {
        return;
    }
    //
    // 요약:
    //     defender가 공격 대상인지 판단합니다.
    bool IsAttackTarget(Entity defender, string attackInfo)
    {
        return;
    }
    //
    // 요약:
    //     이 엔티티가 공격할 때 호출되는 함수입니다.
    void OnAttack(Entity defender)
    {

    }

}
