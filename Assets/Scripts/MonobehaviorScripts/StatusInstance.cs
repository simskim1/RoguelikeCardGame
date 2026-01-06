using UnityEngine;

[System.Serializable] // 유니티 인스펙터에서 확인하기 위해 필요
public class StatusInstance
{
    public StatusEffect effectData; // "어떤 효과인가?" (SO를 참조)
    public int stacks;             // "몇 중첩인가?" (개별 데이터)
    public int duration;           // "몇 턴 남았나?" (개별 데이터)

    public StatusInstance(StatusEffect data, int amt, int dur)
    {
        effectData = data;
        stacks = amt;
        duration = dur;
    }
}