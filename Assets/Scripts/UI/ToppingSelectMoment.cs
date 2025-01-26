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

	public PlayerInput playerInput;
	public Transform held_token;

    private void Start()
    {
        image = GetComponent<Image>();
    }



    public Transform ToggleButton(PlayerInput player, Transform token)
    {
        if (isOn) {
			if (playerInput != player)
				return null;
    	    image.sprite = defaultSprite;
            isOn = false;

			token = held_token;
			token.SetParent(player.transform, true);
			playerInput = null;
			held_token = null;
			return token;
        }
		if (token != null) {
        	image.sprite = clicked;
        	isOn = true;

        	token.SetParent(null, true);
			playerInput = player;
			held_token = token;
        	return token;
		}
		return null;
    }
}
