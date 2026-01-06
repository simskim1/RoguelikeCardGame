using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "ScriptableObjects/StatusEffect")]

public abstract class StatusEffect : ScriptableObject
{
    public string effectName;
    public Sprite icon;
    public bool isDebuff;

    // 상태 이상이 부여될 때 실행
    public virtual void OnApply(GameObject target, int stacks) { }

    // 매 턴 시작 시 실행 (틱 처리)
    public virtual void OnTurnStart(GameObject target, StatusInstance instance) { }

    // 공격할 때, 대미지 계산 전 실행 (예: 힘 증가)
    public virtual float OnProcessDamage(float damage, StatusInstance instance) => damage;

    // 상태 이상이 제거될 때 실행
    public virtual void OnRemove(GameObject target) { }
}