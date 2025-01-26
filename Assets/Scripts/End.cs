using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
	public float cam_distance = 25f;
	public Straw straw;
	public Transform straw_target;
	public TMP_Text text;
	public CursorManager cmanager;

	CameraController cam;
	Dictionary<PlayerController, int> score = new Dictionary<PlayerController, int>();

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.attachedRigidbody) {
			Tapioca tapioca = other.attachedRigidbody.GetComponent<Tapioca>();
			if (tapioca && tapioca.controller) {
				PlayerController controller = tapioca.controller;
				if (tapioca.RemoveSelf()) {
					tapioca.shape.gameObject.layer = 8;
					tapioca.gameObject.layer = 2;
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
		straw.GetComponent<Collider2D>().enabled = false;
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
			else {
				bool merging = true;
				foreach (PlayerController key in score.Keys) {
					merging &= key.merging;
				}
				if (merging) {
					StartCoroutine(Straw());
				}
			}
		}
	}

	IEnumerator Straw() {
		straw.GetComponent<Collider2D>().enabled = true;
		
		while (straw.transform.position != straw_target.position) {
			straw.transform.position = Vector3.MoveTowards(straw.transform.position,
					straw_target.position, 5f * Time.deltaTime);
			yield return null;
		}
		
		float timer = 0;
		while (timer < 5f) {

			timer += Time.deltaTime;
			yield return null;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
