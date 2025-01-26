using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class End : MonoBehaviour
{

	Dictionary<PlayerInput, int> score = new Dictionary<PlayerInput, int>();

	private void OnTriggerEnter2D(Collider2D other) {
		
	}
}
