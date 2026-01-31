using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputAsset;

    private Vector2 direction = Vector2.zero;

    [SerializeField]
    private Rigidbody2D rb;


    [SerializeField]
    private float speed = 3.0f;

    private NPCBehaviour currentGuest = null;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        InputAction moveAction = inputAsset.FindAction("Move");
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        InputAction interactAction = inputAsset.FindAction("Interact");

        interactAction.started += OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if(!currentGuest)
        {
            return;
        }

        currentGuest.Talk();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
       direction = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = speed * direction;
    }

    public void SetCurrentGuest(NPCBehaviour currentGuest_)
    {
        currentGuest = currentGuest_;
    }


    public NPCBehaviour GetCurrentGuest()
    {
        return currentGuest;
    }

    private void OnDisable()
    {
        InputAction moveAction = inputAsset.FindAction("Move");
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        InputAction interactAction = inputAsset.FindAction("Interact");

        interactAction.started -= OnInteract;
    }
}
