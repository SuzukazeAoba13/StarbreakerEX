using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBarHUD : StatsBar
{
    [SerializeField] protected Text _percentText;

    protected virtual void SetPrecentText()
    {
        // _percentText.text = Mathf.RoundToInt(_targetFillAmount * 100) + "%";
        _percentText.text = _targetFillAmount.ToString("P0");
    }

    public override void Initialize(float currentValue, float maxValue)
    {
        base.Initialize(currentValue, maxValue);
        SetPrecentText();
    }

    protected override IEnumerator BufferedFillingCoroutine(Image image)
    {
        SetPrecentText();
        return base.BufferedFillingCoroutine(image);
    }
}
