using UnityEngine;

public class PlatformMoving : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 10;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private float movementAmountX = 1;
    [SerializeField] private float movementSpeedX = 1;
    [SerializeField] private float movementAmountY = 1;
    [SerializeField] private float movementSpeedY = 1;

	[SerializeField] bool random_offset = false;
    private Vector3 initialPosition;
    private Vector3 initialRotation;
	private float x, y, z = 0f;

	Rigidbody2D bod;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation.eulerAngles;
		if (random_offset) {
			x = Random.Range(-Mathf.PI, Mathf.PI);
			y = Random.Range(-Mathf.PI, Mathf.PI);
			z = Random.Range(-Mathf.PI, Mathf.PI);
		}

		bod = GetComponent<Rigidbody2D>();
		if (bod) {
			transform.position = new Vector3(
				initialPosition.x + movementAmountX * Mathf.Sin((Time.time + x) * movementSpeedX),
				initialPosition.y + movementAmountY * Mathf.Sin((Time.time + y) * movementSpeedY),
				initialPosition.z
			);
			transform.rotation = Quaternion.Euler(
				initialRotation.x,
				initialRotation.y,
				initialRotation.z + rotationAmount * Mathf.Sin((Time.time + z) * rotationSpeed)
			);
			inverse_time = 1f / Time.fixedDeltaTime;
		}
    }

	float inverse_time;

    private void FixedUpdate()
    {
		if (bod) {
			bod.linearVelocityX = (movementAmountX * Mathf.Sin((Time.time + x) * movementSpeedX) -
									movementAmountX * Mathf.Sin((Time.time + x -  Time.fixedDeltaTime) * movementSpeedX)) * inverse_time;
			bod.linearVelocityY = (movementAmountY * Mathf.Sin((Time.time + y) * movementSpeedY) -
									movementAmountY * Mathf.Sin((Time.time + y -  Time.fixedDeltaTime) * movementSpeedY)) * inverse_time;
			bod.angularVelocity = (rotationAmount * Mathf.Sin((Time.time + z) * rotationSpeed) -
									rotationAmount * Mathf.Sin((Time.time + z -  Time.fixedDeltaTime) * rotationSpeed)) * inverse_time;
		}
		else {
			transform.position = new Vector3(
            	initialPosition.x + movementAmountX * Mathf.Sin((Time.time + x)* movementSpeedX),
            	initialPosition.y + movementAmountY * Mathf.Sin((Time.time + y)* movementSpeedY),
	            initialPosition.z
        	);
        	transform.rotation = Quaternion.Euler(
            	initialRotation.x,
            	initialRotation.y,
            	initialRotation.z + rotationAmount * Mathf.Sin((Time.time + z)* rotationSpeed)
        	);
		}
    }
}
