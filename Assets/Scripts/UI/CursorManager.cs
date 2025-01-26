using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
	public Material[] playerMats;
	public Selectable first;
	public ToppingSelectMoment[] buttons;

	PlayerInputManager pmanager;
	List<PlayerInput> players = new List<PlayerInput>();

	private void Awake() {
		pmanager = GetComponent<PlayerInputManager>();
	}

	public void OnJoined(PlayerInput input) {
		players.Add(input);

		CursorController cursor = input.GetComponent<CursorController>();
		foreach (Renderer render in cursor.renderers) {
			render.material = playerMats[pmanager.playerCount - 1];
		}
		cursor.offset = pmanager.playerCount * 3f;
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
				CursorController cursor = players[i].GetComponent<CursorController>();
				foreach (Renderer render in cursor.renderers) {
					render.material = playerMats[i];
				}
				foreach (Transform token in cursor.tokens) {
					if (token.parent != cursor.transform) {
						token.transform.position += Vector3.forward * 3f;
					}
				}
				cursor.transform.position += Vector3.forward * 3f;
				cursor.text.text = "P" + (i + 1);
			}
			++i;
		}
	}
}
