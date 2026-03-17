using UnityEngine;

public class AudioAttack : MonoBehaviour
{
    AudioSource slashSound;

    void Start()
    {
        slashSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // chuột trái
        {
            Attack();
        }
    }

    void Attack()
    {
        slashSound.Play();
    }
}