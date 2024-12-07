using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    /// <summary>
    /// Ограничение по времени на прохождение уровня
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [SerializeField] private float timeLimit = 300.0f;
        [SerializeField] private Text timerText;

        private float currentTime;
        private bool IsTimerRunning = false;

        private void Start()
        { 
            currentTime = timeLimit;
            UpdateTimerText();
            StartTimer();
        }

        private void Update()
        {
            if (IsTimerRunning)
            {
                currentTime -= Time.deltaTime;
                UpdateTimerText();

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    IsTimerRunning = false;
                    OnTimerEnd();
                }
            }
        }

        private void UpdateTimerText()
        {
            if (timerText != null)
            {
                timerText.text = "Время: " + Mathf.CeilToInt(currentTime).ToString();
            }
        }

        private void OnTimerEnd()
        {
            Debug.Log("Time`s up! Game Over."); // Логика для обработки конца таймера
        }

        public void StartTimer()
        {
            IsTimerRunning = true;
        }

        public void StopTimer()
        {
            IsTimerRunning = false;
        }
    }  
}