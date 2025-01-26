using UnityEngine;

public class Tapioca : MonoBehaviour
{
	public Collider2D shape;
	[HideInInspector]
	public PlayerController controller;
	[HideInInspector]
	public Rigidbody2D bod;

	private void Awake() {
		bod = GetComponent<Rigidbody2D>();
		shape.gameObject.layer = 10;
		transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
	}

	public void Move(float direction, float rot_target) {
		if (bod.bodyType == RigidbodyType2D.Kinematic) return;

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

	public bool RemoveSelf() {
		if (!controller) return false;
		
		shape.gameObject.layer = 10;

		controller.balls.Remove(this);
		controller.move_tapioca -= Move;
		controller.cam.following.Remove(transform);
		controller = null;
		return true;
	}

	private void OnDestroy() {
		RemoveSelf();
	}

	private void OnTriggerStay2D(Collider2D other) {
		if (controller && other.attachedRigidbody) {
			Tapioca tapioca = other.attachedRigidbody.GetComponent<Tapioca>();
			if (tapioca && tapioca.controller == null) {
				controller.TakeTapioca(tapioca);
			}
		}
	}
}
