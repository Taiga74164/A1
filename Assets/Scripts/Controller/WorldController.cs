using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Controls the game environment.
/// </summary>
[RequireComponent(typeof(ResourceManager))]
public class WorldController : MonoBehaviour
{
    #region Manager References

    [Header("Manager References")]
    public ResourceManager ResourceManager;

    #endregion

    #region Constants

    private GameObject _damageIndicator;

    #endregion

    /// <summary>
    /// Displays damage dealt in the world.
    /// </summary>
    /// <param name="at">Where the damage occurred.</param>
    /// <param name="damage">The damage dealt.</param>
    /// <param name="type">The type of damage dealt.</param>
    public GameObject DisplayDamage(Transform at, float damage, Color color = default(Color))
    {
        // Set the color to white if not specified since default is RGBA(0.000, 0.000, 0.000, 0.000)
        if (color == default(Color))
            color = Color.white;
        
        // Get a position around the target transform within a 1 unit radius.
        var position = at.position + Random.insideUnitSphere;
        
        // Create a damage indicator at the position.
        var indicator = Instantiate(_damageIndicator, position, Quaternion.identity, at.parent);
        
        // Set the indicator text and color.
        var text = indicator.GetComponent<TextMesh>();
        text.text = Math.Round((decimal) damage, 0).ToString(CultureInfo.InvariantCulture);
        text.color = color;
        
        return indicator;
    }

    #region Events

    private void Awake()
    {
        // Load constants.
        _damageIndicator = ResourceManager.LoadResource<GameObject>("Prefabs/World/Damage Display");
    }

    private void Start()
    {
        
    }

    #endregion
}