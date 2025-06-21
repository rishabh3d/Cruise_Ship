using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float fastSpeedMultiplier = 2f;
    public float mouseSensitivity = 3f;
    public float scrollSpeed = 5f;

    public float smoothTime = 0.1f;
    public float pitchMin = -80f;
    public float pitchMax = 80f;

    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        targetPosition = transform.position;
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        HandleCursorLock();
        HandleMovement();
        HandleRotation();
        HandleScrollZoom();
        SmoothMove();
    }

    void HandleCursorLock()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleMovement()
    {
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            speed *= fastSpeedMultiplier;
        }

        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += transform.forward;
        if (Input.GetKey(KeyCode.S)) direction -= transform.forward;
        if (Input.GetKey(KeyCode.A)) direction -= transform.right;
        if (Input.GetKey(KeyCode.D)) direction += transform.right;
        if (Input.GetKey(KeyCode.E)) direction += transform.up;
        if (Input.GetKey(KeyCode.Q)) direction -= transform.up;

        targetPosition += direction.normalized * speed * Time.deltaTime;
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            yaw += mouseX;
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    void HandleScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetPosition += transform.forward * scroll * scrollSpeed;
        }
    }

    void SmoothMove()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothTime);
    }
}