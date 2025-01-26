using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 offset;
	[SerializeField]	float distanceScalerx = 0.75f;
						float distanceScalery;
	[SerializeField]	float minWidth = 10f;
	[SerializeField]	float maxWidth = 100f;
	[SerializeField]	float speed = 10f;
	public Transform fallback;
	public List<Transform> following = new List<Transform>();
	public List<Color> colors = new List<Color>();
	static public List<Color> colours = new List<Color>();

	[HideInInspector]
	public int playerCount = 0;

	private void Awake() {
		colours = colors;
		distanceScalery = distanceScalerx * (float)Screen.width / (float)Screen.height;
	}

	private void LateUpdate() {
		if (following.Count == 0 || playerCount == 0) {
			if (fallback) {
				transform.position = Vector3.MoveTowards(transform.position, fallback.position + offset, speed * 5f * Time.deltaTime);
			}

			return;
		}

		float minx = float.PositiveInfinity, maxx = float.NegativeInfinity;
		float miny = float.PositiveInfinity, maxy = float.NegativeInfinity;
		Vector3 sum = Vector3.zero;
		for (int i = 0; i < following.Count;) {
			if (following[i] == null || following[i].gameObject.layer == 10) {
				following.RemoveAt(i);
				continue;
			}
			else { 
				sum += following[i].position;
			}
			if (following[i].position.x > maxx)
				maxx = following[i].position.x;
			if (following[i].position.x < minx)
				minx = following[i].position.x;
			if (following[i].position.y > maxy)
				maxy = following[i].position.y;
			if (following[i].position.y < miny)
				miny = following[i].position.y;
			++i;
		}

		//if something gets killed in the loop
		if (following.Count <= 0) return;

		//get average
		sum /= following.Count;

		//figure out distance ratio
		offset.z = -Mathf.Clamp(Mathf.Max((maxx - minx) * distanceScalerx, (maxy - miny) * distanceScalery), minWidth, maxWidth);

		transform.position = Vector3.MoveTowards(transform.position, sum + offset, speed * Time.deltaTime);
	}
}
