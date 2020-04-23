using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.StealthLOS
{
    [RequireComponent(typeof(EnemyLOS2D))]
    public class EnemyDemo2D : MonoBehaviour
    {
        public GameObject exclama_prefab;
        public GameObject death_fx_prefab;

        private EnemyLOS2D enemy;
        private Animator animator;
        

        void Start()
        {
            animator = GetComponentInChildren<Animator>();
            enemy = GetComponent<EnemyLOS2D>();
            enemy.onDeath += OnDeath;
            enemy.onSeeTarget += OnSeen;
            
        }

        void Update()
        {
            //animation
            animator.SetBool("Move", enemy.GetEnemy().GetMove().magnitude > 0.5f);
            //animator.SetBool("Run", is_running);
        }

        private void OnSeen(VisionTarget target)
        {
            if (exclama_prefab)
            {
                Instantiate(exclama_prefab, transform);
            }
        }

        private void OnDeath()
        {
            if(death_fx_prefab)
                Instantiate(death_fx_prefab, transform.position + Vector3.up * 0.5f, death_fx_prefab.transform.rotation);
        }
    }
}
