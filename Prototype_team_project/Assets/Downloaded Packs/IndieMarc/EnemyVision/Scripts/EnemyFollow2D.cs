using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.StealthLOS
{
    [RequireComponent(typeof(Enemy2D))]
    public class EnemyFollow2D : MonoBehaviour
    {
        public float speed_mult = 1f;
        public GameObject target;
        
        private Enemy2D enemy;

        void Start()
        {
            enemy = GetComponent<Enemy2D>();
            
        }

        void Update()
        {
            if (target == null)
                return;

            Vector3 targ = target.transform.position;
            enemy.MoveTo(targ, speed_mult);
            enemy.FaceToward(enemy.GetMoveTarget(), 2f);
        }

        public Enemy2D GetEnemy()
        {
            return enemy;
        }
    }

}