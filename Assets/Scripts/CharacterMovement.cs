using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float drainStamina = 0.1f;
    public Vector3 gravity;
    public Vector3 playerVelocity;
    public bool groundedPlayer;
    public float mouseSensitivy = 5.0f;
    private float gravityValue = -9.81f;
    private CharacterController controller;
    private float walkSpeed = 5;
    private float runSpeed = 8;
    private bool isSprint = false;
    private bool canSprint = true;
    private Animator animator;

    public HUDManager hud;    
 
    private void Start()
    {
        hud = HUDManager.Instance;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InvokeRepeating("Sprint", 1.0f, drainStamina);
        InvokeRepeating("SprintRecharge", 1.0f, drainStamina);
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        ProcessMovement();
    }

    public void LateUpdate()
    {
        if (GameManager.Instance.ControlsEnabled == false)
        {
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        bool isMoving = Mathf.Ceil(Mathf.Abs(vertical) + Mathf.Abs(horizontal)) > 0;

        animator.SetFloat("Speed", (isMoving ? 1 : 0) * GetMovementSpeed() / runSpeed);
        animator.SetFloat("VerticalDirection", vertical);
    }

    void ShootLaser()
    {

    }

   void ProcessMovement()
    {
        if (GameManager.Instance.StaminaPercentage == 0){
            canSprint = false;
        }
        if(Input.GetButton("Fire3")){
            if(canSprint){
                isSprint = true;
            }
        }
        else{
            isSprint = false;
        }
        // Moving the character forward according to the speed
        float speed = GetMovementSpeed();

        // Get the camera's forward and right vectors
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // Make sure to flatten the vectors so that they don't contain any vertical component
        cameraForward.y = 0;
        cameraRight.y = 0;

        // Normalize the vectors to ensure consistent speed in all directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction based on input and camera orientation
        Vector3 moveDirection = (cameraForward * Input.GetAxis("Vertical")) + (cameraRight * Input.GetAxis("Horizontal"));

        // Apply the movement direction and speed
        Vector3 movement = moveDirection.normalized * speed * Time.deltaTime;
        
        if (GameManager.Instance.ControlsEnabled == false)
        {
            movement = Vector3.zero;
        }


        groundedPlayer = controller.isGrounded;
        if (groundedPlayer)
        {
            gravity.y = -1.0f;
        }
        else
        {
            // Since there is no physics applied on character controller we have this applies to reapply gravity
            gravity.y += gravityValue * Time.deltaTime;
        }
        // Apply gravity and move the character
        playerVelocity = gravity * Time.deltaTime + movement;
        controller.Move(playerVelocity);
    }

    public void Sprint(){
        if (GameManager.Instance.StaminaPercentage >= 0 && isSprint && (playerVelocity.x != 0 || playerVelocity.z != 0)){
            GameManager.Instance.StaminaPercentage -= 1;
        }
    }

    public void SprintRecharge(){
        if(!canSprint) {
            hud.SetSprintColor(new Color32(255, 0, 0, 255));
            if(GameManager.Instance.StaminaPercentage == 100){
                hud.SetSprintColor(new Color32(124, 180, 255, 255));
                canSprint = true;
            }
            else{
                GameManager.Instance.StaminaPercentage += 1;
            }
        }
    }

    float GetMovementSpeed()
    {
        Debug.Log("Is Sprint: " + isSprint);
        if (isSprint)// Left shift
        {
            return runSpeed;
        }
        else
        {
            return walkSpeed;
        }
    }
}
