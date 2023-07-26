using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages inputs globally for the game.
/// </summary>
public class InputManager : MonoBehaviour
{
    private static InputActions _actions;

    // Player actions.
    public static InputAction Move;
    public static InputAction Jump;
    public static InputAction Attack;
    public static InputAction Consume;

    // Interface actions.
    public static InputAction Cursor;

    private void Awake()
    {
        // Create the input actions asset.
        _actions = new InputActions();

        // Update the player actions.
        Move = _actions.Player.Movement;
        Jump = _actions.Player.Jump;
        Attack = _actions.Player.Attack;
        Consume = _actions.Player.Consume;
        // Update the interface actions.
        Cursor = _actions.Interface.ShowCursor;
    }

    #region Boilerplate

    private void OnEnable()
    {
        // Enable the input actions.
        _actions.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions.
        _actions.Disable();
    }

    #endregion
}