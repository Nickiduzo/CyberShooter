using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    [SerializeField] private Image[] images;

    [SerializeField] private PlayerBehaviour playerBehaviour;

    private void Update()
    {
        ChangeIcon();
    }

    private void ChangeIcon()
    {
        ResetAllImagesTransparency();

        switch (playerBehaviour.currentState)
        {
            case PlayerState.Empty:
                SetImageTransparency(images[0], 0.5f);
                break;
            case PlayerState.TwoSwords:
                SetImageTransparency(images[1], 0.5f);
                break;
            case PlayerState.FastSwords:
                SetImageTransparency(images[2], 0.5f);
                break;
            case PlayerState.Sword:
                SetImageTransparency(images[3], 0.5f);
                break;
            default:
                break;
        }
    }

    private void ResetAllImagesTransparency()
    {
        foreach(var elem in images)
        {
            SetImageTransparency(elem,1f);
        }
    }

    private void SetImageTransparency(Image image, float apha)
    {
        Color color = image.color;
        color.a = apha;
        image.color = color;
    }
}
