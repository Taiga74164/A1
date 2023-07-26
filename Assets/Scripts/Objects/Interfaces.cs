using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Definition of a living entity.
/// </summary>
public interface ILiving : IDamageable
{
    /// <summary>
    /// Returns the living entity's attribute holder.
    /// </summary>
    /// <returns>An attribute holder.</returns>
    AttributeHolder GetAttributes();
}

/// <summary>
/// An interface for damageable objects.
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// Deals damage to the object.
    /// </summary>
    /// <param name="value">The damage to deal.</param>
    void Damage(/*int value*/);
    
    /// <summary>
    /// Deals damage to the object.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="direction"></param>
    // void Damage(int value, Vector3 direction);
    
    /// <summary>
    /// Heals the object.
    /// </summary>
    /// <param name="value">The amount to heal.</param>
    void Heal(/*int value*/);
}