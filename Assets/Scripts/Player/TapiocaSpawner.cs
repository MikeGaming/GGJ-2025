using UnityEngine;

public class TapiocaSpawner : MonoBehaviour
{
	public PlayerController controller;
	public int amount = 500;
	void Start()
	{
		controller.SpawnTapioca(amount, transform.position);
	}
}
