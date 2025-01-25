using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public Material[] options;
	PlayerInputManager pmanager;

	private void Awake() {
		pmanager = GetComponent<PlayerInputManager>();
	}

	public void PlayerJoined(PlayerInput pinput) {
		pinput.GetComponent<PlayerController>().colour = options[pmanager.playerCount % options.Length];
	}
}
