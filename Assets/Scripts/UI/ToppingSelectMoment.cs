using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ToppingSelectMoment : MonoBehaviour
{
    bool isOn = false;
    [SerializeField] Sprite defaultSprite, clicked;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }



    public void ToggleButton(int toppingType)
    {
        if (isOn)
        {
            image.sprite = defaultSprite;
            isOn = false;
            // remove topping from list
            //ToppingManager.Instance.RemoveTopping(toppingType);
        }
        else //if ToppingManager.Instance.count < 3
        {
            image.sprite = clicked;
            isOn = true;
            // add topping to list
            //ToppingManager.Instance.AddTopping(toppingType);
            //ToppingManager.Instance.count++;
        }
    }
}
