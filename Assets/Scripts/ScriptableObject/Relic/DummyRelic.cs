using UnityEngine;

[CreateAssetMenu(fileName = "Dummy1", menuName = "Relics/Dummy1")]
public class DummyRelic : BaseRelic, ITurnStartRelic
{
    public override void OnTurnStart()
    {
        Debug.Log("어어어어");
    }
}