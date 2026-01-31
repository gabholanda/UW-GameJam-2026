using UnityEngine;

public class NPCBehaviour : MonoBehaviour
{
    public DialogueContainer dialogueContainer;

    [SerializeField]
    private TMPro.TextMeshPro textObject;

    [SerializeField]
    private TMPro.TextMeshPro interactObject;

    private bool isAlreadyTalkedWith = false;

    private void Awake()
    {
        textObject.text = dialogueContainer.dialogue;
        textObject.enabled = false;

        interactObject.enabled = false;
    }

    public void Talk()
    {
        interactObject.enabled = false;

        textObject.enabled = true;
        isAlreadyTalkedWith = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAlreadyTalkedWith)
        {
            return;
        }

        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        playerController.SetCurrentGuest(this);

        interactObject.enabled = true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController.GetCurrentGuest() != this)
        {
            return;
        }

        playerController.SetCurrentGuest(null);
        interactObject.enabled = false;
        textObject.enabled = false;
    }
}
