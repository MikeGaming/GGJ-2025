using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public int start_amount = 50;
	public float speed = 90f;
	public float final_angle = 95f;
	public Rigidbody2D cup;

	public void PlayerJoined(PlayerController pinput, int index, Material mat) {
		pinput.name = "Player " + index;
		pinput.transform.position = transform.position;
		pinput.player_mat = mat;
		pinput.SpawnTapioca(start_amount, transform.position);
	}

	public void StartGame() {
		StartCoroutine(Rotate());
	}

	IEnumerator Rotate() {
		float angle = cup.rotation;
		while (angle != final_angle) {
			cup.angularVelocity = (Mathf.MoveTowards(angle, final_angle, speed * Time.fixedDeltaTime) - angle) / Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
			angle = cup.rotation;
		}
		cup.angularVelocity = 0f;
		cup.rotation = angle;
	}
}
