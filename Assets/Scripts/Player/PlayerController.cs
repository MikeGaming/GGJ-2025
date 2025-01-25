using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public Material colour;	
	public float speed = 2.5f;
	public float merge_speed = 2.5f;
	public float max_speed = 5f;
	public float roll_speed = 36000f;
	public float max_rot_speed = 360f;
	public float jump_delay = 3f;
	public float min_jump_strength = 10f;
	public float max_jump_strength = 12f;
	public float jump_spread = 1f;
	public event Action<float, float> move_tapioca;
	List<Tapioca> balls = new List<Tapioca>();
	CameraController cam;
	float direction = 0;
	[HideInInspector]
	public Vector2 average = Vector2.zero;
	bool merging = false;

	private void Awake() {
		cam = Camera.main.GetComponent<CameraController>();
	}

	private void OnEnable() {
		cam.playerCount += 1;
	}

	private void OnDisable() {
		cam.playerCount -= 1;
	}

	private void FixedUpdate() {
		average = Vector2.zero;
		foreach (Tapioca ball in balls) {
			average += new Vector2(ball.transform.position.x, ball.transform.position.y);
		}
		average /= balls.Count;

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

	public void TakeTapioca(Tapioca tapioca) {
		if (tapioca.controller) {
			tapioca.controller.balls.Remove(tapioca);
			tapioca.controller.move_tapioca -= tapioca.Move;
		}
		else {
			cam.following.Add(tapioca.transform);
		}
		
		tapioca.controller = this;
		tapioca.render.material = colour;
		balls.Add(tapioca);
		move_tapioca += tapioca.Move;
	}

	//WaitForFixedUpdate wffu = new WaitForFixedUpdate();

	IEnumerator JumpTimer() {
		jump_queued = 3;
		yield return new WaitForSeconds(jump_delay);
		if (jump_queued == 2)
			Jump();
		else
			jump_queued = 0;
	}
}
