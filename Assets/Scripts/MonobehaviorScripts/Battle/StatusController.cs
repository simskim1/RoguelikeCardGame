using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    private float finalDamage;
    public List<StatusInstance> activeStatuses = new List<StatusInstance>();

    public event System.Action OnStatusChanged; // 상태 변화를 알리는 이벤트

    //카드로 스테이터스를 붙일 때는 CardEffect를, 적이 스테이터스를 붙일때는 의도들에 효과를 다는 형태로 이용
    public void AddStatus(StatusEffect effect, int amt, int dur)
    {
        // 이미 해당 상태 이상이 있는지 확인
        var existing = activeStatuses.Find(s => s.effectData == effect);
        if (existing != null)
        {
            existing.AddStacks(amt);
            existing.AddDuration(dur);
        }
        else
        {
            var newInst = new StatusInstance(effect, amt, dur);
            activeStatuses.Add(newInst);
            effect.OnApply(gameObject, amt);
            Debug.Log($"상태 부여! 현재 {gameObject.name}의 상태 이상 개수: {activeStatuses.Count}");
        }
        OnStatusChanged?.Invoke(); // 변화가 생겼음을 알림
    }

    public void TickTurn()
    {
        for (int i = activeStatuses.Count - 1; i >= 0; i--)
        {

            var inst = activeStatuses[i];
            inst.effectData.OnTurnStart(gameObject, inst);
            inst.AddDuration(-1);

            if (inst.duration <= 0)
            {
                inst.effectData.OnRemove(gameObject);
                activeStatuses.RemoveAt(i);
            }
        }
        OnStatusChanged?.Invoke();
    }

    //적이 데미지를 받을 때는 자기 자신의 버프/ 디버프와 플레이어의 버프 디버프 체크, 반대도 마친가지 이므로 각각 Enemy.TakeDamage와 Enemy.TakeAction에서 수행
    public int DamageCheck(float incomingDamage)
    {
        // 1. 초기값을 입력받은 데미지로 설정
        float currentCalculatedDamage = incomingDamage;

        Debug.Log($"DamageCheck 시작! 현재 상태 이상 개수: {activeStatuses.Count}");
        for (int i = activeStatuses.Count - 1; i >= 0; i--)
        {
            Debug.Log($"순회 중인 효과: {activeStatuses[i].effectData.name}");
            var inst = activeStatuses[i];

            // 2. 중요: '이전까지 계산된 값'을 넣고 '새 결과'로 갱신함 (누적)
            currentCalculatedDamage = inst.effectData.OnProcessDamage(currentCalculatedDamage, inst);
        }

        // 3. 최종적으로 모든 연산이 끝난 값을 반환
        return Mathf.RoundToInt(currentCalculatedDamage);
    }
}