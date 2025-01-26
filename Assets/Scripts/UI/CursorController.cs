using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
	public float offset = 0;
	public TMP_Text text;
	public Renderer[] renderers;
	PlayerInput player;
	Selectable cur_option = null;

	private void Awake() {
		player = GetComponent<PlayerInput>();
	}

	public void Move(InputAction.CallbackContext input) {
		if (!input.started)
			return;
		
		Vector2 dir = input.ReadValue<Vector2>();
		if (dir != Vector2.zero) {
			if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) {
				if (dir.x > 0) {
					if (cur_option.navigation.selectOnRight)
						SetCurOption(cur_option = cur_option.navigation.selectOnRight);
				}
				else
					if (cur_option.navigation.selectOnLeft)
					SetCurOption(cur_option = cur_option.navigation.selectOnLeft);
			}
			else {
				if (dir.y > 0) {
					if (cur_option.navigation.selectOnUp)
						SetCurOption(cur_option = cur_option.navigation.selectOnUp);
				}
				else
					if (cur_option.navigation.selectOnDown)
					SetCurOption(cur_option = cur_option.navigation.selectOnDown);
			}
		}
	}

	public void Merge(InputAction.CallbackContext input) {
		if (input.started) {
			if (cur_option.GetComponent<ToppingSelectMoment>()) {
				(cur_option.GetComponent<ToppingSelectMoment>()).ToggleButton(player);
			}
			else if (cur_option as Button) {
				(cur_option as Button).onClick.Invoke();
			}
		}
	}

	public void SetCurOption(Selectable option) {
		Debug.Log(option);
		cur_option = option;
		transform.position = cur_option.transform.position + Vector3.back * offset;
	}
}
