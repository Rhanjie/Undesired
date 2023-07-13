using System;
using Systems.Core.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Characters.Behaviours
{
    public class CharacterMovement : IMovement
    {
        readonly Settings _settings;

        private float _horizontal;
        private bool _isFacingRight;
        
        //TODO: Move to scriptable object
        private float _speed = 10f;
        private float _jumpingPower = 5f;

        [Inject]
        public CharacterMovement(Settings settings)
        {
            _settings = settings;

            Debug.Log($"Dependencies have been injected to movement:\n" +
                      $"{_settings.transformHandler.name}->{_settings.transformHandler.GetType()}\n" +
                      $"{_settings.dynamicComponent.name}->{_settings.dynamicComponent.GetType()}\n" +
                      $"{_settings.feetPoint.name}->{_settings.feetPoint.GetType()}");
        }

        public void PerformMove(Vector2 delta)
        {
            _horizontal = delta.x;

            var verticalVelocity = _settings.dynamicComponent.velocity.y;
            _settings.dynamicComponent.velocity = new Vector2(_horizontal * _speed, verticalVelocity);
            
            if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
                Flip();
            
            Debug.Log($"Move performed {_horizontal}");
        }

        public void PerformJump()
        {
            if (!IsGrounded())
                return;
            
            var horizontalVelocity = _settings.dynamicComponent.velocity.x;
            _settings.dynamicComponent.velocity = new Vector2(horizontalVelocity, _jumpingPower);
            
            Debug.Log($"Jump performed");
        }
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(_settings.feetPoint.position, 0.2f, LayerMask.NameToLayer("Walkable"));
        }

        private void Flip()
        {
            _isFacingRight = !_isFacingRight;

            var localScale = _settings.transformHandler.localScale;
            localScale.x *= -1f;

            _settings.transformHandler.localScale = localScale;
        }

        [Serializable]
        public class Settings
        {
            public Transform transformHandler;
            public Rigidbody2D dynamicComponent;
            public Transform feetPoint;
        }
    }
}