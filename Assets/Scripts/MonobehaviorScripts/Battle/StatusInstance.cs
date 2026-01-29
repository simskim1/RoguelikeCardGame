using UnityEngine;

[System.Serializable] // 유니티 인스펙터에서 확인하기 위해 필요
public class StatusInstance
{
    [SerializeField] private StatusEffect EffectData; // "어떤 효과인가?" (SO를 참조)
    [SerializeField] private int Stacks;             // "몇 중첩인가?" (개별 데이터)
    [SerializeField] private int Duration;           // "몇 턴 남았나?" (개별 데이터)
    public StatusEffect effectData => EffectData;
    public int stacks => Stacks;
    public int duration => Duration;
    public StatusInstance(StatusEffect data, int amt, int dur)
    {
        EffectData = data;
        Stacks = amt;
        Duration = dur;
    }

    public void AddStacks(int sum)
    {
        Stacks += sum;
    }

    public void AddDuration(int sum)
    {
        Duration += sum;
    }
}