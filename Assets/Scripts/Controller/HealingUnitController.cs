using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public enum HealingUnitState
{
    Idle = 0,
    Healing = 1,
    Recharging = 2
}

// ToDo:
// - Implement Pathfinding/NavMeshAgent
public class HealingUnitController : MonoBehaviour
{
    #region Healing Unit Components

    [Header("Healing Unit Components")]
    public PlayerController Player;
    public GameObject BaseLocation;
    public NavMeshAgent NavMeshAgent;
    #endregion

    #region Settings

    [Header("Healing Unit Settings")]
    public float TravelSpeed = 5.0f;
    public float HealingThresholdPercent = 33.0f;
    public float HealingAmount = 10.0f;
    public float HealingTime = 5.0f;
    public float RechargeTime = 5.0f;

    #endregion
    
    #region Properties

    private bool _isHealing;

    #endregion

    #region Data

    // public HealingUnitData HealingUnitData;

    public UnityEvent OnHealEvent;
    
    #endregion
    
    private void Start()
    {
        // Subscribe to the player's heal event.
        Player.PlayerData.HealEvent.AddListener(OnHealEvent.Invoke);
        
        // Monitor the player's health.
        StartCoroutine(MonitorPlayerHealth());
        
        // Set the healing unit's speed.
        // NavMeshAgent.speed = TravelSpeed;
    }

    private void Update()
    {
        // InvokeState();
    }

    public void OnPlayerHeal()
    {
        _isHealing = true;
        StartCoroutine(HealPlayer());
    }
    
    private IEnumerator MonitorPlayerHealth()
    {
        while (true)
        {
            var playerHealthPercent = Player.Health / Player.GetMaxHealth() * 100.0f;
            if (playerHealthPercent >= 100.0f)
            {
                // Unsubscribe from the player's heal event.
                Player.PlayerData.HealEvent.RemoveListener(OnHealEvent.Invoke);
                _isHealing = false;
            }
            else if (playerHealthPercent <= HealingThresholdPercent && !_isHealing)
            {
                OnHealEvent.Invoke();
            }
            else
            {
                // Unsubscribe from the player's heal event.
                Player.PlayerData.HealEvent.RemoveListener(OnHealEvent.Invoke);
                Debug.Log(playerHealthPercent);
            }
            
            yield return null;
        }
    }
    
    private IEnumerator HealPlayer()
    {
        var initialPosition = transform.position;
        var targetPosition = Player.transform.position;
        var elapsedTime = 0f;

        while (elapsedTime < HealingTime)
        {
            Debug.Log("Healing Player");
            
            // Move the healing unit to the player.
            transform.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / HealingTime) * TravelSpeed);
            
            // Heal the player.
            Player.Health = Player.Health + HealingAmount * Time.deltaTime;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        // Reset the healing unit's position.
        elapsedTime = 0f;
        initialPosition = transform.position;
        targetPosition = BaseLocation.transform.position;
        
        while (elapsedTime < HealingTime)
        {
            Debug.Log("Returning to Base");
            
            // Move the healing unit to the base.
            transform.position = Vector3.Lerp(initialPosition, targetPosition, (elapsedTime / HealingTime) * TravelSpeed);
            
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }

        _isHealing = false;
        // Recharge the healing unit.
        StartCoroutine(Recharge());
    }

    private IEnumerator Recharge()
    {
        // Unsubscribe from the player's heal event.
        Player.PlayerData.HealEvent.RemoveListener(OnHealEvent.Invoke);

        var rechargeCounter = 0f;
        while (rechargeCounter < RechargeTime)
        {
            Debug.Log("Recharging");
            rechargeCounter += Time.deltaTime;
            yield return null;
        }
        
        // Re-subscribe to the player's heal event.
        Player.PlayerData.HealEvent.AddListener(OnHealEvent.Invoke);
        Debug.Log("Unit recharged, ready to heal again!");
    }
}
