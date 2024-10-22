using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerBehaviour))]
public class PlayerRifleShot : MonoBehaviour
{
    [SerializeField] private PlayerState currentState;

    [SerializeField] private PlayerBehaviour playerBehaviour;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform bulletPosition;
    //[SerializeField] private ParticleSystem shotEffect;

    public UnityEvent MakeShot;

    private void Update()
    {
        currentState = playerBehaviour.currentState;
        HandlerMoving();
    }

    private void HandlerMoving()
    {
        if(Input.GetMouseButton(1))
        {
            if(Input.GetMouseButton(0))
            {
                Shot();
                MakeShot?.Invoke();
            }
        }
    }

    private void Shot()
    {
        if (currentState == PlayerState.Firegun)
        {
            print("Make shot");
            CreateShot();
        }
    }

    private GameObject CreateShot()
    {
        return Instantiate(bullet, bulletPosition.transform.position, Quaternion.identity);
    }
}
