using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class End : MonoBehaviour
{
	public float cam_distance = 25f;
	public TMP_Text text;
	CameraController cam;
	Dictionary<PlayerController, int> score = new Dictionary<PlayerController, int>();

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.attachedRigidbody) {
			Tapioca tapioca = other.attachedRigidbody.GetComponent<Tapioca>();
			if (tapioca && tapioca.controller) {
				PlayerController controller = tapioca.controller;
				if (tapioca.RemoveSelf()) {
					if (score.ContainsKey(controller))
						score[controller] += 1;
					else {
						score[controller] = 1;
					}
				}
			}
		}
	}

	private void Awake() {
		cam = Camera.main.GetComponent<CameraController>();
	}

	private void Update() {
		if (score.Count != 0 && cam.following.Count == 0) {
			if (cam.fallback != transform) {
				cam.fallback = transform;
				cam.offset = Vector3.back * cam_distance;
				List<PlayerController> keys = new List<PlayerController>();

				foreach (PlayerController key in score.Keys) {
					int cur_score = score[key];
					int index = 0;
					foreach (PlayerController key2 in keys) {
						if (cur_score > score[key2]) {
							keys.Insert(index, key);
							cur_score = -1;
							break;
						}
						++index;
					}
					if (cur_score != -1)
						keys.Add(key);
				}

				text.text = "";
				foreach (PlayerController key in keys) {
					text.text += key.name + ": " + score[key] + "\n";
				}
			}
		}
	}
}
