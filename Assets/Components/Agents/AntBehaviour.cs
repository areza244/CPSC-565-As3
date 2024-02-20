using System;
using System.Collections.Generic;
using UnityEngine;

namespace Antymology.Terrain
{
    public class AntBehaviour : MonoBehaviour
    {
        public float Health 
        { 
            get; private set; 
        }
        private float healthDecreaseRate = 1.0f;
        // ant max health is 50
        private float maxHealth = 50.0f;
        private bool flag = false;
        // air block for when digging and eating Mulch
        private AbstractBlock empty = new AirBlock();
        // nest block for nest production
        private AbstractBlock nest = new NestBlock();
        // checking to see if there is already a queen
        private static bool queenExists = false;
        public bool IsQueen 
        { 
            get; private set; 
        }

        void Start()
        {
            // initialization of the ant Queen
            if (!queenExists)
            {
                IsQueen = true;
                queenExists = true;
            }
            else
            {
                IsQueen = false;
            }
            // setting the ant's health
            Health = maxHealth;
        }

        /// <summary>
        /// Calling all of the functions on each frame
        /// </summary>
        void Update()
        {
            Movement();
            UpdateHealth();
            ConsumeMulch();
            ProduceNestBlock();
            CheckDig();
        }

        private void Movement()
        {
            Vector3 currentPosition = transform.position;
            List<Vector3> potentialPositions = new List<Vector3>();
            List<Vector3> approvedPositions = new List<Vector3>();

            /// <summary>
            /// finding all the possible movements for the ants in all direction
            /// but they can't go higher than +2 elevation
            /// </summary>
            int[] horizontalMoves = new int[] { 1, -1, 0, 0 };
            int[] verticalMoves = new int[] { 0, 0, 1, -1 };
            int[] elevation = new int[] { 2, 1, -1, -2, 0 }; 

            foreach (int h in horizontalMoves)
            {
                foreach (int v in verticalMoves)
                {
                    foreach (int e in elevation)
                    {
                        if (h == 0 && v == 0 && e == 0) 
                        {
                            continue;
                        }
                        Vector3 nextPosition = new Vector3(currentPosition.x + h, currentPosition.y + e, currentPosition.z + v);
                        potentialPositions.Add(nextPosition);
                    }
                }
            }

            /// <summary>
            /// Checking to see if the available next positions are empty/air
            /// </summary>
            foreach (var position in potentialPositions)
            {
                if (BlockTypeOn(position) != "AirBlock")
                {
                    if (BlockTypeUp(position) == "AirBlock")
                    {
                        flag = true;
                        approvedPositions.Add(position);
                    }
                }
            }

            /// <summary>
            /// if moving positions are available the ant will move to a random position
            /// </summary>
            if (flag == true)
            {        
                var select = approvedPositions[UnityEngine.Random.Range(0, approvedPositions.Count)];
                select.y = select.y + 0.57f; 
                this.transform.position = select;
            } 
        }

        /// <summary>
        /// Ants health will decrease at a certain rate
        /// if they are standing on a acidic block they will lose health twice the normal rate
        /// </summary>
        private void UpdateHealth()
        {
            if (IsOnAcidicBlock() == true)
            {
                this.Health = this.Health - (2 * healthDecreaseRate);
            }
            else
            {
                this.Health = this.Health - healthDecreaseRate;
            }

            if (this.Health <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// If the ant is on a Mulch block it will eat it and restore health
        /// </summary>
        public void ConsumeMulch()
        {
            if (IsOnMulch() == true)
            {
                this.Health = maxHealth;
                RemoveMulchBlock(this.transform.position, empty); 
            }
        }

        /// <summary>
        /// Check to see if the ant is standing on a Acidic block
        /// </summary>
        private bool IsOnAcidicBlock()
        {
            if (BlockTypeOn(this.transform.position) == "AcidicBlock")
                {
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Check to see if the ant is standing on a Mulch block
        /// </summary>
        private bool IsOnMulch()
        {
            if (BlockTypeOn(this.transform.position) == "MulchBlock")
                {
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Removing the eaten Mulch blocks and placing air block in its place
        /// </summary>
        private void RemoveMulchBlock(Vector3 position, AbstractBlock block)
        {
            int x = Mathf.RoundToInt(position.x);
            int y = Mathf.RoundToInt(position.y);
            int z = Mathf.RoundToInt(position.z);
            WorldManager.Instance.SetBlock(x, y, z, block);
        }

        /// <summary>
        /// Destroying the dead ants if they reach zero health
        /// </summary>
        private void Die()
        {
            Destroy(gameObject);
        }

        /// <summary>
        /// If the ant is the Queen ant it will produce a nest block on its position
        /// </summary>
        public void ProduceNestBlock()
        {
            if (IsQueen)
            {
                float healthCost = maxHealth / 3f;
                if (Health > healthCost)
                {
                    Health = Health - healthCost;
                    Vector3 position = this.transform.position;
                    WorldManager.Instance.SetBlock(
                        Mathf.RoundToInt(position.x), 
                        Mathf.RoundToInt(position.y + 1), 
                        Mathf.RoundToInt(position.z),
                        nest);
                }
            }  
        }  

        /// <summary>
        /// Check to see if the ant can dig up the block its standing on
        /// </summary>
        public void CheckDig()
        {
            if (IsOnContainer() == false)
            {
                if (BlockTypeOn(this.transform.position) == "GrassBlock")
                {
                    DigUp(this.transform.position, empty);
                }
            }
        }

        /// <summary>
        /// Removing/Digup the block that the ant is standing on
        /// </summary>
        public void DigUp(Vector3 position, AbstractBlock block)
        {
            WorldManager.Instance.SetBlock(
                Mathf.RoundToInt(position.x), 
                Mathf.RoundToInt(position.y), 
                Mathf.RoundToInt(position.z),
                block);
        }

        /// <summary>
        /// Check to see if the ant is standing on a containerblock
        /// </summary>
        public bool IsOnContainer()
        {
            if (BlockTypeOn(this.transform.position) == "ContainerBlock")
                {
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Get the block type of the block the ant is standing on
        /// </summary>
        string BlockTypeOn(Vector3 position)
        {
            AbstractBlock block = WorldManager.Instance.GetBlock(
                Mathf.RoundToInt(position.x), 
                Mathf.RoundToInt(position.y), 
                Mathf.RoundToInt(position.z));
            return block.BlockType;
        }

        /// <summary>
        /// Get the block type of the block on top of the ant
        /// </summary>
        string BlockTypeUp(Vector3 position)
        {
            AbstractBlock block = WorldManager.Instance.GetBlock(
                Mathf.RoundToInt(position.x), 
                Mathf.RoundToInt(position.y) + 1, 
                Mathf.RoundToInt(position.z));
            return block.BlockType;
        }
      
    }
}