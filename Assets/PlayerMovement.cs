using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public PlayerGun playerGun; // Reference to the PlayerGun script
    public float baseSpeed;
    public float jumpPower;
    public float lookSpeed;
    public float lookXLimit;

    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX;
    private CharacterController _characterController;
    private bool _isGrounded;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Ensure PlayerGun is assigned
        if (playerGun == null)
        {
            playerGun = GetComponentInChildren<PlayerGun>();
            if (playerGun == null)
            {
                Debug.LogError("PlayerGun script is not assigned to the PlayerMovement component.");
            }
        }
    }

    void FixedUpdate()
    {
        _isGrounded = _characterController.isGrounded;

        // Movement logic
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = baseSpeed * Input.GetAxis("Vertical");
        float curSpeedY = baseSpeed * Input.GetAxis("Horizontal");

        Vector3 horizontalMovement = (forward * curSpeedX) + (right * curSpeedY);

        // Apply jump logic
        if (_isGrounded)
        {
            _moveDirection.y = Input.GetButton("Jump") ? jumpPower : // Jumping upwards
                0f; // Reset vertical velocity if grounded
        }
        else
        {
            _moveDirection.y -= 9.81f * Time.fixedDeltaTime; // Apply gravity
        }

        // Combine horizontal and vertical movement
        Vector3 finalMovement = horizontalMovement + Vector3.up * _moveDirection.y;

        // Move the character
        _characterController.Move(finalMovement * Time.fixedDeltaTime);

        // Camera rotation
        _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        // Shooting logic
        if (Input.GetButton("Fire1") && playerGun != null)
        {
            playerGun.Shoot();
        }
    }
    
    public void OnPlayerDeath()
    {
        // Pause the game
        Time.timeScale = 0f; // Pause game time
    
        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None; // Unlock cursor
        Cursor.visible = true; // Make the cursor visible

        // Show the death screen via UIManager
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowDeathScreen();
        }
        else
        {
            Debug.LogError("UIManager instance is not found! Ensure there is one active UIManager in the scene.");
        }
    }

    public void Respawn()
    {
        // Reset player position and state
        transform.position = Vector3.zero;
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor for gameplay
        Cursor.visible = false;

        // Hide death screen (handled in UI Manager)
        UIManager.Instance.HideDeathScreen();
    }
}