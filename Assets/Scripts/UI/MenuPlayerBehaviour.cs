using UnityEngine;

public class MenuPlayerBehaviour : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private string[] idleAnimationNames;

    private float delay = 10f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("RepitAnimation", delay, delay * 6);
    }

    private void RepitAnimation()
    {
        int randomIndex = Random.Range(0, idleAnimationNames.Length);
        animator.Play(idleAnimationNames[randomIndex]);
    }
}
