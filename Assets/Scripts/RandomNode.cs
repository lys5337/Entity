using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TJ
{
    public class RandomNode : MonoBehaviour
    {
        public Fighter player;
        public Button optionButton1;
        public Button optionButton2;
        public Text optionText1;
        public Text optionText2;

        public Button resultButton; // ����� ǥ���� ��ư
        public Text resultText; // ��� �ؽ�Ʈ

        public int hpLossAmount = 10;
        public List<Card> possibleCardsToGain = new List<Card>(); // ���� �� �ִ� ī�� ���

        private GameManager gameManager;
        private List<string> optionDescriptions = new List<string>();

        private void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            InitializeOptions();
            SetRandomOptions();
            optionButton1.onClick.AddListener(() => ExecuteRandomEvent(optionText1.text));
            optionButton2.onClick.AddListener(() => ExecuteRandomEvent(optionText2.text));

            // ��� ��ư�� �ʱ⿡�� ��Ȱ��ȭ
            resultButton.gameObject.SetActive(false);
        }

        private void InitializeOptions()
        {
            optionDescriptions.Add("���� ���ڸ� ���� (50% Ȯ���� ī�带 ��ų� HP�� ����)");
            optionDescriptions.Add("������ �ι��� ������ �޾Ƶ��δ� (50% Ȯ���� ī�带 ��ų� ī�带 ����)");
            optionDescriptions.Add("�׳� ��������");
            optionDescriptions.Add("������ �غ��Ѵ� (50% Ȯ���� ī�带 ����)");
            optionDescriptions.Add("�ź��� ��Ҹ� �����Ѵ� (50% Ȯ���� ī�带 ��ų� HP�� ����)");
        }

        private void SetRandomOptions()
        {
            List<string> selectedOptions = new List<string>(optionDescriptions);
            CustomListExtensions.ShuffleList(selectedOptions);

            optionText1.text = selectedOptions[0];
            optionText2.text = selectedOptions[1];
        }

        private void ExecuteRandomEvent(string selectedOption)
        {
            int randomOutcome = Random.Range(0, 2);
            string resultMessage = "";

            if (selectedOption.Contains("���� ����"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} ī�带 ������ϴ�!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    resultMessage = $"{hpLossAmount} HP�� �Ҿ����ϴ�!";
                }
            }
            else if (selectedOption.Contains("������ �ι�"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} ī�带 ������ϴ�!";
                }
                else
                {
                    Card cardLost = RemoveRandomCardFromDeck();
                    resultMessage = $"{cardLost.cardTitle} ī�带 �Ҿ����ϴ�!";
                }
            }
            else if (selectedOption.Contains("�׳� ��������"))
            {
                resultMessage = "�ƹ� �ϵ� �Ͼ�� �ʾҽ��ϴ�.";
            }
            else if (selectedOption.Contains("������ �غ��Ѵ�"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} ī�带 ������ϴ�!";
                }
            }
            else if (selectedOption.Contains("�ź��� ��Ҹ� �����Ѵ�"))
            {
                if (randomOutcome == 0)
                {
                    Card cardGained = GetRandomCardFromList(possibleCardsToGain);
                    gameManager.playerDeck.Add(cardGained);
                    resultMessage = $"{cardGained.cardTitle} ī�带 ������ϴ�!";
                }
                else
                {
                    player.currentHealth -= hpLossAmount;
                    gameManager.DisplayHealth(player.currentHealth, player.maxHealth);
                    resultMessage = $"{hpLossAmount} HP�� �Ҿ����ϴ�!";
                }
            }

            // �ɼ� ��ư�� ����� ��� ��ư�� Ȱ��ȭ�Ͽ� ��� �޽����� ǥ��
            optionButton1.gameObject.SetActive(false);
            optionButton2.gameObject.SetActive(false);

            resultText.text = resultMessage;
            resultButton.gameObject.SetActive(true);

            Debug.Log(resultMessage);
        }

        private Card GetRandomCardFromList(List<Card> cardList)
        {
            int randomIndex = Random.Range(0, cardList.Count);
            return cardList[randomIndex];
        }

        private Card RemoveRandomCardFromDeck()
        {
            if (gameManager.playerDeck.Count == 0)
            {
                Debug.Log("�÷��̾� ���� ī�尡 �����ϴ�.");
                return null;
            }

            int randomIndex = Random.Range(0, gameManager.playerDeck.Count);
            Card cardToRemove = gameManager.playerDeck[randomIndex];
            gameManager.playerDeck.Remove(cardToRemove);
            return cardToRemove;
        }
    }

    public static class CustomListExtensions
    {
        public static void ShuffleList<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
