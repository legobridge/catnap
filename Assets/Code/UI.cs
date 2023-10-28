using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code
{
    public class UI : MonoBehaviour
    {
        public static UI Singleton;

        public TextMeshProUGUI PetsReceivedText;
        public TextMeshProUGUI TimeLeftText;
        public TextMeshProUGUI gameOverText;
        public float TimeLimit = 45;

        private int _numHumans;
        private int _petsReceived;
        private int _timeLeft;


        private void Start()
        {
            Singleton = this;
            ResetStatus();
        }

        private void ResetStatus()
        {
            TimeLimit += Time.time;
            _numHumans = FindObjectsOfType<HumanBehavior>().Length;
            _petsReceived = 0;
            _timeLeft = (int)(TimeLimit - Time.time);
            PetsReceivedText.text = $"Pets Received: {_petsReceived}/{_numHumans}";
            TimeLeftText.text = $"Time Left: {_timeLeft}";
            gameOverText.text = "";
        }

        private void Update()
        {
            _timeLeft = Mathf.Max(0, (int)(TimeLimit - Time.time));
            TimeLeftText.text = $"Time Left: {_timeLeft}";
            if (_timeLeft <= 0)
            {
                GameOver(false);
            }
            if (gameOverText.text != "")
            {
                if (Input.GetKeyDown("Escape"))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    ResetStatus();
                }
            }
        }

        public static void IncrementPets()
        {
            Singleton.IncrementPetsInternal();
        }

        private void IncrementPetsInternal()
        {
            _petsReceived++;
            PetsReceivedText.text = $"Pets Received: {_petsReceived}/{_numHumans}";

            if (_petsReceived == _numHumans)
            {
                // TODO - change pillow sprite
            }
        }

        public static void LandOnPillow()
        {
            Singleton.LandOnPillowInternal();
        }

        private void LandOnPillowInternal()
        {
            if (_petsReceived == _numHumans)
            {
                GameOver(true);
            }
        }

        public static void GameOver(bool win)
        {
            Singleton.GameOverInternal(win);
        }

        private void GameOverInternal(bool win)
        {
            if (win)
            {
                gameOverText.text = "You Win ^_^";
            }
            else
            {
                gameOverText.text = "Game Over >_<";
            }
            gameOverText.text += "\nPress Esc to Restart";
        }
    }
}