using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace IndieMarc.StealthLOS
{

    public enum EnemyLOSState
    {
        Patrol = 0,
        Alert = 5,
        Chase = 10,
    }

    [RequireComponent(typeof(Enemy))]
    public class EnemyLOS : MonoBehaviour
    {
        [Header("Detection")]
        public float vision_angle = 30f;
        public float vision_range = 10f;
        public float touch_range = 1f;
        public float detect_time = 1f;
        public LayerMask vision_mask = ~(0);

        [Header("Chase")]
        public float follow_time = 10f;

        [Header("Ref")]
        public Transform eye;
        public GameObject vision_prefab;
        public GameObject death_fx_prefab;

        public UnityAction<VisionTarget> onSeeTarget; //As soon as seen (Patrol->Alert)
        public UnityAction<VisionTarget> onDetectTarget; //detect_time seconds after seen (Alert->Chase)
        public UnityAction<VisionTarget> onTouchTarget;
        public UnityAction onDeath;

        private EnemyPatrol enemy_patrol;
        private EnemyFollow enemy_follow;
        private Enemy enemy;

        private EnemyLOSState state = EnemyLOSState.Patrol;
        private VisionTarget seen_character = null;
        private EnemyVision vision;
        public GameObject player;
        private float state_timer = 0f;







        
        void Start()
        {
            enemy_patrol = GetComponent<EnemyPatrol>();
            enemy_follow = GetComponent<EnemyFollow>();
            enemy = GetComponent<Enemy>();
            enemy.onDeath += OnDeath;

            if (vision_prefab)
            {
                GameObject vis = Instantiate(vision_prefab, GetEye(), Quaternion.identity);
                vis.transform.parent = transform;
                vision = vis.GetComponent<EnemyVision>();
                vision.target = this;
                vision.vision_angle = vision_angle;
                vision.vision_range = vision_range;
            }
            
            ChangeState(EnemyLOSState.Patrol);
        }

        void Update()
        {
            state_timer += Time.deltaTime;
            
            DetectVisionTarget();
            DetectTouchVisionTarget();
            //ChangeVision(vision_range, vision_angle);


            if (state == EnemyLOSState.Alert)
            {
                if (seen_character == null || !seen_character.CanBeSeen())
                {
                    seen_character = null;
                    ChangeState(EnemyLOSState.Patrol);
                    return;
                }

                Vector3 targ = seen_character.transform.position;
                enemy.FaceToward(targ);

                if (state_timer > detect_time)
                {
                    if (enemy_follow && seen_character.CanBeSeen())
                    {
                        ChangeState(EnemyLOSState.Chase);
                        enemy_follow.target = seen_character.gameObject;

                        if (onDetectTarget != null)
                            onDetectTarget.Invoke(seen_character);
                    }
                    else
                        ChangeState(EnemyLOSState.Patrol);
                }
            }

            if (state == EnemyLOSState.Chase)
            {
                if (seen_character == null) {
                    ChangeState(EnemyLOSState.Patrol);
                    return;
                }

                if (state_timer > follow_time)
                {
                    if(!seen_character.CanBeSeen() || !CanSeeObject(seen_character.gameObject))
                        ChangeState(EnemyLOSState.Patrol);
                }
            }
            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0f;
            if (dir.magnitude < touch_range) //In range and in angle
            {
                Yeetenator();
            }
        }

        public void ChangeVision(float visionSize, float visionAngle)
        {
                vision_range = visionSize;
                vision_angle = visionAngle;

                vision.vision_angle = vision_angle;
                vision.vision_range = vision_range;
        }

        public void DetectVisionTarget()
        {
            //Detect character
            foreach (VisionTarget character in VisionTarget.GetAll())
            {
                if (character == seen_character)
                    continue;

                if (CanSeeObject(character.gameObject))
                {
                    seen_character = character;
                    ChangeState(EnemyLOSState.Alert);

                    if (onSeeTarget != null)
                        onSeeTarget.Invoke(seen_character);
                }
            }
        }

        public void DetectTouchVisionTarget()
        {
            //Detect character touch
            foreach (VisionTarget character in VisionTarget.GetAll())
            {
                if (character == seen_character)
                    continue;

                if (CanTouchObject(character.gameObject))
                {
                    seen_character = character;
                    ChangeState(EnemyLOSState.Alert);

                    if (onSeeTarget != null)
                        onSeeTarget.Invoke(seen_character);
                }
            }
        }

        public bool CanSeeObject(GameObject obj, float range_offset = 0f, float angle_offset = 0f)
        {
            Vector3 forward = transform.forward;
            Vector3 dir = obj.transform.position - GetEye();
            float vis_range = vision_range + range_offset;
            float vis_angle = vision_angle + angle_offset;
            float losangle = Vector3.Angle(forward, dir);
            if ((losangle < vis_angle / 2f && dir.magnitude < vis_range)) //In range and in angle
            {
                RaycastHit hit;
                bool raycast = Physics.Raycast(new Ray(GetEye(), dir.normalized), out hit, vis_range, vision_mask.value);
                if (raycast && (hit.collider.gameObject == obj || hit.collider.transform.IsChildOf(obj.transform))) //See character
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanTouchObject(GameObject obj)
        {
            Vector3 dir = obj.transform.position - transform.position;
            dir.y = 0f;
            if (dir.magnitude < touch_range) //In range and in angle
            {
                return true;
            }
            return false;
        }

        private void Yeetenator()
        {
            Debug.Log("huts");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ChangeState(EnemyLOSState state)
        {
            this.state = state;
            state_timer = 0f;

            if (state == EnemyLOSState.Patrol)
                seen_character = null;

            if (enemy_patrol)
                enemy_patrol.enabled = (state == EnemyLOSState.Patrol);
            if(enemy_follow)
                enemy_follow.enabled = (state == EnemyLOSState.Chase);
        }

        public Vector3 GetEye()
        {
            return eye ? eye.position : transform.position;
        }

        public Enemy GetEnemy()
        {
            return enemy;
        }

        private void OnDeath()
        {
            if(vision)
                vision.gameObject.SetActive(false);

            if (onDeath != null)
                onDeath.Invoke();
        }
    }

}
