using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Generic attribute for an entity's max health.
/// </summary>
[Serializable]
public class MaxHealthAttribute : Attribute
{
    public MaxHealthAttribute(float baseValue)
        : base(Attributes.MaxHealthAttributes, baseValue) { }
}

/// <summary>
/// Generic attribute for entity's health.
/// </summary>
[Serializable]
public class HealthAttribute : Attribute
{
    private MaxHealthAttribute _maxHealth;
    public HealthAttribute()
        : base(Attributes.HealthAttributes, 0) { }
    
    /// <summary>
    /// Sets the references to the fields.
    /// </summary>
    /// <param name="maxHealth">The max health attribute.</param>
    public void SetReferences(MaxHealthAttribute maxHealth)
    {
        // Assign attributes.
        _maxHealth = maxHealth;

        // Set the value to the max health.
        base.Value = (float) _maxHealth.GetValue();
    }

    /// <summary>
    /// Avoid hardcoding the value of the health attribute.
    /// POLYMORPHISM
    /// </summary>
    public override void Update()
    {
        var val = (float) base.Value;
        
        // Get the associated max health attribute.
        var maxHealth = (float) _maxHealth.GetValue();
        
        // Check if the value is greater than the max health.
        if (val > maxHealth)
            val = maxHealth;
        
        // Check if the value is less than 0.
        if (val < 0)
            val = 0;
    }
}

/// <summary>
/// Generic attribute for an entity's health consumable.
/// </summary>
[Serializable]
public class HealthConsumableAttribute : Attribute
{
    public HealthConsumableAttribute(float baseValue)
        : base(Attributes.HealthConsumableAttributes, baseValue) { }
}