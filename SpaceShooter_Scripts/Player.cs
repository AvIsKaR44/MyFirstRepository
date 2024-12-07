using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;
        
        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementController m_MovementController;

        /// <summary>
        /// Задержка взрыва, перед спавном нового корабля
        /// </summary>
        [SerializeField] private float m_RespawnDelay = 2.0f;

        private void Start()
        {
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
            {
                Invoke("Respawn", m_RespawnDelay);
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }

        private void Respawn()
        {
            if (m_PlayerShipPrefab != null)
            {
                var newPlayerShip = Instantiate(m_PlayerShipPrefab);

                // Уничтожаем старый корабль
                if (m_Ship != null)
                {
                    Destroy(m_Ship.gameObject);
                }

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                if (m_Ship != newPlayerShip)
                {
                    m_CameraController.SetTarget(m_Ship.transform);
                    m_MovementController.SetTargetShip(m_Ship);

                    var destructible = m_Ship.GetComponent<Destructible>();
                    if (destructible != null)
                    {
                        destructible.EventOnDeath.RemoveAllListeners();
                        destructible.EventOnDeath.AddListener(OnShipDeath);
                    }
                }
                else
                {
                    Debug.LogError("Failed to get SpaseShip component from newPlayerShip.");
                }
            }
            else
            {
                Debug.LogError("PlayerShipPrefab is null. Make sure it is assigned in the inspector.");
            }
        }
    }
}
