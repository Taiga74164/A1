using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Use Events in Scriptable Objects.
/// </summary>
[CreateAssetMenu(fileName = "HealingUnitData", menuName = "Scriptable Objects/Healing Unit Data")]
public class HealingUnitData : ScriptableObject
{
    public float HealingThresholdPercent = 33.0f;
    public float HealingAmount = 10.0f;
    public float HealingTime = 5.0f;
    
    public UnityEvent DoHealEvent;
    
    public void Heal() => DoHealEvent?.Invoke();
}
