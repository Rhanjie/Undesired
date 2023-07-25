using Systems.Characters;
using Systems.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Installers
{
    public class PlayerReferencesInstaller : MonoInstaller<PlayerReferencesInstaller>
    {
        [SerializeField] private CharacterMovement.References characterMovementReferences;

        public override void InstallBindings()
        {
            Container.Bind<CharacterMovement.References>().FromInstance(characterMovementReferences).AsSingle();
            
            Container.BindInterfacesTo<CharacterMovement>().AsSingle();

            Container.Bind<Player>().AsSingle();
        }
    }
}