using UnityEngine;

namespace Systems.Core.Characters.Behaviours
{
    public interface IMovement
    {
        public void PerformMove(Vector2 delta);
        public void PerformJump();
    }
}