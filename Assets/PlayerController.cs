using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputManager inputManager;
    private CharacterController characterController;
    private Animator animator;

    [SerializeField] private float speed = 1f;
    [Range(1, 4)] public float movementSpeed = 2;
    private float shiftVal = 1f;
    private float turnDirection;
    private Vector3 motionVector;

    private Vector3 relativeVector;

    private bool cursorLocked;

    public GameObject focusPoint;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        motionVector = transform.right * (inputManager.horizontal) + transform.forward * inputManager.vertical;
        if (inputManager.vertical != 0 || inputManager.horizontal != 0)
        {
            characterController.Move(motionVector * movementSpeed * shiftVal * Time.deltaTime);
            transform.Rotate(transform.up * turnDirection * 400 * Time.deltaTime);
            focusPoint.transform.parent.Rotate(transform.up * -turnDirection * 400 * Time.deltaTime);
        }

        Animations();
        MouseLook();
        
    }

    void Animations()
    {
        animator.SetFloat("vertical", inputManager.vertical * shiftVal);
        animator.SetFloat("horizontal", (inputManager.vertical != 0 ? inputManager.horizontal / 2 : inputManager.horizontal) * shiftVal);
    }

    void MouseLook()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) cursorLocked = cursorLocked ? false : true;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        if (cursorLocked)
        {
            relativeVector = transform.InverseTransformPoint(focusPoint.transform.position);
            relativeVector /= relativeVector.magnitude;

            turnDirection = (relativeVector.x / relativeVector.magnitude);

            //vertical
            focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + -Input.GetAxis("Mouse Y"), focusPoint.transform.eulerAngles.y, 0);
            //horizontal
            focusPoint.transform.parent.Rotate(transform.up * Input.GetAxis("Mouse X") * 100 * Time.deltaTime);
        }
    }
}
