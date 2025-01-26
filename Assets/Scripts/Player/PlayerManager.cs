using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public int start_amount = 50;
	public float speed = 90f;
	public float final_angle = 95f;
	public Transform cup;

	public void PlayerJoined(PlayerController pinput, int index) {
		pinput.name = "Player " + index;
		pinput.transform.position = transform.position;
		pinput.SpawnTapioca(start_amount, transform.position);
	}

	public void StartGame() {
		StartCoroutine(Rotate());
	}

	IEnumerator Rotate() {
		float angle = cup.eulerAngles.z;
		while (angle != final_angle) {
			angle = Mathf.MoveTowards(angle, final_angle, speed * Time.deltaTime);
			cup.eulerAngles = Vector3.forward * angle;
			yield return null;
		}
	}
}
