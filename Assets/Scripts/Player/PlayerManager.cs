using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	public Tapioca[] option1;
	public Tapioca[] option2;
	Tapioca[][] options = new Tapioca[2][];
	PlayerInputManager pmanager;

	private void Awake() {
		pmanager = GetComponent<PlayerInputManager>();
		options[0] = option1;
		options[1] = option2;
	}

	public void PlayerJoined(PlayerInput pinput) {
		pinput.GetComponent<PlayerController>().tapioca_prefabs = options[pmanager.playerCount % options.Length];
        pinput.transform.position = transform.position;
    }
}
