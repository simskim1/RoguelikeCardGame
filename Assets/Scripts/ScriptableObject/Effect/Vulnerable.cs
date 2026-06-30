using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "Vulnerable", menuName = "Status/Vulnerable")]
public class Vulnerable : StatusEffect
{

    
    public override float OnProcessDamage(float damage, StatusInstance instance)
    {
        
        Debug.Log("데미지 처리중");
        float resDamage = damage *= (float)1.5;
        return resDamage;
    }
}
