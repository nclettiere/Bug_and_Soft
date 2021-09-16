using System;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem.Data
{
    [Serializable]
    public class PlayerData
    {
        public int Level => level;

        public int Health => health;

        public int Krones => krones;

        public float[] Position => position;

        public int Checkpoint => checkpoint;

        public int CurrentAbility => currentAbility;

        public PlayerData([NotNull] PlayerController pctrl)
        {
            level = SceneManager.GetActiveScene().buildIndex;;
            health = pctrl.currentHealth;
            krones = GameManager.Instance.PlayerKrowns;
            checkpoint = GameManager.Instance.checkpointIndex;

            position = new float[3];
            FillPosition(pctrl.transform.position);
        }

        private void FillPosition(Vector3 position)
        {
            this.position[0] = position.x;
            this.position[1] = position.y;
            this.position[2] = position.z;
        }
        
        private void FillPosition(Vector2 position)
        {
            this.position[0] = position.x;
            this.position[1] = position.y;
            this.position[2] = 0f;
        }
        
        public Vector3 GetPosition()
        {
            return new Vector3(position[0], position[1], position[2]);
        }

        private int level;
        private int health;
        private int krones;
        private float[] position;
        private int checkpoint;
        private int currentAbility;
    }
}