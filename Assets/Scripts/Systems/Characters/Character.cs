using Systems.Core.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Characters
{
    public class Character : MonoBehaviour
    {
        public IMovement Movement { get; set; }

        [Inject]
        public virtual void Construct(IMovement movement)
        {
            Movement = movement;
        }
    }
}