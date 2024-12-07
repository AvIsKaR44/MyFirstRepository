using UnityEngine;


namespace SpaceShooter
{
    public class Turret : Projectile
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;
        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;
        [SerializeField] private Transform[] m_Target; // Массив целей
        
        private bool m_SecondaryTurret;       
        

        #region UNTY_EVENT
        private void Start()
        {
            m_Ship = transform.root.GetComponent<SpaceShip>();
        }

        private void Update()
        {
            if (m_RefireTimer > 0) 
            m_RefireTimer -= Time.deltaTime;
        }
        #endregion

        // Public API
        public void Fire() 
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;


                if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false)
                    return;

                if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false)         
                    return;                
           
            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetParentShooter(m_Ship);

            // Цель для ракет
            if (m_SecondaryTurret && projectile is IHomingMissile)
            {
                if (m_SecondaryTurret && m_Target != null)
                {
                    IHomingMissile missile = projectile as IHomingMissile;
                    missile.SetTargets(m_Target); // Передача массива целей
                    Debug.Log($"Targets set: {m_Target.Length}");
                }
            }
                       
            m_RefireTimer = m_TurretProperties.RateOfFire;

            {
                // SFX
            }
        }

        public void AssignLoadout(TurretProperties props)
        {
            if(m_Mode != props.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = props;           
        }

        private Transform FindClosestTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Target"); // "Target" заменить на тег целей в инспекторе
            Transform closestTarget = null;
            float closestDistance = Mathf.Infinity;

            foreach (GameObject target in targets)
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target.transform;
                }
            }

            return closestTarget;
        }
    }
}