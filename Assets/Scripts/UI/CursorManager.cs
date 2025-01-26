using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
	public Material[] playerMats;
	public Selectable first;

	PlayerInputManager pmanager;

	private void Awake() {
		pmanager = GetComponent<PlayerInputManager>();
	}

	public void OnJoined(PlayerInput input) {
		CursorController cursor = input.GetComponent<CursorController>();
		foreach (Renderer render in cursor.renderers) {
			render.material = playerMats[pmanager.playerCount - 1];
		}
		cursor.offset = pmanager.playerCount * 3f;
		cursor.SetCurOption(first);
		cursor.text.text = "P" + pmanager.playerCount;
	}
}
