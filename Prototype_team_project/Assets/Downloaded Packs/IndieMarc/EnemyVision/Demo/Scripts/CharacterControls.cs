using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.StealthLOS
{
    public class CharacterControls : MonoBehaviour
    {
        public float move_speed = 7f;
        public float move_accel = 40f;

        private Vector3 current_move = Vector3.zero;
        private Rigidbody rigid;
        private Animator animator;

        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void FixedUpdate()
        {
            Vector3 move_dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                move_dir += Vector3.forward;
            if (Input.GetKey(KeyCode.A))
                move_dir += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                move_dir += Vector3.right;
            if (Input.GetKey(KeyCode.S))
                move_dir += Vector3.back;

            move_dir = move_dir.normalized * Mathf.Min(move_dir.magnitude, 1f);
            current_move = Vector3.MoveTowards(current_move, move_dir, move_accel * Time.fixedDeltaTime);
            rigid.velocity = current_move * move_speed;

            animator.SetBool("Move", current_move.magnitude > 0.1f);
        }
    }

}
