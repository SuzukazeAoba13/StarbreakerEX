using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissileDisplay : MonoBehaviour
{
    private static Text _amountText;
    private static Image _cooldownImage;

    private void Awake()
    {
        _amountText = transform.Find("AmountText").GetComponent<Text>();
        _cooldownImage =  transform.Find("CooldownImage").GetComponent<Image>();
    }

    public static void UpdateAmountText(int score) => _amountText.text = score.ToString();
    
    public static void UpdateCooldownImage(float fillAmount) => _cooldownImage.fillAmount = fillAmount;
}
