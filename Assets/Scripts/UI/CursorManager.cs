using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
	public Material[] playerMats;
	public Material[] stampMats;
	public Selectable first;
	public ToppingSelectMoment[] buttons;
	public PlayerInputManager pmanager;
	public PlayerManager plmanager;
	public float stack_size = 1f;
	public float base_stack = 1f;

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
			render.sharedMaterial = playerMats[pmanager.playerCount - 1];
		}
		foreach (Transform render in cursor.tokens) {
			render.GetComponentInChildren<Renderer>().sharedMaterial = stampMats[pmanager.playerCount - 1];
		}
		cursor.offset = base_stack + pmanager.playerCount * stack_size;
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
					render.sharedMaterial = playerMats[i];
				}
				foreach (Transform token in cursor.tokens) {
					token.GetComponentInChildren<Renderer>().sharedMaterial = stampMats[i];
					if (token.parent != cursor.transform) {
						token.transform.position += Vector3.forward * stack_size;
					}
				}
				cursor.transform.position += Vector3.forward * stack_size;
			}
			
			++i;
		}
	}

	public void StartGame() {
		foreach (PlayerInput input in players) {
			CursorController cursor = input.GetComponentInChildren<CursorController>();
			bool check = true;
			foreach (Transform token in cursor.tokens) {
				if (token.parent != cursor.transform) {
					check = false;
					break;
				}
			}
			if (check)
				return;
		}

		pmanager.DisableJoining();
		int index = 1;
		foreach (PlayerInput input in players) {
			PlayerController player = input.GetComponent<PlayerController>();
			CursorController cursor = input.GetComponentInChildren<CursorController>();

			foreach (Transform token in cursor.tokens) {
				if (token.parent == cursor.transform)
					continue;
				ToppingSelectMoment topping = token.parent.GetComponent<ToppingSelectMoment>();
				topping.ToggleButton(input, token);
				player.tapioca_prefabs.Add(topping.prefab);
			}
			player.enabled = true;
			Destroy(cursor.gameObject);
			input.SwitchCurrentActionMap("Player");
			plmanager.PlayerJoined(player, index, playerMats[index]);
			++index;
		}

		plmanager.Invoke("StartGame", 5f);
		game_camera.enabled = true;
        manu.SetActive(false);
	}
}
