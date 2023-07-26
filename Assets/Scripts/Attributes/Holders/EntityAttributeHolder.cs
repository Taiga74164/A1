using System;
using TMPro;
using UnityEngine;

/// <summary>
/// An entity-specific attribute holder.
/// INHERITANCE
/// </summary>
public class EntityAttributeHolder : AttributeHolder
{
    #region Component Reference

    // [Header("Components")]

    #endregion

    #region Entity Attributes

    [Header("Attributes")]
    public MaxHealthAttribute MaxHealth = new(100.0f);
    public HealthAttribute Health = new();
    
    public HealthConsumableAttribute HealthConsumable = new(50.0f);

    #endregion

    /// <summary>
    /// Applies the attributes to the holder.
    /// </summary>
    protected void ApplyAttributes()
    {
        // Apply references.
        Health.SetReferences(MaxHealth);

        // Add attributes.
        CreateAttribute(MaxHealth);
        CreateAttribute(Health);
        CreateAttribute(HealthConsumable);
    }

    private void Awake() => ApplyAttributes();
}