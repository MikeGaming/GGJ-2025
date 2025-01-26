using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 10;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float movementAmountX = 1;
    [SerializeField] private float movementSpeedX = 1;
    [SerializeField] private float movementAmountY = 1;
    [SerializeField] private float movementSpeedY = 1;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
    }

    private void Update()
    {
        transform.position = new Vector3(
            initialPosition.x + movementAmountX * Mathf.Sin(Time.time * movementSpeedX),
            initialPosition.y + movementAmountY * Mathf.Sin(Time.time * movementSpeedY),
            initialPosition.z
        );
        transform.rotation = Quaternion.Euler(
            initialRotation.x,
            initialRotation.y,
            initialRotation.z + rotationAmount * Mathf.Sin(Time.time * rotationSpeed)
        );
    }
}
