using System.Collections;
using NUnit.Framework;
using Systems.Characters;
using Systems.Core.Characters.Behaviours;
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
        private Character _player;
        
        private IMovement _movement;

        [SetUp]
        public void CommonInstall()
        {
            var path = AssetDatabase.GUIDToAssetPath(PlayerPrefabGuid);
            var character = AssetDatabase.LoadAssetAtPath<Character>(path);

            InitGround();
            
            PreInstall();

            Container.Bind<Character>().FromNewComponentOnNewPrefab(character).AsSingle();

            PostInstall();

            _movement = _player.Movement;
        }

        private void InitGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            Object.DestroyImmediate(ground.GetComponent<BoxCollider>());
            ground.AddComponent<BoxCollider2D>();
            
            ground.layer = LayerMask.NameToLayer("Walkable");
            ground.transform.position = new Vector3(0, -2f, 0);
            ground.transform.localScale = new Vector3 (2, 1, 2);
        }
        
        [UnityTest]
        public IEnumerator Move_Character_In_Right_Direction()
        {
            var delta = Vector2.right;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(0.2f);
        
            Assert.Greater(_movement.Position.x, 0);
            Assert.AreEqual(_movement.IsFacingRight, true);
        }
        
        [UnityTest]
        public IEnumerator Move_Character_In_Left_Direction()
        {
            var delta = Vector2.left;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(0.2f);
        
            Assert.Less(_movement.Position.x, 0);
            Assert.AreEqual(_movement.IsFacingRight, false);
        }
        
        [UnityTest]
        public IEnumerator Grounded_Character_After_Jump_Is_Not_Grounded()
        {
            yield return new WaitForSeconds(0.3f);
            
            Assert.AreEqual(_movement.IsGrounded, true);
            
            _movement.PerformJump();
        
            yield return new WaitForSeconds(0.2f);
        
            Assert.AreEqual(_movement.IsGrounded, false);
            
            yield return new WaitForSeconds(4);
            
            Assert.AreEqual(_movement.IsGrounded, true);
        }
    }
}
