using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QTEPack
{
    public class QTE_Ring : QuickTimeEvent
    {
        [SerializeField] private float[] RingSpeedByDifficulty;
        [SerializeField] private Image ringImage;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text keyPromptText;
        private QTEHitResult done;
        private float currentRingScale;
        private List<QTEEVent>[] KeysByDifficulty;
        private List<QTEEVent> currentKeyEvents;
        private int currentKeyIndex = 0;

        protected override void Initialize()
        {
            base.Initialize();

            KeysByDifficulty = new List<QTEEVent>[5];

            KeysByDifficulty[0] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.Q), new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.E), new QTEEvent_Key(KeyCode.R),
                                                        new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_Key(KeyCode.F) };

            KeysByDifficulty[1] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.Q), new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.E), new QTEEvent_Key(KeyCode.R),
                                                        new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_Key(KeyCode.F) };

            KeysByDifficulty[2] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.Q), new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.E), new QTEEvent_Key(KeyCode.R),
                                                        new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_Key(KeyCode.F) };

            KeysByDifficulty[3] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.Q), new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.E), new QTEEvent_Key(KeyCode.R),
                                                        new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_Key(KeyCode.F) };

            KeysByDifficulty[4] = new List<QTEEVent> { new QTEEvent_Key(KeyCode.Q), new QTEEvent_Key(KeyCode.W), new QTEEvent_Key(KeyCode.E), new QTEEvent_Key(KeyCode.R),
                                                        new QTEEvent_Key(KeyCode.A), new QTEEvent_Key(KeyCode.S), new QTEEvent_Key(KeyCode.D), new QTEEvent_Key(KeyCode.F) };
        }


        public override void ShowQTE(Vector2 position, float scale, int difficulty)
        {
            base.ShowQTE(position, scale, difficulty);

            ringImage.color = ColorByDifficulty[difficulty];
            done = QTEHitResult.Playing;
            resultText.text = "";

            currentKeyEvents = new List<QTEEVent> { KeysByDifficulty[difficulty][Random.Range(0, KeysByDifficulty[difficulty].Count)] };
            currentKeyIndex = 0;

            UpdateKeyPromptText();

            StartCoroutine(RunQTE(difficulty));
        }


        public IEnumerator RunQTE(int difficulty)
        {
            float speed = RingSpeedByDifficulty[difficulty];

            currentRingScale = 2.0f;
            float ringAlpha = 1;

            while (done == QTEHitResult.Playing)
            {
                currentRingScale -= FixedSpeed(speed);
                if (currentRingScale < 0.65f)
                {
                    ringAlpha = currentRingScale;
                    if (currentRingScale <= 0.4f)
                    {
                        currentRingScale = 0;
                        done = QTEHitResult.Fail;
                        break;
                    }
                }

                var currentColor = ringImage.color;
                currentColor.a = ringAlpha;
                ringImage.color = currentColor;

                ringImage.transform.localScale = new Vector3(currentRingScale, currentRingScale, currentRingScale);

                yield return new WaitForEndOfFrame();
            }

            if (done == QTEHitResult.Perfect)
            {
                resultText.text = "완벽!";
                OnPerfect.Invoke();
            }
            else if (done == QTEHitResult.Win)
            {
                resultText.text = "성공!";
                OnSuccess.Invoke();
            }
            else
            {
                resultText.text = "실패..";
                OnFail.Invoke();
            }
        }


        private void Update()
        {
            if (done == QTEHitResult.Playing)
            {
                if (currentKeyIndex < currentKeyEvents.Count)
                {
                    var currentKeyEvent = currentKeyEvents[currentKeyIndex] as QTEEvent_Key;
                    if (currentKeyEvent != null && Input.GetKeyDown(currentKeyEvent.Key))
                    {
                        currentKeyIndex++;

                        UpdateKeyPromptText();

                        if (currentKeyIndex == currentKeyEvents.Count)
                        {
                            if (currentRingScale >= 0.9f && currentRingScale <= 1.1f)
                            {
                                done = QTEHitResult.Perfect;
                            }
                            else if (currentRingScale >= 0.75f && currentRingScale <= 1.3f)
                            {
                                done = QTEHitResult.Win;
                            }
                            else
                            {
                                done = QTEHitResult.Fail;
                            }
                        }
                    }
                }
            }
        }

        private void UpdateKeyPromptText()
        {
            if (currentKeyIndex < currentKeyEvents.Count)
            {
                var remainingKeys = new List<string>();
                for (int i = currentKeyIndex; i < currentKeyEvents.Count; i++)
                {
                    var keyEvent = currentKeyEvents[i] as QTEEvent_Key;
                    if (keyEvent != null)
                    {
                        remainingKeys.Add(keyEvent.Key.ToString());
                    }
                }
                keyPromptText.text = string.Join(", ", remainingKeys);
            }
            else
            {
                keyPromptText.text = "";
            }
        }

        private float FixedSpeed(float speed)
        {
            return speed * Time.fixedDeltaTime;
        }

        public override void Hide()
        {
            base.Hide();
            done = QTEHitResult.Playing;
            keyPromptText.text = "";
        }
    }
}
