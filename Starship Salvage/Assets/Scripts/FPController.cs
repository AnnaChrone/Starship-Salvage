using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering; //needed for all input in new input system
using System.Runtime.InteropServices;
public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public NPC Dialogue;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform gunPoint;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    private float originalMoveSpeed;

    [Header("PickUp")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Inventory")]
    public Hotbar hotbarSelector;

    [Header("Pause Menu")]
    public GameObject PauseMenu;
    public bool isPaused = false;

    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private SpaceshipFixing spaceship;

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    public float cursorSpeed = 1000f;
    private Vector2 cursorInput;
    private Vector2 cursorPosition;
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        originalMoveSpeed = moveSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        hotbarSelector.holdPoint = holdPoint;  // assign the camera holdPoint transform
        hotbarSelector.HandleScroll(0);        // force update so the first item shows correctly
    }
    private void Update()
    {
        HandleMovement();
        HandleLook();

        if (heldObject != null)
        {
            heldObject.MoveToHoldPoint(holdPoint.position);
        }

        UpdateCursorPosition();
    }

    private void Start()
    {
        // Initialize cursorPosition with current OS cursor position
        cursorPosition = new Vector2(Screen.width / 2, Screen.height / 2);
        SetCursorPos((int)cursorPosition.x, (int)cursorPosition.y);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!Dialogue.isFrozen)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else return;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!Dialogue.isFrozen)
        {
            lookInput = context.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward *
        moveInput.y;
        controller.Move(move * moveSpeed * Time.deltaTime);
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    public void HandleLook()
    {
        if (isPaused) return; // Don't rotate camera when paused
        float mouseX = lookInput.x * lookSensitivity;
        float mouseY = lookInput.y * lookSensitivity;
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit,
        verticalLookLimit);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(gunPoint.forward * 1000f);
            Destroy(bullet, 3);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.height = crouchHeight;
            moveSpeed = crouchSpeed;
        }
        else if (context.canceled)
        {
            controller.height = standHeight;
            moveSpeed = originalMoveSpeed;
        }
    }

    private GameObject FindFirstFreeSlot()
{
    foreach (GameObject slot in hotbarSelector.slots)
    {
        bool occupied = false;
        foreach (Transform child in slot.transform)
        {
            if (child.CompareTag("Pickup"))
            {
                occupied = true;
                break;
            }
        }
        if (!occupied)
            return slot;
    }
    return null;
}
    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            PickUpObject pickUp = hit.collider.GetComponent<PickUpObject>();
            if (pickUp != null)
            {
                int freeSlot = hotbarSelector.FindFirstFreeSlot();
                if (freeSlot < 0)
                {
                    Debug.Log("No free slots available");
                    return;
                }

                // Use the actual object from the scene
                GameObject item = pickUp.gameObject;

                // Tell it to follow the hold point
                pickUp.PickUp(hotbarSelector.holdPoint);

                // Store the object instance in the hotbar
                hotbarSelector.SetHeldItemInstance(freeSlot, item);

                // Store prefab reference (optional, for icon)
                hotbarSelector.StoreItemInSlot(freeSlot, pickUp.itemPrefab);

                // Update hotbar selection
                hotbarSelector.CurrentIndex = freeSlot;
                hotbarSelector.UpdateSelection();
            }
        }
    }







    public void OnScroll(InputAction.CallbackContext context)
    {
        float scrollValue = context.ReadValue<float>();
        if (scrollValue !=0)
        {
            hotbarSelector.HandleScroll(scrollValue);
        }
    }

    public void OnHotbarNext(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        hotbarSelector.HandleScroll(1f); // same as scrolling forward
    }

    public void OnHotbarPrev(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        hotbarSelector.HandleScroll(-1f); // same as scrolling backward
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        hotbarSelector.DropCurrentItem();
    }

    
    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        isPaused = !isPaused;

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseMenu.SetActive(true);
            Time.timeScale = 0f; 
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PauseMenu.SetActive(false);
            Time.timeScale = 1f; 
        }
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (spaceship != null) // only call if we’re in range
        {
            spaceship.TryUseItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SpaceshipFixing ship))
        {
            spaceship = ship;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Spaceship ship))
        {
            if (spaceship == ship)
                spaceship = null;
        }
    }

    public void OnControllerCursor(InputAction.CallbackContext context)
    {
        cursorInput = context.ReadValue<Vector2>();
    }

    private void UpdateCursorPosition()
    {
        if (cursorInput.sqrMagnitude < 0.01f) return; // deadzone

        cursorPosition += cursorInput * cursorSpeed * Time.deltaTime;

        // Clamp to screen
        cursorPosition.x = Mathf.Clamp(cursorPosition.x, 0, Screen.width);
        cursorPosition.y = Mathf.Clamp(cursorPosition.y, 0, Screen.height);

        // Move OS cursor
        SetCursorPos((int)cursorPosition.x, (int)(Screen.height - cursorPosition.y));
    }

    public void OnControllerClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Simulate left mouse button click
        ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject,
            new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
    }

}