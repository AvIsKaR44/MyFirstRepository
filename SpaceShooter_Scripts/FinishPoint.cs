using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class FinishPoint : MonoBehaviour
    {
        /// <summary>
        /// Настройки логики финиша(телепорт, таймер, сообщение о завершении уровня).
        /// </summary>
        [SerializeField] private Transform teleportPoint;
        [SerializeField] private float teleportDelay = 1f;
        [SerializeField] private Timer timer;
        [SerializeField] private Text levelCompleteText;

        private void Start()
        {

            if(levelCompleteText != null)
            {
                levelCompleteText.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Player entered the finish point!");

                // Отключение коллайдера
                GetComponent<Collider2D>().enabled = false;

                if (timer != null)
                {
                    timer.StopTimer();
                }

                StartCoroutine(TeleportPlayer(collision.transform));
            }
        }

        private IEnumerator TeleportPlayer(Transform playerTransform)
        {
            Debug.Log("Teleporting player in " + teleportDelay + " seconds...");

            yield return new WaitForSeconds(teleportDelay);

            Debug.Log("Teleporting player to " + teleportPoint.position);

            if (teleportPoint != null)
            {
                playerTransform.position = teleportPoint.position;


                var rb = playerTransform.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                }
            }

            if (levelCompleteText != null)
            {
                levelCompleteText.text = "Уровень пройден!";
                levelCompleteText.gameObject.SetActive(true);
            }

            // Скрытие сообщения о прохождении и остановка таймера после задержки
            yield return new WaitForSeconds(2f); // Задержка перед скрытием сообщения

            if (levelCompleteText != null)
            {
                levelCompleteText.gameObject.SetActive(false);
            }

            if (timer != null)
            {
                timer.StopTimer();
                timer.gameObject.SetActive(false);
            }
        }
    }
}
