using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CardResetter : MonoBehaviour
{
    [Header("Keycard & Socket")]
    public GameObject accessCard;
    public XRSocketInteractor socketInteractor;

    [Header("Visuals")]
    public MeshRenderer ledRenderer;
    public Material redLedMaterial;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody cardRb;

    private void Start()
    {
        if (accessCard != null)
        {
            initialPosition = accessCard.transform.position;
            initialRotation = accessCard.transform.rotation;
            cardRb = accessCard.GetComponent<Rigidbody>();
        }
    }

    public void ResetKeycardAndSocket()
    {
        // 1. Force the socket to release the card and trigger its exit events
        if (socketInteractor != null)
        {
            socketInteractor.socketActive = false;
            socketInteractor.enabled = false;
        }

        // 2. Teleport card back to original desk position
        if (accessCard != null)
        {
            if (cardRb != null)
            {
                cardRb.velocity = Vector3.zero;
                cardRb.angularVelocity = Vector3.zero;
            }

            accessCard.transform.position = initialPosition;
            accessCard.transform.rotation = initialRotation;
        }

        // 3. Reset the LED back to red
        if (ledRenderer != null && redLedMaterial != null)
        {
            ledRenderer.material = redLedMaterial;
        }

        // 4. Re-enable the socket so it is ready to receive the card again
        if (socketInteractor != null)
        {
            Invoke(nameof(ReenableSocket), 0.1f);
        }
    }

    private void ReenableSocket()
    {
        if (socketInteractor != null)
        {
            socketInteractor.enabled = true;
            socketInteractor.socketActive = true;
        }
    }
}