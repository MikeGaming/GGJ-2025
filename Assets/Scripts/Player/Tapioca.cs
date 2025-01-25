using UnityEngine;

public class Tapioca : MonoBehaviour
{
	public Renderer render;
	[HideInInspector]
	public PlayerController controller;
	Rigidbody2D bod;

	private void Awake() {
		bod = GetComponent<Rigidbody2D>();
		transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
	}

	public void Move(float direction, float rot_target) {
		if (Mathf.Abs(bod.linearVelocityX) < controller.max_speed) {
			bod.linearVelocityX = Mathf.Clamp(bod.linearVelocityX + direction * Time.fixedDeltaTime,
					-controller.max_speed, controller.max_speed);
		}
		bod.angularVelocity = Mathf.MoveTowards(bod.angularVelocity, -Mathf.Sign(direction) * rot_target, controller.roll_speed * Time.fixedDeltaTime);
	}

	public void Jump(float min_strength, float max_strength, float x_variance) {
		bod.linearVelocityY = Mathf.Max(bod.linearVelocityY, 0f) + Random.Range(min_strength, max_strength) * Mathf.Max(
				100f - (new Vector2(transform.position.x, transform.position.y) - controller.average).sqrMagnitude, 50f) * 0.01f;
		bod.linearVelocityX += Random.Range(-x_variance, x_variance);
	}

	private void OnDestroy() {
		controller.move_tapioca -= Move;
	}
}
