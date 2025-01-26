using UnityEngine;

public class MainMenuBalls : MonoBehaviour
{
	public Transform lower_left;
	public Transform upper_right;

	public Tapioca[] tapiocas;
	public int amount;

	private void Start() {
		for (int i = 0; i < amount; ++i) {
			Instantiate(tapiocas[Random.Range(0, tapiocas.Length)],
					Vector3.right * Random.Range(lower_left.position.x, upper_right.position.x) +
					Vector3.up * Random.Range(lower_left.position.y, upper_right.position.y),
					Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
		}
	}
}
