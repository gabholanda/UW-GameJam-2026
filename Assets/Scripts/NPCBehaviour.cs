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
    public string dialogueText;
    public string NPCName;
    public string NPCSobriet;

    [SerializeField] private TMPro.TextMeshPro textObject;
    [SerializeField] private TMPro.TextMeshPro interactObject;
    [SerializeField] private TMPro.TextMeshPro executeObject;

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

        textObject.enabled = false;
        interactObject.enabled = false;
        executeObject.enabled = false;
    }

    public void Initialize(NPCData npcData)
    {
        textObject.text = npcData.dialogueContainer.dialogue;
        NPCName = npcData.targetName;
        NPCSobriet = npcData.targetSobriet;
    }

    public void SetFakeClue(string fakeClue)
    {
        textObject.text = fakeClue;
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
        if (!collision.CompareTag("Player")) return;

        PlayerController playerController = collision.GetComponent<PlayerController>();
        playerController.SetCurrentGuest(this);

        if (isAlreadyTalkedWith)
        {
            executeObject.enabled = true;
            return;
        }
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
        executeObject.enabled = false;
    }
}