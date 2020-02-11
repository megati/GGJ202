using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum TITLE_SELECT
{
    START,
    MANUAL,
    END
}

/// <summary>
/// タイトルのシーン選択
/// </summary>
public class TitleTap : MonoBehaviour
{
    [SerializeField]
    TITLE_SELECT titleSelect;
    [SerializeField]
    AudioSource touchSe;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ボタンに触れた際の処理
    /// </summary>
    public void OnTouchButton()
    {
        touchSe.Play();
        image.color = new Color(1.0f,1.0f,1.0f,1.0f);
        Debug.Log("Hit");
    }

    /// <summary>
    /// ボタンを離したら
    /// </summary>
    public void OnExitButton()
    {
        image.color = new Color(1.0f, 1.0f, 1.0f, 0.1f);
    }

    /// <summary>
    /// 画像をクリックした
    /// </summary>
    public void OnClickButton()
    {
        Debug.Log("click");
        switch (titleSelect)
        {
            case TITLE_SELECT.START:
                SceneTransition.Instance.TransitionScene(SceneName.MasterGame);
                break;
            case TITLE_SELECT.MANUAL:
                break;
            case TITLE_SELECT.END:
                DontDestroyOnLoadCanvas.Instance.GetFadeableImage.AlphaFadeOut(1f, () =>
                {
                    UnityEngine.Application.Quit();
                });
                UnityEngine.Application.Quit();
                break;
        }
    }
}
