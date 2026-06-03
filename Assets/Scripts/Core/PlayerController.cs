using UnityEngine;
using Fusion;
using Unity.VisualScripting;

public enum InputButtons
{
    Jump,
    Interact
}
public class PlayerController : NetworkBehaviour
{
    private NetworkButtons previousButtons;

    [SerializableType]private CharacterController controller;
    public float speed;

    bool interact;
    ///////////CAMERA///////////
    public Camera myCamera;
    public float mouseSense;
    float xRotation = 0f;
    public Transform cameraPivot;

    /////////////JUMP/////////////
    private Vector3 velocity;
    private bool isJumping;
    float gravity = -9.81f;
    public float jumpForce;

    IInteractuable currentInteractuable;
    private void Awake()
    {
           controller = GetComponent<CharacterController>();
           myCamera=GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            interact=true;
        }
    }
    public override void Spawned()
    {
        bool isLocalPlayer = Object.HasInputAuthority;

        myCamera.gameObject.SetActive(isLocalPlayer);

        if (isLocalPlayer) { 
            Cursor.lockState= CursorLockMode.Locked;
            Cursor.visible= false;        
        }

        AudioListener listener = myCamera.GetComponent<AudioListener>();
        if (listener != null) listener.enabled = isLocalPlayer;
    }
    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out NetworkInputData inputData))
            return;

        if (controller.isGrounded)
            velocity = new Vector3(0, -1, 0);

        NetworkButtons buttons = inputData.Buttons;

        #region Camera Logic

        float mouseX = inputData.Look.x * mouseSense * Runner.DeltaTime;
        float mouseY = inputData.Look.y * mouseSense * Runner.DeltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);

        #endregion

        #region Interaction Logic

        LayerMask interactableLayer = LayerMask.GetMask("Interactable");

        Ray ray = new Ray(
            myCamera.transform.position,
            myCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 15f, interactableLayer))
        {
            if (hit.collider.TryGetComponent<IInteractuable>(out var interactuable))
            {
                if (currentInteractuable != interactuable)
                {
                    currentInteractuable?.UnHighlight();

                    currentInteractuable = interactuable;
                    currentInteractuable.Highlight();
                }

                if (buttons.WasPressed(previousButtons, InputButtons.Interact))
                {
                    currentInteractuable.Interact(this);
                }
            }
            else
            {
                currentInteractuable?.UnHighlight();
                currentInteractuable = null;
            }
        }
        else
        {
            currentInteractuable?.UnHighlight();
            currentInteractuable = null;
        }

        #endregion

        #region Movement

        Vector3 forward = cameraPivot.forward;
        Vector3 right = cameraPivot.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement =
        (
            forward * inputData.Move.y +
            right * inputData.Move.x
        ) * speed * Runner.DeltaTime;

        velocity.y += gravity * Runner.DeltaTime;

        if (buttons.WasPressed(previousButtons, InputButtons.Jump)
            && controller.isGrounded)
        {
            velocity.y += jumpForce;
        }

        controller.Move(movement + velocity * Runner.DeltaTime);

        #endregion

        previousButtons = buttons;
    }
}
