using Systems.Characters.Behaviours;
using UnityEngine;
using Zenject;

namespace Systems.Installers
{
    [CreateAssetMenu(fileName = "PlayerInstaller", menuName = "Installers/PlayerInstaller")]
    public class PlayerSettingsInstaller : ScriptableObjectInstaller<PlayerSettingsInstaller>
    {
        [SerializeField] private CharacterMovement.Settings characterMovementSettings;
        
        public override void InstallBindings()
        {
            Container.Bind<CharacterMovement.Settings>().FromInstance(characterMovementSettings).AsSingle();
        }
    }
}