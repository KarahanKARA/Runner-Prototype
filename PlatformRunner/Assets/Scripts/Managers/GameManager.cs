using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject confettiVFX;
        [SerializeField] private GameObject halloweenVFX;
        [SerializeField] private Transform paintPointTransform;
        [SerializeField] private GameObject slider;
        [SerializeField] private GameObject homeButton;
        [SerializeField] private GameObject deathScreenUI;
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private List<GameObject> competitionUsers = new List<GameObject>();

        private bool _canPlayerSwipe;

        public bool CanPlayerSwipe
        {
            get { return _canPlayerSwipe; }
            set { _canPlayerSwipe = value; }
        }

        private bool _canPlayerMoveToForward;

        public bool CanPlayerMoveToForward
        {
            get { return _canPlayerMoveToForward; }
            set { _canPlayerMoveToForward = value; }
        }

        private bool _canPlayerPaint;

        public bool CanPlayerPaint
        {
            get { return _canPlayerPaint; }
            set
            {
                _canPlayerPaint = value;
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    return;
                }

                slider.SetActive(CanPlayerPaint);
            }
        }

        private bool _isPlayerFinishPaint;

        public bool IsPlayerFinishPaint
        {
            get { return _isPlayerFinishPaint; }
            set { _isPlayerFinishPaint = value; }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            CanPlayerSwipe = true;
            CanPlayerMoveToForward = true;
            CanPlayerPaint = false;
            IsPlayerFinishPaint = false;
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                if (CanPlayerSwipe)
                {
                    List<float> zPosList = new List<float>();
                    float playerZpos = 0;
                    int rank = 0;
                    foreach (var contestant in competitionUsers)
                    {
                        zPosList.Add(contestant.transform.position.z);
                        if (contestant.tag == "Player")
                        {
                            playerZpos = contestant.transform.position.z;
                        }
                    }

                    zPosList.Sort();
                    int counter = 12;
                    foreach (var VARIABLE in zPosList)
                    {
                        counter--;
                        if (Math.Abs(VARIABLE - playerZpos) < .03f)
                        {
                            rank = counter;
                        }
                    }
                    rankText.text = "RANK #" + rank;
                }
            }
        }

        public void OpenConfettiVFX()
        {
            confettiVFX.SetActive(true);
        }

        public void OpenHalloweenVFX()
        {
            halloweenVFX.SetActive(true);
            IsPlayerFinishPaint = true;
            homeButton.GetComponent<HearthBeatEffect>().enabled = true;
        }

        public Vector3 GetPaintPointPosition()
        {
            return paintPointTransform.position;
        }

        public void HomeButtonOnClick()
        {
            homeButton.GetComponent<HearthBeatEffect>().enabled = false;
            SceneManager.LoadScene(0);
        }

        public void OnDeathUI()
        {
            SceneManager.LoadScene(1);
            deathScreenUI.SetActive(false);
        }

        public void DeathScreenUIActivity()
        {
            StartCoroutine(OpenDeathScreenWithDelay());
        }

        private IEnumerator OpenDeathScreenWithDelay()
        {
            yield return new WaitForSeconds(2f);
            deathScreenUI.SetActive(true);
        }
        
    }
}