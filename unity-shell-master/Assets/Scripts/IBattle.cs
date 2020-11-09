using UnityEngine;

public interface IBattle
{
    float Recovery(float health);
    
    float Damage(float damage);

    bool Death(bool force = false);

    float MaxHealth();

    float CurrentHealth();

    void ChangeAttack(float value, float time = 0.0f);

    void AttackBack(Vector3 position, float times = 1);
}