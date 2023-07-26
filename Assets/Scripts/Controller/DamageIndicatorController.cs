using System;
using UnityEngine;

/// <summary>
/// Controls a damage indicator.
/// </summary>
public class DamageIndicatorController : MonoBehaviour
{
    private void Start()
    {
        // Destroy the indicator.
        Destroy(gameObject, 0.2f);
    }

    public void Update()
    {
        // Move the indicator up slowly.
        transform.Translate(Vector3.up * Time.deltaTime * 8.0f);
    }
}