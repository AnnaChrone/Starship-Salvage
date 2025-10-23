using UnityEngine;

public class AlienController : MonoBehaviour
{
    Animator animator;
    public bool isBouncing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("isBouncing", isBouncing);
    }
}
