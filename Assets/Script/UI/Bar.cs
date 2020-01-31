using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// バーの表示管理
/// </summary>
public class Bar : MonoBehaviour
{
    [SerializeField]
    private Image barImage;
    [SerializeField]
    private TextMeshProUGUI percentText;
    [SerializeField]
    private Color barColor;

    // Start is called before the first frame update
    void Start()
    {
        percentText.text = "0%";
        barImage.fillAmount = 0.0f;
        barImage.color = barColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// パーセントを登録
    /// </summary>
    /// <param name="percentAmount"></param>
    public void SetParameter(float percentAmount)
    {
        percentText.text = percentAmount.ToString() + "%";
        barImage.fillAmount = percentAmount /100;
    }
}
