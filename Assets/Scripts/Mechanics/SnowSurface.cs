using UnityEngine;

namespace Platformer.Mechanics
{
    public class SnowSurface : MonoBehaviour
    {
        [Header("Movement")]
        [Range(0f, 1f)]
        public float speedMultiplier = 0.6f;   // замедление движения

        [Header("Jump")]
        [Range(0f, 1f)]
        public float jumpMultiplier = 0.7f;    // уменьшение высоты прыжка
    }
}
