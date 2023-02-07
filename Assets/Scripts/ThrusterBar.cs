using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ThrusterBar : MonoBehaviour
{
    public Slider slider;
    public Image sliderfill;
    public void Update()
    {
        sliderfill.color= Color.Lerp(Color.cyan, Color.red, (slider.value / slider.maxValue));
    }
    public void SetMaxValue(int val)
    {
        slider.maxValue = val;
    }

    public void SetDefaultValue(int val)
    {
        slider.value = val;
    }
}
