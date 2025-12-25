using Platformer.Mechanics;
using UnityEngine;

public class SlowMotionZone : MonoBehaviour
{
    public float slowMotionFactor = 0.5f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.speedMultiplier = slowMotionFactor;
                player.limitJumpHeight = true; // Включаем ограничение высоты прыжка
            }

            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.speed *= slowMotionFactor;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.speedMultiplier = 1f;
                player.limitJumpHeight = false; // Выключаем ограничение
            }

            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.speed /= slowMotionFactor;
            }
        }
    }
}
