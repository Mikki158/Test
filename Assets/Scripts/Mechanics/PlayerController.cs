using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;
using UnityEngine.InputSystem;
using System;

namespace Platformer.Mechanics
{
    /// <summary>
    /// This is the main class used to implement control of the player.
    /// It is a superset of the AnimationController class, but is inlined to allow for any kind of customisation.
    /// </summary>
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        /// <summary>
        /// Max horizontal speed of the player.
        /// </summary>
        public float maxSpeed = 7;
        /// <summary>
        /// Initial jump velocity at the start of a jump.
        /// </summary>
        public float jumpTakeOffSpeed = 7;
        public float speedMultiplier = 1f;
        public bool limitJumpHeight = false;
        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        /*internal new*/
        public Collider2D collider2d;
        /*internal new*/
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        private InputAction m_MoveAction;
        private InputAction m_JumpAction;

        public Bounds Bounds => collider2d.bounds;

        [Header("Ice settings")]
        IceSurface currentIce;
        float iceSpeed;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            m_MoveAction = InputSystem.actions.FindAction("Player/Move");
            m_JumpAction = InputSystem.actions.FindAction("Player/Jump");

            m_MoveAction.Enable();
            m_JumpAction.Enable();
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = m_MoveAction.ReadValue<Vector2>().x;
                if (jumpState == JumpState.Grounded && m_JumpAction.WasPressedThisFrame())
                    jumpState = JumpState.PrepareToJump;
                else if (m_JumpAction.WasReleasedThisFrame())
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }
            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                float currentJumpTakeOffSpeed = jumpTakeOffSpeed;

                if (limitJumpHeight)
                {
                    // Уменьшаем начальную скорость прыжка, чтобы ограничить высоту
                    currentJumpTakeOffSpeed *= 0.5f; // Можно настроить множитель
                }

                velocity.y = currentJumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            //targetVelocity = move * maxSpeed;
            if (currentIce != null)
            {
                // если игрок жмёт влево / вправо
                if (Mathf.Abs(move.x) > 0.01f)
                {
                    iceSpeed += currentIce.acceleration * Time.deltaTime;
                    iceSpeed = Mathf.Min(iceSpeed, currentIce.maxSpeed);
                }
                else
                {
                    // трение — медленно останавливаемся
                    iceSpeed = Mathf.Lerp(iceSpeed, 0, currentIce.friction);
                }

                float direction =
                    Mathf.Abs(move.x) > 0.01f
                    ? Mathf.Sign(move.x)
                    : Mathf.Sign(velocity.x);

                targetVelocity = new Vector2(direction * iceSpeed, velocity.y);
            }
            else
            {
                targetVelocity = move * maxSpeed * speedMultiplier;
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            var ice = collision.collider.GetComponent<IceSurface>();
            if (ice != null)
            {
                currentIce = ice;
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            var ice = collision.collider.GetComponent<IceSurface>();
            if (ice != null && ice == currentIce)
            {
                currentIce = null;
                iceSpeed = 0f;
            }
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }
    }
}