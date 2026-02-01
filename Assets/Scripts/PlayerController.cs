using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputAsset;

    [SerializeField]
    private CinemachineCamera cineCamera;

    private Vector2 direction = Vector2.zero;

    [SerializeField]
    private Rigidbody2D rb;


    [SerializeField]
    private float speed = 3.0f;

    private NPCBehaviour currentGuest = null;

    public bool canMove = true;

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
        if (!currentGuest)
        {
            return;
        }

        if (currentGuest.isAlreadyTalkedWith)
        {
            currentGuest.CleanUpDialogue();
            currentGuest.EndTalk();
            currentGuest = null;
            cineCamera.Target.TrackingTarget = transform;
            StartCoroutine(StartZoom(100, 20));
            canMove = true;
            return;
        }
        cineCamera.Target.TrackingTarget = currentGuest.transform;
        StartCoroutine(StartZoom(500, 10));
        canMove = false;
        currentGuest.Talk();
    }

    public IEnumerator StartZoom(int value, int smooth)
    {
        PixelPerfectCamera PPC = Camera.main.GetComponent<PixelPerfectCamera>();
        while (PPC.assetsPPU != value)
        {
            PPC.assetsPPU = Mathf.Clamp(PPC.assetsPPU + smooth, 0, value);
            yield return null;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!canMove)
        {
            return;
        }
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
