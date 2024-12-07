using UnityEngine;


namespace SpaceShooter
{

    public class Projectile : Entity, IHomingMissile
    {
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_TurnSpeed;

        [SerializeField] private float m_LifeTime;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect ImpactEffectPrefab;

        private float m_Timer;

        protected Transform[] m_Targets;

        public void SetTargets(Transform[] targets)
        {
            m_Targets = targets;
            Debug.Log($"Targets set: {m_Targets.Length}");
        }

        public void OnExplosion(Vector2 pos)
        {
            Destroy(gameObject);
        }


        void Update()
        {
            if (m_Targets != null && m_Targets.Length > 0)
            {
                Transform closestTarget = FindClosestTarget();
                if (closestTarget != null)
                {
                    Vector2 direction = (Vector2)closestTarget.position - (Vector2)transform.position;
                    direction.Normalize();
                    transform.up = Vector3.Lerp(transform.up, direction, Time.deltaTime * m_TurnSpeed);
                    Debug.Log($"Updating direction to target: {closestTarget.position}");
                }
                else
                {
                    Debug.Log("No closest target found.");
                }
            }
            else
            {
                Debug.Log("No targets set.");
            }
        

            float stepLenght = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (hit)
            {
                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);
                }
                OnProjectileLifeEnd(hit.collider, hit.point);
            }
            m_Timer += Time.deltaTime;

            if (m_Timer > m_LifeTime)
                Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, 0);
        }

        private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            Destroy(gameObject);
        }

        private Destructible m_Parent;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }

        private Transform FindClosestTarget()
        {
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (Transform target in m_Targets)
            {
                if (target == null) continue;

                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            return closestTarget;
        }     
    }   
}
