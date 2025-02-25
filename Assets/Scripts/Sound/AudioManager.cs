using Unity.Netcode;
using UnityEngine;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager instanse;

    public Sound[] sounds;

    public Sound[] walkSteps;

    private void Awake()
    {
        if (instanse == null)
        {
            instanse = this;
        }
        DontDestroyOnLoad(gameObject);
    }


}
