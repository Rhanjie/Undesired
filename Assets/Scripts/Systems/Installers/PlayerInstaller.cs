using Systems.Characters;
using Systems.Characters.Behaviours;
using Systems.Core.Characters.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Systems.Installers
{
    public class PlayerInstaller : MonoInstaller<PlayerInstaller>
    {
        [SerializeField] private CharacterMovement.Settings characterMovementSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<IMovement>()
                .To<CharacterMovement>()
                .WithArguments(characterMovementSettings)
                .WhenInjectedInto<Player>();

            Container.Bind<Player>().AsSingle();
        }
    }
}