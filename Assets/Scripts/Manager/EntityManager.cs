using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages entities in the game.
/// </summary>
public class EntityManager : Singleton<EntityManager>
{
    private PlayerController GetPlayerController()
    {
        // Get the player controller.
        return FindObjectOfType<PlayerController>();
    }
}
