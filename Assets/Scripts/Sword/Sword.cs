using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            print(collision.gameObject.name);
        }
    }
}
