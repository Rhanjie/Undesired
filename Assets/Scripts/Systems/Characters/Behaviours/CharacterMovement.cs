using System;
using Systems.Core.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Characters.Behaviours
{
    public class CharacterMovement : IMovement, IFixedTickable
    {
        public Vector2 Position => _references.transformHandler.position;
        public bool IsGrounded => Physics2D.OverlapCircle(_references.feetPoint.position, 0.2f, _walkableLayerMask);
        public bool IsFacingRight { get; private set; } = true;
        
        private readonly References _references;
        private readonly Settings _settings;

        private float _horizontal;

        private readonly int _walkableLayerMask = LayerMask.GetMask("Walkable");

        [Inject]
        public CharacterMovement(References references, Settings settings)
        {
            _references = references;
            _settings = settings;
        }

        public void FixedTick()
        {
            CalculateMovement();
            CalculateFriction();
        }

        private void CalculateMovement()
        {
            var targetSpeed = _horizontal * _settings.speed;
            var speedDifference = targetSpeed - _references.rigidbody.velocityX;

            var accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f)
                ? _settings.acceleration
                : _settings.decceleration;

            var direction = Mathf.Sign(speedDifference);
            var force = Mathf.Abs(speedDifference) * accelerationRate;
            var movement = Mathf.Pow(force, _settings.velocityPower) * direction;
            
            _references.rigidbody.AddForce(movement * Vector2.right);
        }

        private void CalculateFriction()
        {
            if (!IsGrounded || Mathf.Abs(_horizontal) >= 0.01f)
                return;

            var amount = Mathf.Min(Mathf.Abs(_references.rigidbody.velocityX), Mathf.Abs(_settings.frictionAmount));
            amount *= Mathf.Sign(_references.rigidbody.velocityX);
            
            _references.rigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        public void PerformMove(Vector2 delta)
        {
            _horizontal = delta.x;
            
            if (IsFacingRight && _horizontal < 0f || !IsFacingRight && _horizontal > 0f)
                Flip();
        }

        public void PerformJump()
        {
            if (!IsGrounded)
                return;
            
            var horizontalVelocity = _references.rigidbody.velocity.x;
            _references.rigidbody.velocity = new Vector2(horizontalVelocity, _settings.jumpingPower);
        }
        
        
        private void Flip()
        {
            IsFacingRight = !IsFacingRight;

            var localScale = _references.transformHandler.localScale;
            localScale.x *= -1f;

            _references.transformHandler.localScale = localScale;
        }

        [Serializable]
        public class References
        {
            public Transform transformHandler;
            public Rigidbody2D rigidbody;
            public Transform feetPoint;
        }

        [Serializable]
        public class Settings
        {
            public float speed = 20f;
            public float jumpingPower = 2f;
            
            public float acceleration = 13;
            public float decceleration = 16;

            public float velocityPower = 0.96f;
            public float frictionAmount = 2.5f;
        }
    }
}