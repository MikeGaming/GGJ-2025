using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Straw : MonoBehaviour
{
	public Transform[] points;
	public float speed = 10f;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.attachedRigidbody && other.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic) {
			StartCoroutine(Suck(other.attachedRigidbody, other));
		}
	}

	WaitForFixedUpdate wffu = new WaitForFixedUpdate();

	IEnumerator Suck(Rigidbody2D boba, Collider2D shape) {
		boba.bodyType = RigidbodyType2D.Kinematic;
		boba.linearVelocity = Vector2.zero;
		boba.angularVelocity = 0f;
		Transform trans = boba.transform;
		while (trans && trans.position != transform.position) {
			trans.position = Vector3.MoveTowards(trans.position, transform.position, speed * Time.fixedDeltaTime);
			yield return wffu;
		}
		//If killed midway
		if (boba == null)
			yield break;

		int temp_layer = boba.gameObject.layer;
		int temp_layer2 = shape.gameObject.layer;
		boba.gameObject.layer = 7;
		shape.gameObject.layer = 7;

		int index = 0;
		float percentage = speed * Time.fixedDeltaTime;
		Vector3 start_pos = transform.position;
		while (trans && index < points.Length) {
			float dist = Vector3.Distance(trans.position, points[index].position);
			float one_over_dist = 1f / dist;
			while (trans && percentage < dist) {
				trans.position = Vector3.Lerp(start_pos, points[index].position, percentage * one_over_dist);
				yield return wffu;
				percentage += speed * Time.fixedDeltaTime;
			}
			start_pos = points[index].position;
			percentage -= dist;
			++index;
		}
		//If killed midway
		if (boba == null)
			yield break;

		trans.position = start_pos;

		boba.gameObject.layer = temp_layer;
		shape.gameObject.layer = temp_layer2;
		boba.bodyType = RigidbodyType2D.Dynamic;
	}
}
