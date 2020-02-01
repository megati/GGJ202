using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム起動とともに DontDestroyOnLoad となるCanvas
/// </summary>
public class DontDestroyOnLoadCanvas : SingletonMonoBehaviour<DontDestroyOnLoadCanvas>
{
    [SerializeField]
    FadeableImage fadeableImage = null;

    public FadeableImage GetFadeableImage => fadeableImage;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EntryPoint()
    {
        DontDestroyOnLoad(Instantiate(Resources.Load("Prefabs/UIs/DontDestroyOnLoadCanvas")));
    }

    void Awake()
    {
        fadeableImage.SetColor(Color.black, 1.0f).AlphaFadeIn(1.0f);
    }
}
