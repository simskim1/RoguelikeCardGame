using UnityEngine;

[CreateAssetMenu(fileName = "FullArmor", menuName = "Powers/FullArmor")]
public class FullArmor : AbstractPower
{
    [SerializeField]private StatusEffect power;
    // 이제 부모 클래스의 메서드를 재정의(override)하여 사용할 수 있습니다.
    public override void onHaveGuard10()
    {
        GameObject playerObj = PlayerController.Instance.gameObject;
        StatusController status = PlayerController.Instance.StatusControllerGetter();
        status.AddStatus(power, playerObj, 1, 0);
    }
}