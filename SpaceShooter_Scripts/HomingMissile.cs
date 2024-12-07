using UnityEngine;

namespace SpaceShooter
{
    public interface IHomingMissile
    {
        void SetTargets(Transform[] targets);
        void OnExplosion(Vector2 pos);
    }

    public class HomingMissile : MonoBehaviour, IHomingMissile
    {
        private Transform[] m_Targets;
        private Transform m_CurrentTarget;
        private Rigidbody2D m_Rigidbody;

        [SerializeField] private float m_Speed = 10f;
        [SerializeField] private float m_RotationSpeed = 200f;

        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (m_CurrentTarget == null && m_Targets != null && m_Targets.Length > 0)
            {
                m_CurrentTarget = m_Targets[0];
            }

            if (m_CurrentTarget != null)
            {
                Vector2 direction = (Vector2)m_CurrentTarget.position - m_Rigidbody.position;
                direction.Normalize();

                float rotateAmount = Vector3.Cross(direction, transform.up).z;

                m_Rigidbody.angularVelocity = -rotateAmount * m_RotationSpeed;
                m_Rigidbody.velocity = transform.up * m_Speed;
            }
        }

        public void SetTargets(Transform[] targets)
        {
            m_Targets = targets;
        }

        public void OnExplosion(Vector2 pos)
        {
            // Логика взрыва ракеты
        }
    }
}