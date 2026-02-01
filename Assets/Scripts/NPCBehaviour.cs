using UnityEngine;

public enum NPCStates
{
    Idle,
    Dancing,
    Walking,
    Talking
}

public class NPCBehaviour : MonoBehaviour
{
    public DialogueContainer dialogueContainer;

    [SerializeField] private TMPro.TextMeshPro textObject;
    [SerializeField] private TMPro.TextMeshPro interactObject;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float walkDuration = 2f;
    [SerializeField] private float idleDuration = 2f;
    [SerializeField] private float danceDuration = 3f;

    private Rigidbody2D rb;
    private Vector2 walkDirection;
    private float stateTimer;

    public bool isAlreadyTalkedWith = false;

    public NPCStates currentState = NPCStates.Idle;
    private NPCStates lastStateBeforeTalking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        textObject.text = dialogueContainer.dialogue;
        textObject.enabled = false;
        interactObject.enabled = false;
    }

    private void Start()
    {
        PickNewState();
    }

    private void Update()
    {
        switch (currentState)
        {
            case NPCStates.Walking:
                rb.linearVelocity = walkDirection * walkSpeed;
                break;

            case NPCStates.Idle:
            case NPCStates.Dancing:
            case NPCStates.Talking:
                rb.linearVelocity = Vector2.zero;
                break;
        }

        // Countdown state timer (unless talking)
        if (currentState != NPCStates.Talking)
        {
            stateTimer -= Time.deltaTime;

            if (stateTimer <= 0f)
            {
                PickNewState();
            }
        }
    }

    private void PickNewState()
    {
        // Randomly choose between Walking, Idle, or Dancing
        int choice = Random.Range(0, 3);

        switch (choice)
        {
            case 0:
                currentState = NPCStates.Walking;
                walkDirection = Random.insideUnitCircle.normalized;
                stateTimer = walkDuration;
                break;

            case 1:
                currentState = NPCStates.Idle;
                stateTimer = idleDuration;
                break;

            case 2:
                currentState = NPCStates.Dancing;
                stateTimer = danceDuration;
                break;
        }
    }

    public void Talk()
    {
        interactObject.enabled = false;
        textObject.enabled = true;
        isAlreadyTalkedWith = true;

        // Save what the NPC was doing before talking
        lastStateBeforeTalking = currentState;
        currentState = NPCStates.Talking;
    }

    public void EndTalk()
    {
        textObject.enabled = false;

        // Resume what the NPC was doing before
        currentState = lastStateBeforeTalking;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlreadyTalkedWith) return;
        if (!collision.CompareTag("Player")) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        playerController.SetCurrentGuest(this);

        interactObject.enabled = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController.GetCurrentGuest() != this) return;

        playerController.SetCurrentGuest(null);
        CleanUpDialogue();
    }

    public void CleanUpDialogue()
    {
        interactObject.enabled = false;
        textObject.enabled = false;
    }
}