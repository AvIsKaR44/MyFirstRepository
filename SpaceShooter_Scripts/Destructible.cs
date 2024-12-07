using UnityEngine;
using UnityEngine.Events;


namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект на сцене. То что может иметь хитпоинты.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        /// <summary>
        /// Object ignor damage.
        /// </summary>
        [SerializeField] private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Starting hitpoints.
        /// </summary>
        [SerializeField] private int m_HitPoints;


        /// <summary>
        /// Current hitpoints
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;
        /// <summary>
        /// Префаб взрыва корабля
        /// </summary>
        [SerializeField] private GameObject m_ExplosionPrefab; 

        #endregion

        #region Unity Event

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }

        #endregion

        #region Public API
        /// <summary>
        /// Apply damage an to object
        /// </summary>
        /// <param name="damage">Damage dealt to an object</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0)
                OnDeath();

        }

        #endregion
        /// <summary>
        /// Переопределяемое событие уничтожения объекта, когда хитпоинты ниже нуля. (An overridden object destruction event when the hit is below zero).
        /// </summary>
        protected virtual void OnDeath()
        {
            if(m_ExplosionPrefab != null)
            {
               GameObject explosion = Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);

                Destroy(explosion, 2.0f);
            }

            if(!IsOnFinishPoint())
            {
                Destroy(gameObject);
                m_EventOnDeath?.Invoke();
            }          
        }

        #region IsOnFinishPoint
        /// <summary>
        /// Проверка нахождения корабля на точке финиша
        /// </summary>
        /// <returns></returns>
        private bool IsOnFinishPoint()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("FinishPoint"))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;
    }
}
