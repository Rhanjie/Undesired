using System.Collections;
using Moq;
using NUnit.Framework;
using Systems.Characters;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace Tests.Playmode.Characters
{
    [TestFixture]
    public class CharacterMovementTests : ZenjectIntegrationTestFixture
    {
        private const string PlayerPrefabGuid = "3e3d3456b6262b141831c46d2e741df4";
        
        [Inject]
        private Character _character;

        [SetUp]
        public void CommonInstall()
        {
            PreInstall();

            var path = AssetDatabase.GUIDToAssetPath(PlayerPrefabGuid);
            var character = AssetDatabase.LoadAssetAtPath<Character>(path);

            Container.Bind<Character>().FromNewComponentOnNewPrefab(character).AsSingle();

            PostInstall();
        }
        
        [UnityTest]
        public IEnumerator Move_Character_In_Right_Direction()
        {
            var delta = Vector2.right;
            _character.Movement.PerformMove(delta);

            yield return null;
        
            Assert.Greater(_character.Movement.Position.x, 0);
        }
        
        [UnityTest]
        public IEnumerator Move_Character_In_Left_Direction()
        {
            var delta = Vector2.left;
            _character.Movement.PerformMove(delta);

            yield return null;
        
            Assert.Less(_character.Movement.Position.x, 0);
        }
        
        [UnityTest]
        public IEnumerator Grounded_Character_After_Jump_Is_Not_Grounded()
        {
            Assert.AreEqual(_character.Movement.IsGrounded, true);
            
            _character.Movement.PerformJump();

            yield return null;
        
            Assert.AreEqual(_character.Movement.IsGrounded, false);
        }
    }
}
