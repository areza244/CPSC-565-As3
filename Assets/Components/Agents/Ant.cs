using System.Collections.Generic;
using UnityEngine;

namespace Antymology.Terrain
{
    public class Ant : MonoBehaviour
    {
        public float Health { get; private set; }
        private float healthDecreaseRate = 1.0f; 
        private float maxHealth = 100.0f;
        void Start()
        {
            Health = maxHealth;
        }

        void Update()
        {
            UpdateHealth();

        }
        private void UpdateHealth()
        {
            if (IsOnAcidicBlock())
            {
                Health -= 2 * healthDecreaseRate;
            }
            else
            {
                Health -= healthDecreaseRate;
            }

            if (Health <= 0)
            {
                Die();
            }
        }

        public void ConsumeMulch()
        {
            if (IsOnMulch())
            {
                Health = maxHealth;
                RemoveMulchBlock(); 
            }
        }

        private bool IsOnAcidicBlock()
        {
            
        }

        private bool IsOnMulch()
        {
            
        }

        private void RemoveMulchBlock()
        {

        }

        private void Die()
        {
        Destroy(gameObject);
        }

        public void ProduceNestBlock()
        {
             
        }  
        public void digup()
        {

        }
    }
}