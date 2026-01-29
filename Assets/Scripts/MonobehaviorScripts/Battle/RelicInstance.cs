using UnityEngine;

public class RelicInstance
{
    [SerializeField] private BaseRelic Relic; // "어떤 효과인가?" (SO를 참조)
    public BaseRelic relic => Relic;
    public RelicInstance(BaseRelic data)
    {
        Relic = data;
    }
}