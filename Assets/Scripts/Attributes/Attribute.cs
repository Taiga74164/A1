﻿using System;
using System.Collections.Generic;

/// <summary>
/// List of attribute IDs.
/// </summary>
public static class Attributes
{
    public const string MaxHealthAttributes = "entity.max_health";
    public const string HealthAttributes = "entity.health";
    
    // ToDo:
    // - Add inventory system instead of putting in attribute.
    public const string HealthConsumableAttributes = "entity.health_consumable";
}

/// <summary>
/// Abstract class for an attribute.
/// </summary>
[Serializable]
public class Attribute
{
    /// <summary>
    /// ID of this attribute.
    /// </summary>
    private readonly string _id;
    
    /// <summary>
    /// Value of this attribute.
    /// </summary>
    protected object Value;
    
    public Attribute(string id, object baseValue)
    {
        _id = id;
        Value = baseValue;
    }
    
    /// <summary>
    /// Returns the attribute's ID.
    /// </summary>
    /// <returns>A type of <see cref="Attributes"/>.</returns>
    public string GetId()
    {
        return _id;
    }
    
    /// <summary>
    /// Called when the attribute is started.
    /// </summary>
    public virtual void Start() { }
    
    /// <summary>
    /// Called when the attribute is updated.
    /// </summary>
    public virtual void Update() { }
    
    /// <summary>
    /// Returns the value of this attribute.
    /// </summary>
    /// <returns>The current, immutable attribute value.</returns>
    public virtual object GetValue() => Value;
    
    /// <summary>
    /// Sets the value of this attribute.
    /// </summary>
    /// <param name="updated">The new value for the attribute.</param>
    public virtual void SetValue(object updated) => Value = updated;
}