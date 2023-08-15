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
        
        private const float SmallDuration = 0.2f;
        private const float MediumDuration = 2f;
        private const float LongDuration = 5f;

        [SetUp]
        public void CommonInstall()
        {
            var path = AssetDatabase.GUIDToAssetPath(PlayerPrefabGuid);
            var character = AssetDatabase.LoadAssetAtPath<Character>(path);

            InitBox(new Vector2(0, -2f), new Vector2 (8, 1));
            InitBox(new Vector2(4, 0f), new Vector2(1, 3));

            PreInstall();

            Container.Bind<Character>().FromNewComponentOnNewPrefab(character).AsSingle();

            PostInstall();

            _movement = _player.Movement;
        }

        private void InitBox(Vector2 position, Vector2 scale)
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            Object.DestroyImmediate(ground.GetComponent<BoxCollider>());
            ground.AddComponent<BoxCollider2D>();
            
            ground.layer = LayerMask.NameToLayer("Walkable");
            ground.transform.position = new Vector3(position.x, position.y, 0f);
            ground.transform.localScale = new Vector3(scale.x, scale.y, 1f);;
        }
        
        [UnityTest]
        public IEnumerator Move_Small_Distance_In_Right_Direction()
        {
            var delta = Vector2.right;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(SmallDuration);
        
            Assert.Greater(_movement.Position.x, 0);
            Assert.AreEqual(_movement.IsFacingRight, true);
        }

        [UnityTest]
        public IEnumerator Move_Long_Distance_In_Right_Direction_And_Block_On_Obstacle()
        {
            var delta = Vector2.right;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(MediumDuration);
        
            Assert.Greater(_movement.Position.x, 0);
            Assert.LessOrEqual(_movement.Position.x, 3f);
        }
        
        [UnityTest]
        public IEnumerator Move_Small_Distance_In_Left_Direction()
        {
            var delta = Vector2.left;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(SmallDuration);
        
            Assert.Less(_movement.Position.x, 0);
            Assert.AreEqual(_movement.IsFacingRight, false);
        }
        
        [UnityTest]
        public IEnumerator Move_Long_Distance_In_Left_Direction()
        {
            var delta = Vector2.left;
            _movement.PerformMove(delta);

            yield return new WaitForSeconds(MediumDuration);
        
            Assert.LessOrEqual(_movement.Position.x, -3f);
        }

        [UnityTest]
        public IEnumerator Jump_To_Top_Without_Obstacles()
        {
            yield return new WaitForSeconds(SmallDuration);
            
            Assert.AreEqual(_movement.IsGrounded, true);
            
            _movement.PerformJump();
        
            yield return new WaitForSeconds(SmallDuration);
        
            Assert.AreEqual(_movement.IsGrounded, false);
            
            yield return new WaitForSeconds(LongDuration);
            
            Assert.AreEqual(_movement.IsGrounded, true);
        }
    }
}
