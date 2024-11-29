using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_SmashButton : QuickTimeEvent
    {
        [SerializeField] private int[] clicksToCompleteByDifficulty;
        [SerializeField] private float[] BackSpeedByDifficulty;
        [SerializeField] private float timeToLoose; //in seconds. If -1, infinite time 
        [SerializeField] private Image fillCircle;
        [SerializeField] private TMP_Text resultText;

        private bool done;
        private int clicksCount;

        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            fillCircle.color = ColorByDifficulty[difficulty];
            fillCircle.fillAmount = 0;

            done = false;
            clicksCount = 0;
            resultText.text = "빠르게 연타!!";
            StartCoroutine(RunQTE(difficulty));
        }

        public IEnumerator RunQTE(int difficulty)
        {
            StartCoroutine(DeathTimer());
            StartCoroutine(BackSpeedTriggerer(BackSpeedByDifficulty[difficulty]));

            while (!done)
            {
                fillCircle.fillAmount = (float)clicksCount / (float)clicksToCompleteByDifficulty[difficulty];
                done = clicksCount >= clicksToCompleteByDifficulty[difficulty];

                yield return new WaitForEndOfFrame();
            }

            StopAllCoroutines();

            if (clicksCount >= clicksToCompleteByDifficulty[difficulty])
            {
                resultText.text = "성공!!!";
                OnSuccess.Invoke();
            }
            else
            {
                resultText.text = "실패...";
                OnFail.Invoke();
            }
        }

        private IEnumerator DeathTimer()
        {
            if (timeToLoose > 0)
            {
                yield return new WaitForSeconds(timeToLoose);
                done = true;
            }
        }

        private IEnumerator BackSpeedTriggerer(float speed)
        {
            yield return new WaitForSeconds(speed);
            while (!done)
            {
                clicksCount = Math.Max(0, clicksCount - 1);
                yield return new WaitForSeconds(speed);
            }
        }

        private void Update()
        {
            if (!done)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) // 0은 왼쪽
                {
                    clicksCount++;
                }
            }
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}