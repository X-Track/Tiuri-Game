using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.StealthLOS
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyFollow : MonoBehaviour
    {
        public float speed_mult = 1f;
        public GameObject target;
        
        private Enemy enemy;

        private Vector3 last_seen_pos;

        void Start()
        {
            enemy = GetComponent<Enemy>();
            
        }

        void Update()
        {
            if (target == null)
                return;
            
            Vector3 targ = target.transform.position;
            enemy.MoveTo(targ, speed_mult);
            enemy.FaceToward(enemy.GetMoveTarget(), 2f);
        }

        public Enemy GetEnemy()
        {
            return enemy;
        }
    }

}