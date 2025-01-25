using UnityEngine;

public class TapiocaSpawner : MonoBehaviour
{
	public Tapioca[] tapioca;
	public PlayerController controller;
	public int amount = 500;
	void Start()
	{
		for (int i = 0; i < amount; ++i) {
			controller.TakeTapioca(Instantiate(tapioca[Random.Range(0, tapioca.Length)], transform.position + Vector3.right * Random.Range(-0.5f, 0.5f), Quaternion.identity));
		}
	}
}
