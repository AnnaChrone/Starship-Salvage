using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering; //needed for all input in new input system
using System.Runtime.InteropServices;
using System.Collections;
using TMPro;
public class FPController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float runSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("NPCs")]
    public NPC Zorb;
    public NPC CoLuPres;
    public NPC RaLuPres;
    public NPC LuLuPres;
    public NPC Zinnia;
    public NPC CoLu;
    public NPC RaLu;
    public NPC LuLu;
    public NPC MinLu;

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float lookSensitivity = 2f;
    public float verticalLookLimit = 90f;

    [Header("Crouch")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;
    private float originalMoveSpeed;

    [Header("Grow")]
    public float growHeight = 10f;
    public float growSpeed = 20f;
    public AudioSource grow;
    public AudioSource shrink;


    [Header("PickUp")]
    public float pickupRange = 3f;
    public Transform holdPoint;
    private PickUpObject heldObject;

    [Header("Inventory")]
    public Hotbar hotbarSelector;


    [Header("Pause Menu")]
    public GameObject PauseMenu;
    public bool isPaused = false;
    public AudioSource Select;
    public AudioSource Deselect;
    public RectTransform PMenu;

    [Header("Map")]
    public RectTransform Map;
    public bool mapOpen = false;
    private Coroutine slideRoutineMap;
    public float mapHiddenX = 2450f;
    public float mapVisibleX = 1472f;

    [Header("Slide Settings")]
    public float slideSpeed = 1000f;
    private Coroutine slideRoutinePause;
    public float pauseHiddenY = 900f;      // offscreen (above)
    public float pauseVisibleY = 0f;       // fully visible

    [Header("Landing Particles")]
    public ParticleSystem landingParticles;
    [SerializeField] private Transform feet; // at player's feet

    [Header("Floating")]
    public float floatDuration = 10f;   // how long to float
    public float floatSpeed = 3f;      // upward speed
    private bool wasGrounded;
    public TextMeshProUGUI Count;
    public PlayerAbilities fruits;
    public GameObject FloatDisplay;
    public AudioSource Floating;

    [Header("Footsteps")]
    public AudioSource Footsteps;


    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;


    private CharacterController controller;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private SpaceshipFixing spaceship;
    private bool Freeze;
    public bool animated = false;

    [Header("Table Minigame")]
    public Table Table;
    public GameObject TableGameobject;
    public GameObject Minigame;

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


        if (Minigame.activeInHierarchy || RaLuPres.isFrozen || MinLu.isFrozen || LuLuPres.isFrozen || CoLuPres.isFrozen || Zinnia.isFrozen || Zorb.isFrozen || CoLu.isFrozen || LuLu.isFrozen || RaLu.isFrozen || isPaused)
        {
            Freeze = true;
        }
        else
        {
            Freeze = false;
        }

        if (Freeze)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            PickUpObject cursor = hit.collider.GetComponent<PickUpObject>();
            if (cursor != null && animated == false)
            {
                //logic to update cursor
                Debug.Log("i will be animated!");
                animated = true;
            }

            if (cursor == null && animated == true)
            {
                animated = false;
                Debug.Log("im not animated anymore");
                
            }
        }
    }
    private bool IsGrounded()
    {
        return Physics.Raycast(feet.position, Vector3.down, groundCheckDistance, groundMask);
    }


    public void OnMovement(InputAction.CallbackContext context)
    {
        if (!Freeze)
        {
            moveInput = context.ReadValue<Vector2>();
        }
        else return;
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!Freeze)
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

    private void OnLand()
    {
        if (landingParticles != null)
        {
            Debug.Log("triggering particles");
            landingParticles.Play();
        }
    }
    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        if (grown)
        {
            controller.Move(move * growSpeed * Time.deltaTime);

        } else
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }


        bool isGrounded = IsGrounded();

        if (!wasGrounded && isGrounded)
            OnLand();
        wasGrounded = isGrounded;

        // Gravity
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        
        bool isMoving = moveInput.magnitude > 0.1f;  // player is pressing WASD/analog stick

        if (isGrounded && isMoving && velocity.y <= 0 && gravity < 0)
        {
            if (!Footsteps.isPlaying)
            {
                Footsteps.loop = true;
                Footsteps.Play();
            }
            Footsteps.pitch = moveSpeed > originalMoveSpeed ? 1.5f : 1f;
        }
        else
        {
            if (Footsteps.isPlaying)
            {
                Footsteps.Stop();
            }
        }

    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (Freeze) return;

        if (context.performed) // double-tap W
        {
            moveSpeed = runSpeed;
            Debug.Log("Double-tap detected Running!");
        }
        else if (context.canceled) // when W released
        {
            moveSpeed = originalMoveSpeed;
            Debug.Log("Stopped running");
        }
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

        if (slideRoutinePause != null)
            StopCoroutine(slideRoutinePause);

        if (isPaused)
        {
            Select.Play();
            slideRoutinePause = StartCoroutine(SlidePause(pauseVisibleY));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            Deselect.Play();
            slideRoutinePause = StartCoroutine(SlidePause(pauseHiddenY));
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    private IEnumerator SlidePause(float targetY)
    {
        Vector2 pos = PMenu.anchoredPosition;

        while (Mathf.Abs(pos.y - targetY) > 0.1f)
        {
            pos.y = Mathf.MoveTowards(pos.y, targetY, slideSpeed * Time.unscaledDeltaTime);
            PMenu.anchoredPosition = pos;
            yield return null;
        }

        pos.y = targetY;
        PMenu.anchoredPosition = pos;
    }

    public void OnMap(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        mapOpen = !mapOpen;

        if (slideRoutineMap != null)
            StopCoroutine(slideRoutineMap);

        if (mapOpen)
        {
            slideRoutineMap = StartCoroutine(SlideMap(mapVisibleX));
        }
        else
        {
            slideRoutineMap = StartCoroutine(SlideMap(mapHiddenX));
        }
    }

    private IEnumerator SlideMap(float targetX)
    {
        Vector2 pos = Map.anchoredPosition;

        while (Mathf.Abs(pos.x - targetX) > 0.1f)
        {
            pos.x = Mathf.MoveTowards(pos.x, targetX, slideSpeed * Time.unscaledDeltaTime);
            Map.anchoredPosition = pos;
            yield return null;
        }

        pos.x = targetX;
        Map.anchoredPosition = pos;
    }
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hotbarSelector != null)
            {
                if (!hotbarSelector.TryUseSelectedItem(gameObject))
                    Debug.Log("No usable item in hand!");
            }
        }
    }

    public void OnFloat(InputAction.CallbackContext context)
    {
        if (Freeze) return;

        if (context.performed && fruits.FloatAquired) // double-tap space
        {
            FloatDisplay.SetActive(true);
            Debug.Log("Double-tap SPACE Floating!");
            Floating.Play();
            StartCoroutine(FloatUpwards());
            StartCoroutine(Countdown());
           

        }
    }

    private IEnumerator Countdown()
    {
        for (int i = 5; i >= 0; i--)
        {
            Count.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        FloatDisplay.SetActive(false);

    }

    private IEnumerator FloatUpwards()
    {
        float timer = 0f;
        gravity = 0f;

        while (timer < floatDuration)
        {
            controller.Move(Vector3.up * floatSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; // wait one frame
        }

        Debug.Log("falling!");
        gravity = -9.81f;
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

    private bool grown = false;
    public void OnGrow(InputAction.CallbackContext context)
    {
        if (Freeze) return;
        if (context.performed)
        {
            grown = !grown;
        }
        if (grown && fruits.GrowAquired)
        {
            Debug.Log("Growing");
            transform.localScale = Vector3.one * growHeight;
            grow.Play();


            moveSpeed = growSpeed;
        }
        else
        {
            // Reset scale
            transform.localScale = Vector3.one;
            shrink.Play();
            Debug.Log("Shrinking");

            moveSpeed = originalMoveSpeed;
        }
    }

    private bool Bouquet =false;
    public void OnBouquet(InputAction.CallbackContext context)
    {
        if (Freeze) return;
        if (!context.performed) return;


        if (Table.RangeTable)
        {
            Bouquet = !Bouquet;
            if (Bouquet)
            {
                TableGameobject.SetActive(true);
                

            }
            else
            {
                TableGameobject.SetActive(false);

            }
        }
    }
}