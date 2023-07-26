using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int _regenAmount = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        
        // Regenerate the player's health consumable.
        var playerController = other.GetComponent<PlayerController>();
        playerController.SetHealthConsumable(playerController.GetHealthConsumable() + _regenAmount);
        
        // Destroy once collected
        Destroy(gameObject);
    }

    private void Update()
    {
        // Rotate the object to make it look cool.
        transform.Rotate(new Vector3(0f, 1f));
    }
}
