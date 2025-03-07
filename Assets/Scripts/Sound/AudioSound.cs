using UnityEngine;

[System.Serializable]
public class AudioSound 
{
    public string name; 
    public AudioClip clip; 
    [Range(0f, 1f)] public float volume = 1f; 
    public bool loop; 

    [HideInInspector] public AudioSource source;
}
