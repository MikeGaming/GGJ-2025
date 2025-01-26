using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ToppingSelectMoment : MonoBehaviour
{
	public int toppingType = 0;
    bool isOn = false;
    [SerializeField] Sprite defaultSprite, clicked;
    Image image;

	PlayerInput playerInput;

    private void Start()
    {
        image = GetComponent<Image>();
    }



    public void ToggleButton(PlayerInput player)
    {
        if (isOn)
        {
            image.sprite = defaultSprite;
            isOn = false;
			playerInput = player;
            // remove topping from list
            //ToppingManager.Instance.RemoveTopping(toppingType);
        }
        else if (playerInput == player)
        {
            image.sprite = clicked;
            isOn = true;
            // add topping to list
            //ToppingManager.Instance.AddTopping(toppingType);
            //ToppingManager.Instance.count++;
        }
    }
}
