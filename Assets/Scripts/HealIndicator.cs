using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TJ
{
    public class HealIndicator : MonoBehaviour
    {
        public Animator animator;
        public TMP_Text text;
        public void DisplayHeal(int amount)
        {
            text.text = amount.ToString();
            animator.Play("DisplayHeal");
        }
    }
}