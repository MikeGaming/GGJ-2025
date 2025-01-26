using UnityEngine;
using UnityEngine.InputSystem;

public class Leave : MonoBehaviour
{
	public CursorManager pmanager;

	public void RemovePLayer(PlayerInput player) {
		foreach (ToppingSelectMoment option in pmanager.buttons) {
			if (option.playerInput == player) {
				Destroy(option.held_token.gameObject);
				option.ToggleButton(player, null);
			}
		}
		Destroy(player.gameObject);
	}
}
