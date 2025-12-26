using UnityEngine;

namespace Platformer.Mechanics
{
    public class WindZone : MonoBehaviour
    {
        [Header("Wind")]
        public Vector2 direction = Vector2.right; // направление ветра
        public float strength = 3f;                // сила

        public Vector2 WindForce => direction.normalized * strength;

        private void OnTriggerStay2D(Collider2D other)
        {
            Debug.Log("info");
            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Добавляем силу ветра к объекту
                rb.AddForce(WindForce, ForceMode2D.Force);
            }
        }
    }
}
