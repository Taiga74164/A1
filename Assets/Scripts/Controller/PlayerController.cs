using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ILiving
{
    #region Player Components
    
    [Header("Player Components")]
    public Transform MainCamera;
    public CharacterController Controller;
    public PlayerAttributeHolder PlayerAttributes;
    public WorldController World;
    
    #endregion
    
    #region Player Settings
    
    [Header("Player Settings")]
    public float JumpHeight = 1.2f;
    private readonly float _gravity = Physics.gravity.y; 
    public float RotationSpeed = 10f;
    public float MoveSpeed = 5.0f;

    #endregion
    
    #region Input Actions
    
    private InputAction _movement, _jump, _attack, _consume;
    
    #endregion
    
    #region Properties
    
    private Quaternion _targetRotation = Quaternion.identity;
    private Vector3 _velocity = Vector3.zero;
    private Vector2 _moveInput;
    private bool _isMoving, _canMove = true;
    private float _rotationTime = 0.0f;

    public AttributeHolder GetAttributes() => PlayerAttributes;

    public float GetMaxHealth() => Mathf.Floor((float) PlayerAttributes.GetAttribute(Attributes.MaxHealthAttributes).GetValue());

    public float GetHealth() => Mathf.Floor((float) PlayerAttributes.GetAttribute(Attributes.HealthAttributes).GetValue());

    public void SetHealth(float value)
    {
        // Set the player's health to the max health if the value is greater than the max health.
        if (value >= GetMaxHealth())
        {
            PlayerAttributes.GetAttribute(Attributes.HealthAttributes).SetValue(GetMaxHealth());
            return;
        }

        PlayerAttributes.GetAttribute(Attributes.HealthAttributes).SetValue(value);
    }

    public float GetHealthConsumable() => Mathf.Floor((float) PlayerAttributes.GetAttribute(Attributes.HealthConsumableAttributes).GetValue());
    
    public void SetHealthConsumable(float value) => PlayerAttributes.GetAttribute(Attributes.HealthConsumableAttributes).SetValue(value);
    
    #endregion

    #region Particle System
    
    [Header("Particle System")]
    public ParticleSystem DamageParticles;
    public ParticleSystem HealParticles;
    
    #endregion
    
    #region Audio Source
    
    [Header("Audio Source")]
    public AudioSource DamageAudio;
    public AudioSource HealAudio;
    
    #endregion

    #region Data

    [Header("Player Data")]
    public PlayerData PlayerData;

    public UnityEvent OnDamage;
    public UnityEvent OnHeal;
    
    #endregion
    
    private void Start()
    {
        // Setup input action.
        _movement = InputManager.Move;
        _jump = InputManager.Jump;
        _attack = InputManager.Attack;
        _consume = InputManager.Consume;
        
        // Listen for input events.
        _jump.started += Jump;
        _attack.performed += Attack;
        _consume.performed += Consume;
        
        // Subscribe to player events.
        // PlayerData.DamageEvent += Damage;
        // PlayerData.HealEvent += Heal;
        PlayerData.DamageEvent.AddListener(OnDamage.Invoke);
        PlayerData.HealEvent.AddListener(OnHeal.Invoke);
    }

    private void Update()
    {
        // Player movement.
        UpdatePosition();
        HandleVelocity();
        HandleRotation();
        HandlePlayerHealth();
        
        //Debug.Log($"{GetHealth()}, {GetMaxHealth()}");
    }
    
    /// <summary>
    /// Handles the player's position.
    /// </summary>
    private void UpdatePosition()
    {
        // Check if the player can move.
        if (!_canMove)
            return;
        
        // Get the input values.
        _moveInput = _movement.ReadValue<Vector2>();
        // Update the player's state.
        _isMoving = _moveInput != Vector2.zero;
        if (Controller.isGrounded && _velocity.y < 0)
            _velocity.y = 0f;
        
        // Calculate a target position & direction.
        var position = new Vector3(_moveInput.x, 0, _moveInput.y);
        // Adjust for camera rotation.
        position = MainCamera.forward * position.z + MainCamera.right * position.x;
        position.y = 0;
        
        // Move the character controller.
        Controller.Move(position * (Time.deltaTime * MoveSpeed));
    }
    
    /// <summary>
    /// Handles the player's velocity.
    /// </summary>
    private void HandleVelocity()
    {
        // Apply gravity to the velocity.
        _velocity.y += _gravity * Time.deltaTime;
        // Apply drag/friction to the velocity.
        _velocity.x *= 0.9f;
        _velocity.z *= 0.9f;
        // Move the controller in the direction of velocity.
        Controller.Move(_velocity * Time.deltaTime);
    }
    
    /// <summary>
    /// Handles the player's rotation.
    /// </summary>
    private void HandleRotation()
    {
        // Check if the player has a target rotation.
        if (_targetRotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                _targetRotation, Time.deltaTime * RotationSpeed);
            _rotationTime += 0.3f + RotationSpeed * Time.deltaTime;
            
            // Check if the player has reached the target rotation.
            if (_rotationTime >= RotationSpeed)
            {
                _canMove = true;
                _rotationTime = 0.0f;
                _targetRotation = Quaternion.identity;
            }
            
            return;
        }
        
        // Check if the player is moving.
        if (!_isMoving)
            return;
        
        // Calculate rotation.
        var angle = Mathf.Atan2(_moveInput.x, _moveInput.y) *
            Mathf.Rad2Deg + MainCamera.eulerAngles.y;
        var rotation = Quaternion.Euler(0f, angle, 0f);
        // Rotate the player transform.
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
    }

    /// <summary>
    /// Handles the player's health.
    /// </summary>
    private void HandlePlayerHealth()
    {
        Mathf.Clamp(GetHealth(), 0, GetMaxHealth());
        if (GetHealth() > 0) 
            return;
        
        Debug.Log("Player died. Restarting game.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (Controller.isGrounded)
            _velocity.y += Mathf.Sqrt(JumpHeight * -2f * _gravity);
    }
    
    private void Attack(InputAction.CallbackContext context)
    {
        //Debug.Log("Attack performed");
        if (context.phase == InputActionPhase.Performed)
            PlayerData.Damage();
    }
    
    private void Consume(InputAction.CallbackContext context)
    {
        //Debug.Log("Consume Item performed");
        if (context.phase == InputActionPhase.Performed)
            PlayerData.Heal();
    }
    
    #region Interface

    public void Damage(/*int value*/)
    {
        // Get the calculated health value.
        var newHealth = GetHealth() - PlayerData.DamageDealt;
        SetHealth(newHealth);

        // Display Damage UI.
        World.DisplayDamage(transform, PlayerData.DamageDealt);
        
        // Spawn particles and play audio.
        Instantiate(DamageParticles, transform.position, Quaternion.identity);
        DamageAudio.Play();
    }
    
    //void Damage(int value, Vector3 direction)
    
    public void Heal(/*int value*/)
    {
        // Check if consumable is available.
        if (GetHealthConsumable() <= 0)
            return;
        
        // Check if the player is at max health.
        if (GetHealth() >= GetMaxHealth())
            return;
        
        // Get the calculated health value.
        var newHealth = GetHealth() + PlayerData.HealAmount;
        SetHealth(newHealth > GetMaxHealth() ? GetMaxHealth() : newHealth);
        
        // Set the new consumable value.
        var newConsumable = PlayerData.HealAmount * 10.0f;
        SetHealthConsumable(GetHealthConsumable() - newConsumable);
        
        // Spawn particles and play audio.
        Instantiate(HealParticles, transform, false);
        HealAudio.Play();
        
    }

    #endregion
}
