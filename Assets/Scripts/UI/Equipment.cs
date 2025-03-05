using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : NetworkBehaviour
{
    [SerializeField] private Image[] images;

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(IsOwner);
        ChangeIcon(PlayerState.Empty);
    }
    private void Update()
    {
        if (!IsOwner) return;
        if(Input.GetKeyDown(KeyCode.Alpha1)) ChangeIcon(PlayerState.Empty);
        if(Input.GetKeyDown(KeyCode.Alpha2)) ChangeIcon(PlayerState.TwoSwords);
        if(Input.GetKeyDown(KeyCode.Alpha3)) ChangeIcon(PlayerState.FastSwords);
        if(Input.GetKeyDown(KeyCode.Alpha4)) ChangeIcon(PlayerState.Sword);
    }

    private void ChangeIcon(PlayerState playerState)
    {
        ResetAllImagesTransparency();

        switch (playerState)
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
