using UnityEngine;

[CreateAssetMenu(fileName = "NewBloodRage", menuName = "Powers/BloodRage")]
public class BloodRage : AbstractPower
{
    // 이제 부모 클래스의 메서드를 재정의(override)하여 사용할 수 있습니다.
    public override void onLoseHp()
    {
        DeckManager.Instance.DrawCard(1);
    }
}