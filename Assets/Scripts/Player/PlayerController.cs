using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
	public Tapioca[] tapioca_prefabs;
	public float speed = 2.5f;
	public float merge_speed = 2.5f;
	public float max_speed = 5f;
	public float roll_speed = 36000f;
	public float max_rot_speed = 360f;
	public float jump_delay = 3f;
	public float min_jump_strength = 10f;
	public float max_jump_strength = 12f;
	public float jump_spread = 1f;
	public float max_distance = 25f;
	public event Action<float, float> move_tapioca;
	[HideInInspector]
	public List<Tapioca> balls = new List<Tapioca>();
	[HideInInspector]
	public CameraController cam;
	float direction = 0;
	[HideInInspector]
	public Vector2 average = Vector2.zero;
	[HideInInspector]
	public bool merging = false;

	private void Awake() {
		cam = Camera.main.GetComponent<CameraController>();
		average = transform.position;
	}

	private void OnEnable() {
		cam.playerCount += 1;
	}

	private void OnDisable() {
		cam.playerCount -= 1;
	}

	private void FixedUpdate() {
		if (balls.Count == 0)
			return;
		
		Vector2 new_average = Vector2.zero;
		for (int i = 0; i < balls.Count;) {
			Tapioca ball = balls[i];
			if (Vector2.SqrMagnitude(ball.bod.position - average) > max_distance * max_distance) {
				ball.RemoveSelf();
				continue;
			}
			new_average += new Vector2(ball.transform.position.x, ball.transform.position.y);
			++i;
		}
		average = new_average / balls.Count;

		if (direction != 0) {
			move_tapioca?.Invoke(direction * speed, max_rot_speed);
		}

		if (merging) {
			float half_rot_speed = max_rot_speed * 0.5f;
			foreach (Tapioca ball in balls) {
				ball.Move(Mathf.Clamp(average.x - ball.transform.position.x, -merge_speed, merge_speed), half_rot_speed);
			}
		}
	}

	int jump_queued = 0;
	public void Move(InputAction.CallbackContext input) {
		if (input.started) {
			return;
		}

		if (input.ReadValue<Vector2>().y > 0.25f) {
			if (jump_queued == 0)
				Jump();
			else if (jump_queued == 1)
				jump_queued = 2;
		}
		else if (jump_queued != 0)
			jump_queued = 1;

		direction = input.ReadValue<Vector2>().x;
	}

	public void Merge(InputAction.CallbackContext input) {
		if (input.started) {
			merging = true;
		}
		else if (input.canceled)
			merging = false;
		
	}

	void Jump() {
		foreach (Tapioca ball in balls) {
			ball.Jump(min_jump_strength, max_jump_strength, jump_spread);
		}

		StartCoroutine(JumpTimer());
	}

	public void SpawnTapioca(int amount, Vector3 position) {
		for (int i = 0; i < amount; ++i) {
			TakeTapioca(Instantiate(tapioca_prefabs[Random.Range(0, tapioca_prefabs.Length)],
					position + Vector3.right * Random.Range(-0.5f, 0.5f) +
					Vector3.up * Random.Range(-0.5f, 0.5f), Quaternion.identity));
		}
	}

	public void TakeTapioca(Tapioca tapioca) {
		if (tapioca.controller)
			tapioca.RemoveSelf();
		
		tapioca.shape.gameObject.layer = 3;
		
		tapioca.controller = this;
		balls.Add(tapioca);
		cam.following.Add(tapioca.transform);
		move_tapioca += tapioca.Move;
	}

	IEnumerator JumpTimer() {
		jump_queued = 3;
		yield return new WaitForSeconds(jump_delay);
		if (jump_queued == 2)
			Jump();
		else
			jump_queued = 0;
	}
}
