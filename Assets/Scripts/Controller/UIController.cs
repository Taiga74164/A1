using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class UIController : MonoBehaviour
{
    public PlayerAttributeHolder AttributeHolder;
    public CinemachineInputProvider CameraInputProvider;
    private InputAction _showCursor;


    #region Unity Events

    private void Start()
    {
        // Setup input action references.
        _showCursor = InputManager.Cursor;
        
        // Initialize the UI.
        CaptureCursor();
        InitializeHealthUI();
        InitializeHealthConsumableUI();
    }
    
    private void Update()
    {
        UpdateCursorStatus();
        UpdateHealthUI();
        UpdateHealthConsumableUI();
    }

    #endregion

    #region Cursor Control

    private void CaptureCursor()
    {
        // Lock the cursor to the game.
        Cursor.lockState = CursorLockMode.Locked;
        // Hide the cursor from view.
        Cursor.visible = false;
    }
    
    private void UpdateCursorStatus()
    {
        // Check the cursor input action or if the game is paused.
        var cursor = _showCursor.ReadValue<float>() > 0;
        
        // Update the cursor properties.
        Cursor.visible = cursor;
        Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        
        // Update the camera input provider.
        CameraInputProvider.enabled = !cursor;
    }

    #endregion

    #region Health UI

    [Header("Health Bar")]
    public TMP_Text HealthText;
    public Image HealthBarForeground;
    public Image HealthBarCritical;

    // Assigned on initialization.
    private MaxHealthAttribute _maxHealthAttribute;
    private HealthAttribute _healthAttribute;
    
    private void InitializeHealthUI()
    {
        // Get the player attributes.
        _maxHealthAttribute = (MaxHealthAttribute) AttributeHolder.GetAttribute(Attributes.MaxHealthAttributes);
        _healthAttribute = (HealthAttribute) AttributeHolder.GetAttribute(Attributes.HealthAttributes);
    }

    private void UpdateHealthUI()
    {
        // Get the values of the attributes.
        var health = Mathf.Floor((float) _healthAttribute.GetValue());
        var maxHealth = Mathf.Floor((float) _maxHealthAttribute.GetValue());
        if (health > maxHealth) health = maxHealth;
        if (health < 0) health = 0;

        // Update the health text.
        HealthText.text = $"{health} / {maxHealth}";

        // Check which bar to display.
        if (health / maxHealth < 0.3f)
        {
            // Health is "critical".
            HealthBarForeground.gameObject.SetActive(false);
            HealthBarCritical.gameObject.SetActive(true);

            // Update the critical bar.
            HealthBarCritical.fillAmount = health / maxHealth;
        }
        else
        {
            // Health is "normal".
            HealthBarForeground.gameObject.SetActive(true);
            HealthBarCritical.gameObject.SetActive(false);

            // Update the normal bar.
            HealthBarForeground.fillAmount = health / maxHealth;
        }
    }

    #endregion
    
    #region Health Consumabnle

    [Header("Health Consumable")]
    public Image HealthConsumableImage;

    // Assigned on initialization.
    private HealthConsumableAttribute _healthConsumableAttribute;
    
    private void InitializeHealthConsumableUI()
    {
        // Get the player attributes.
        _healthConsumableAttribute = (HealthConsumableAttribute) AttributeHolder.GetAttribute(Attributes.HealthConsumableAttributes);
    }

    private void UpdateHealthConsumableUI()
    {
        // Get the values of the attributes.
        var healthConsumable = Mathf.Floor((float) _healthConsumableAttribute.GetValue());
        if (healthConsumable < 0) healthConsumable = 0;
        
        // Fill the image.
        HealthConsumableImage.fillAmount = healthConsumable / 100;
    }

    #endregion
}
