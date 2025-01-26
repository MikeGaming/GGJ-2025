using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
	public Material[] playerMats;
	public Selectable first;
	public ToppingSelectMoment[] buttons;
	public PlayerInputManager pmanager;
	public PlayerManager plmanager;

	public GameObject manu;
	public Camera game_camera;

	List<PlayerInput> players = new List<PlayerInput>();

	void Start() {
		game_camera.enabled = false;
		manu.SetActive(true);
	}

	public void OnJoined(PlayerInput input) {
		input.GetComponent<PlayerController>().cam = game_camera.GetComponent<CameraController>();
		players.Add(input);

		CursorController cursor = input.GetComponentInChildren<CursorController>();
		foreach (Renderer render in cursor.renderers) {
			render.material = playerMats[pmanager.playerCount - 1];
		}
		cursor.offset = pmanager.playerCount * 2f;
		cursor.text.text = "P" + pmanager.playerCount;

		StartCoroutine(Delay(cursor));
	}

	IEnumerator Delay(CursorController cursor) {
		yield return new WaitForSeconds(0.1f);
		cursor.SetCurOption(first);
	}

	public void OnLeaved(PlayerInput input) {
		bool after = false;
		for (int i = 0; i < players.Count;) {
			if (players[i] == input) {
				players.RemoveAt(i);
				after = true;
				continue;
			}
			if (after) {
				CursorController cursor = players[i].GetComponentInChildren<CursorController>();
				foreach (Renderer render in cursor.renderers) {
					render.material = playerMats[i];
				}
				foreach (Transform token in cursor.tokens) {
					if (token.parent != cursor.transform) {
						token.transform.position += Vector3.forward * 2f;
					}
				}
				cursor.transform.position += Vector3.forward * 2f;
				cursor.text.text = "P" + (i + 1);
			}
			
			++i;
		}
	}

	public void StartGame() {
		foreach (PlayerInput input in players) {
			CursorController cursor = input.GetComponentInChildren<CursorController>();
			if (cursor.tokens[0].parent == cursor.transform)
				return;
		}

		pmanager.DisableJoining();
		int index = 1;
		foreach (PlayerInput input in players) {
			PlayerController player = input.GetComponent<PlayerController>();
			CursorController cursor = input.GetComponentInChildren<CursorController>();

			foreach (Transform token in cursor.tokens) {
				if (token.parent == cursor.transform)
					break;
				ToppingSelectMoment topping = token.parent.GetComponent<ToppingSelectMoment>();
				topping.ToggleButton(input, token);
				player.tapioca_prefabs.Add(topping.prefab);
			}
			player.enabled = true;
			Destroy(cursor.gameObject);
			input.SwitchCurrentActionMap("Player");
			plmanager.PlayerJoined(player, index++);
		}

		plmanager.Invoke("StartGame", 5f);
		game_camera.enabled = true;
		manu.SetActive(false);
	}
}
