using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform firstPersonAnchor;
    [SerializeField] private Transform thirdPersonAnchor;

    [Header("Motion")]
    [SerializeField] private Vector2 sensitivity = new Vector2(4.0f, 3.5f);
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float pitchClamp = 70f;

    private float yaw;
    private float pitch;
    private Vector2 lookInput;
    public bool useFirstPerson = false;

    public void SetTarget(Transform playerTarget)
    {
        target = playerTarget;
    }

    public void SetLookInput(Vector2 newInput)
    {
        lookInput = newInput;
    }

    public void ToggleCameraMode()
    {
        useFirstPerson = !useFirstPerson;
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        yaw += lookInput.x * sensitivity.x;
        pitch -= lookInput.y * sensitivity.y;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Transform selectedAnchor = useFirstPerson ? firstPersonAnchor : thirdPersonAnchor;

        if (selectedAnchor != null)
        {
            Vector3 desiredPosition = selectedAnchor.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * followSpeed);
        }
        else
        {
            Vector3 desiredPosition = target.position + new Vector3(0f, 2.5f, -5.5f);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * followSpeed);
        }
    }
}
