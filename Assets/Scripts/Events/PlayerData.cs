using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Use Events in Scriptable Objects.
/// </summary>
[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    // public float Health = 100.0f;
    public float DamageDealt;
    public float HealAmount;

    // public event Action DamageEvent;
    // public event Action HealEvent;
    public UnityEvent DamageEvent;
    public UnityEvent HealEvent;
    
    public void Damage() => DamageEvent?.Invoke();

    public void Heal() => HealEvent?.Invoke();
}
