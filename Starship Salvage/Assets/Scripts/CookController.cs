using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
//using Unity.SharpZipLib.BZip2;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering; //needed for all input in new input system
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using Cursor = UnityEngine.Cursor;
using Image = UnityEngine.UI.Image;
public class CookController : MonoBehaviour
{
    [Header("Movement Settings")]
    public GameObject Player;
    public float moveSpeed = 5f;

  
    private CharacterController controller;
    private Vector2 moveInput;

    private Vector3 velocity;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }
    private void Update()
    {
        HandleMovement();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
            moveInput = context.ReadValue<Vector2>();

    }
    public void HandleMovement()
    {
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        
            controller.Move(move * moveSpeed * Time.deltaTime);
        


     //   velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isMoving = moveInput.magnitude > 0.1f;

       
    }

}