using System;
using Systems.Core.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Characters.Behaviours
{
    public class CharacterMovement : IMovement, IFixedTickable
    {
        private readonly References _references;
        private readonly Settings _settings;

        private float _horizontal;
        private bool _isFacingRight = true;

        private readonly int _walkableLayerMask = LayerMask.GetMask("Walkable");

        [Inject]
        public CharacterMovement(References references, Settings settings)
        {
            _references = references;
            _settings = settings;
        }
        
        public void FixedTick()
        {
            var horizontalVelocity = _horizontal * _settings.speed * Time.fixedDeltaTime;
            var verticalVelocity = _references.dynamicComponent.velocity.y;

            _references.dynamicComponent.velocity = new Vector2(horizontalVelocity, verticalVelocity);
        }

        public void PerformMove(Vector2 delta)
        {
            _horizontal = delta.x;
            
            if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
                Flip();
        }

        public void PerformJump()
        {
            if (!IsGrounded())
                return;
            
            var horizontalVelocity = _references.dynamicComponent.velocity.x;
            _references.dynamicComponent.velocity = new Vector2(horizontalVelocity, _settings.jumpingPower);
        }
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(_references.feetPoint.position, 0.4f, _walkableLayerMask);
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;

            var localScale = _references.transformHandler.localScale;
            localScale.x *= -1f;

            _references.transformHandler.localScale = localScale;
        }

        [Serializable]
        public class References
        {
            public Transform transformHandler;
            public Rigidbody2D dynamicComponent;
            public Transform feetPoint;
        }

        [Serializable]
        public class Settings
        {
            public float speed = 200f;
            public float jumpingPower = 2f;
        }
    }
}