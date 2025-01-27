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
	float counter = 0f;

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
			if (random_offset) {
				transform.position = new Vector3(
					initialPosition.x + movementAmountX * Mathf.Sin((counter + x) * movementSpeedX),
					initialPosition.y + movementAmountY * Mathf.Sin((counter + y) * movementSpeedY),
					initialPosition.z
				);
				transform.rotation = Quaternion.Euler(
					initialRotation.x,
					initialRotation.y,
					initialRotation.z + rotationAmount * Mathf.Sin((counter + z) * rotationSpeed)
				);
			}
			inverse_time = 1f / Time.fixedDeltaTime;
		}
    }

	float inverse_time;

    private void FixedUpdate()
    {
		if (bod) {
			bod.linearVelocityX = (movementAmountX * Mathf.Sin((counter + x) * movementSpeedX) -
									movementAmountX * Mathf.Sin((counter + x -  Time.fixedDeltaTime) * movementSpeedX)) * inverse_time;
			bod.linearVelocityY = (movementAmountY * Mathf.Sin((counter + y) * movementSpeedY) -
									movementAmountY * Mathf.Sin((counter + y -  Time.fixedDeltaTime) * movementSpeedY)) * inverse_time;
			bod.angularVelocity = (rotationAmount * Mathf.Sin((counter + z) * rotationSpeed) -
									rotationAmount * Mathf.Sin((counter + z -  Time.fixedDeltaTime) * rotationSpeed)) * inverse_time;
		}
		else {
			transform.position = new Vector3(
            	initialPosition.x + movementAmountX * Mathf.Sin((counter + x)* movementSpeedX),
            	initialPosition.y + movementAmountY * Mathf.Sin((counter + y)* movementSpeedY),
	            initialPosition.z
        	);
        	transform.rotation = Quaternion.Euler(
            	initialRotation.x,
            	initialRotation.y,
            	initialRotation.z + rotationAmount * Mathf.Sin((counter + z)* rotationSpeed)
        	);
		}
		counter += Time.fixedDeltaTime;
    }
}
