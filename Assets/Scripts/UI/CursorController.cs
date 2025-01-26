using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
	public float offset = 0;
	public TMP_Text text;
	public Renderer[] renderers;
	public List<Transform> tokens;
	public PlayerInput player;
	Selectable cur_option = null;
	int index = 0;

	private void Awake() {
		foreach (Transform token in tokens) {
			token.transform.localPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), 0f);
			token.localEulerAngles = Vector3.forward * Random.Range(-25f, 25f);
		}
	}

	public void Move(InputAction.CallbackContext input) {
		if (!input.started || cur_option == null)
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
		if (!input.started || cur_option == null)
			return;
		
		if (cur_option.GetComponent<ToppingSelectMoment>()) {
			Transform token = cur_option.GetComponent<ToppingSelectMoment>().ToggleButton(player,
					index == tokens.Count ? null : tokens[index]);
			if (token != null) {
				if (token.parent != transform) {
					++index;
					token.transform.position = cur_option.transform.position +
							new Vector3(Random.Range(-5f, 5f), Random.Range(-3f, 3f), -offset);
					token.localEulerAngles = Vector3.forward * Random.Range(-25f, 25f);
				}
				else {
					--index;
				}
			}
		}
		else if (cur_option.GetComponent<Leave>()) {
			cur_option.GetComponent<Leave>().RemovePLayer(player);
		}
		else if (cur_option as Button) {
			(cur_option as Button).onClick.Invoke();
		}
	}

	public void SetCurOption(Selectable option) {
		cur_option = option;
		transform.position = cur_option.transform.position + Vector3.back * offset;
	}
}
