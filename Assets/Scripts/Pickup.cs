using System.Collections;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public int amount = 25;

	private void OnTriggerEnter2D(Collider2D other) {
		if (amount == 0)
			return;
		
		if (other.attachedRigidbody) {
			Tapioca tapioca = other.attachedRigidbody.GetComponent<Tapioca>();
			if (tapioca && tapioca.controller) {
				tapioca.controller.SpawnTapioca(amount, transform.position);
				amount = 0;
				gameObject.layer = 10;
				StartCoroutine(Die());
			}
		}
	}

	IEnumerator Die() {
		float spin = 720f;
		float scale_velo = -2f;
		transform.eulerAngles = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f));
		while (transform.localScale != Vector3.zero) {
			transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, scale_velo * Time.deltaTime);
			scale_velo += 3f * Time.deltaTime;

			transform.RotateAround(transform.position, Vector3.forward, spin * Time.deltaTime);
			spin -= 900f * Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);
	}
}
