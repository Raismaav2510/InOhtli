using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HP : mov
{
    // Start is called before the first frame update
    [Header("Health Settings")]
    [SerializeField] private float CurrentHealth;
    [SerializeField] private float MaxHealth;
    protected bool Live=true;

    public void Damage(float DamageValue,Transform tr,float HitForce)
    {
        CurrentHealth -= DamageValue;
        Vector2 KnockbackDirection=transform.position-tr.position;
        rb.AddForce(KnockbackDirection*HitForce);
        if (CurrentHealth <= 0 )
        {
            Live=false;
        }
    }

    public void Cure(float CureValue)
    {
        if ((CurrentHealth + CureValue) > CurrentHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth += CureValue;
        }
    }
    
    public float getCurrentHP(){
        return CurrentHealth;    
    }
    public float getMaxHP(){
        return MaxHealth;    
    }
    public void setCurrentHP(float newHP){
        CurrentHealth=newHP;
    }
    public void setMaxHP(float newMaxHP){
        MaxHealth=newMaxHP;
    }

}
