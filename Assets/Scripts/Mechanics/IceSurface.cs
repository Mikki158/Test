using UnityEngine;

namespace Platformer.Mechanics
{
    public class IceSurface : MonoBehaviour
    {
        [Header("Ice settings")]
        public float acceleration = 20f;   // как быстро разгоняемся
        public float maxSpeed = 12f;        // максимальная скорость на льду
        public float friction = 0.05f;      // как быстро замедляемся
    }
}

