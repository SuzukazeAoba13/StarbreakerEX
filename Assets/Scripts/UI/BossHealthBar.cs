using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealthBar : StatsBarHUD
{
    protected override void SetPrecentText()
    {
        //_percentText.text = string.Format("{0:N2}", _targetFillAmount * 100f) + "%";
        // _percentText.text = (_targetFillAmount * 100f).ToString("f2") + "%";
        _percentText.text = _targetFillAmount.ToString("P2");
    }
}
