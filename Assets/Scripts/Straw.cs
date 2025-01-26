using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Straw : MonoBehaviour
{
	public Transform[] points;
	public float speed = 10f;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.attachedRigidbody) {
			StartCoroutine(Suck(other.attachedRigidbody, other));
		}
	}

	WaitForFixedUpdate wffu = new WaitForFixedUpdate();

	IEnumerator Suck(Rigidbody2D boba, Collider2D shape) {
		boba.bodyType = RigidbodyType2D.Kinematic;
		boba.linearVelocity = Vector2.zero;
		boba.angularVelocity = 0f;

		while (boba.position.x != transform.position.x && boba.position.y != transform.position.y) {
			yield return wffu;
			boba.position = Vector2.MoveTowards(boba.position, transform.position, speed * Time.fixedDeltaTime);
		}

		int temp_layer = boba.gameObject.layer;
		boba.gameObject.layer = 7;
		shape.gameObject.layer = 7;

		int index = 0;
		float percentage = 0;
		Vector3 start_pos = transform.position;
		while (index < points.Length) {
			float dist = Vector2.Distance(boba.position, points[index].position);
			float one_over_dist = 1f / dist;
			while (percentage < dist) {
				boba.transform.position = Vector3.Lerp(start_pos, points[index].position, percentage * one_over_dist);
				yield return wffu;
				percentage += speed * Time.fixedDeltaTime;
			}
			start_pos = points[index].position;
			percentage -= dist;
			++index;
		}
		boba.position = start_pos;

		boba.gameObject.layer = temp_layer;
		shape.gameObject.layer = temp_layer;
		boba.bodyType = RigidbodyType2D.Dynamic;
	}
}
