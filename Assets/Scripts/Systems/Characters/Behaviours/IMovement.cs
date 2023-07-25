using UnityEngine;

namespace Systems.Core.Characters.Behaviours
{
    public interface IMovement
    {
        public Vector2 Position { get; }
        public bool IsGrounded { get; }

        public void PerformMove(Vector2 delta);
        public void PerformJump();
    }
}