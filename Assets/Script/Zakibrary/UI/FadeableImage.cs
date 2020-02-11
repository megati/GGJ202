using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

/// <summary>
/// Imageをフェード可能
/// </summary>
public class FadeableImage : MonoBehaviour
{
    /// <summary>
    /// 自身が持つImageComponent
    /// </summary>
    Image _image = null;

    #region Sample

    //SetSprite("{SpritePath}").SetColor(Color.black, 1.0f).FilledFadeOutHorizontal(0.3f, Image.OriginHorizontal.Left,
    //// フェード終了時
    //() =>
    //        {
    //    SceneController.Instance.LoadSceneAsync(SceneName.SampleScene,
    //        // シーン読込開始時
    //        () =>
    //        {

    //        },
    //        // シーン読込終了時
    //        () =>
    //        {
    //            AlphaFadeIn(0.3f,
    //                () =>
    //                {
    //                    Destroy(transform.parent.gameObject);
    //                });
    //        });
    //});

    #endregion

    #region Set Param

    /// <summary>
    /// Resources/以下のSpriteAtlasのパスと、その使用するSprite名を設定する
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public FadeableImage SetSprite(string path, string name)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return this;
        }

        var spriteAtlas = Resources.Load<SpriteAtlas>(path);
        _image.sprite = spriteAtlas.GetSprite(name);

        return this;
    }

    /// <summary>
    /// Resources/以下のSpriteのパスからSpriteを読み込み設定する
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public FadeableImage SetSprite(string path)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return this;
        }

        var sprite = Resources.Load<Sprite>(path);
        _image.sprite = sprite;

        return this;
    }

    /// <summary>
    /// 色を設定する
    /// </summary>
    /// <param name="color">色</param>
    /// <param name="alpha">透明度（未入力なら透明度は変わらない）</param>
    /// <returns></returns>
    public FadeableImage SetColor(Color color, float? alpha = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return this;
        }

        color.a = (alpha == null) ? _image.color.a : (float)alpha;
        _image.color = color;

        return this;
    }

    /// <summary>
    /// alpha値を設定する
    /// </summary>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public FadeableImage SetAlpha(float alpha)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return this;
        }

        var color = _image.color;
        color.a = alpha;
        _image.color = color;

        return this;
    }

    #endregion

    #region Alpha Fade

    /// <summary>
    /// アルファフェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    public void AlphaFadeIn(float duration, System.Action endAction = null)
    {
        _image.raycastTarget = true;
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        if (_image.color.a == 0.0f)
        {
            Debug.LogWarning("Already Alpha is min.");
            return;
        }

        StartCoroutine(AlphaFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// アルファフェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    public void AlphaFadeOut(float duration, System.Action endAction = null)
    {
        _image.raycastTarget = true;
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        if (_image.color.a == 1.0f)
        {
            Debug.LogWarning("Already Alpha is max.");
            return;
        }

        StartCoroutine(AlphaFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// アルファフェードインを行うコルーチン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    IEnumerator AlphaFadeInCoroutine(float duration, System.Action endAction)
    {
        var color = _image.color;

        if (duration <= 0.0f)
        {
            color.a = 0.0f;
            _image.color = color;
            yield break;
        }

        var speed = color.a / duration * Time.deltaTime;
        //var speed = (duration == 0.0f) ? 1.0f : color.a / duration * Time.deltaTime;

        yield return new WaitWhile(() =>
        {
            color.a = Mathf.Clamp01(color.a - speed);
            _image.color = color;

            return (_image.color.a > 0.0f);
        });

        endAction?.Invoke();
        _image.raycastTarget = false;
    }

    /// <summary>
    /// アルファフェードアウトを行うコルーチン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    IEnumerator AlphaFadeOutCoroutine(float duration, System.Action endAction)
    {
        var color = _image.color;

        if (duration <= 0.0f)
        {
            color.a = 1.0f;
            _image.color = color;
            yield break;
        }

        var speed = (1.0f - color.a) / duration * Time.deltaTime;
        //var speed = (duration == 0.0f) ? 1.0f : (1.0f - color.a) / duration * Time.deltaTime;

        yield return new WaitWhile(() =>
        {
            color.a = Mathf.Clamp01(color.a + speed);
            _image.color = color;

            return (_image.color.a < 1.0f);
        });

        endAction?.Invoke();
    }

    #endregion

    #region Filled Fade

    /// <summary>
    /// 横フェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeInHorizontal(float duration, Image.OriginHorizontal origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Horizontal;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 0.0f)
        {
            Debug.LogWarning("Already FillAmount is min.");
            return;
        }

        StartCoroutine(FilledFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// 横フェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeOutHorizontal(float duration, Image.OriginHorizontal origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Horizontal;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 1.0f)
        {
            Debug.LogWarning("Already FillAmount is max.");
            return;
        }

        StartCoroutine(FilledFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// 縦フェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeInVertical(float duration, Image.OriginVertical origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Vertical;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 0.0f)
        {
            Debug.LogWarning("Already FillAmount is min.");
            return;
        }

        StartCoroutine(FilledFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// 縦フェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeOutVertical(float duration, Image.OriginVertical origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Vertical;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 1.0f)
        {
            Debug.LogWarning("Already FillAmount is max.");
            return;
        }

        StartCoroutine(FilledFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// クオーターフェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeInQuarter(float duration, Image.Origin90 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial90;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 0.0f)
        {
            Debug.LogWarning("Already FillAmount is min.");
            return;
        }

        StartCoroutine(FilledFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// クオーターフェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeOutQuarter(float duration, Image.Origin90 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial90;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 1.0f)
        {
            Debug.LogWarning("Already FillAmount is max.");
            return;
        }

        StartCoroutine(FilledFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// ハーフフェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeInHalf(float duration, Image.Origin180 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial180;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 0.0f)
        {
            Debug.LogWarning("Already FillAmount is min.");
            return;
        }

        StartCoroutine(FilledFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// ハーフフェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeOutHalf(float duration, Image.Origin180 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial180;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 1.0f)
        {
            Debug.LogWarning("Already FillAmount is max.");
            return;
        }

        StartCoroutine(FilledFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// フルフェードイン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeInFull(float duration, Image.Origin360 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial360;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 0.0f)
        {
            Debug.LogWarning("Already FillAmount is min.");
            return;
        }

        StartCoroutine(FilledFadeInCoroutine(duration, endAction));
    }

    /// <summary>
    /// フルフェードアウト
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="origin"></param>
    /// <param name="endAction"></param>
    public void FilledFadeOutFull(float duration, Image.Origin360 origin, System.Action endAction = null)
    {
        if (!(_image = _image ?? GetComponent<Image>()))
        {
            Debug.LogError("Null Image Component.");
            return;
        }

        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial360;
        _image.fillOrigin = (int)origin;

        if (_image.fillAmount == 1.0f)
        {
            Debug.LogWarning("Already FillAmount is max.");
            return;
        }

        StartCoroutine(FilledFadeOutCoroutine(duration, endAction));
    }

    /// <summary>
    /// フィルフェードインを行うコルーチン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    IEnumerator FilledFadeInCoroutine(float duration, System.Action endAction)
    {
        if (duration <= 0.0f)
        {
            _image.fillAmount = 0.0f;
            yield break;
        }

        var speed = _image.fillAmount / duration * Time.deltaTime;
        //var speed = (duration == 0.0f) ? 1.0f : _image.fillAmount / duration * Time.deltaTime;

        yield return new WaitWhile(() =>
        {
            _image.fillAmount = Mathf.Clamp01(_image.fillAmount - speed);

            return (_image.fillAmount > 0.0f);
        });

        endAction?.Invoke();
    }

    /// <summary>
    /// フィルフェードアウトを行うコルーチン
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    IEnumerator FilledFadeOutCoroutine(float duration, System.Action endAction)
    {
        if (duration <= 0.0f)
        {
            _image.fillAmount = 0.0f;
            yield break;
        }

        var speed = (1.0f - _image.fillAmount) / duration * Time.deltaTime;
        //var speed = (duration == 0.0f) ? 1.0f : (1.0f - _image.fillAmount) / duration * Time.deltaTime;

        yield return new WaitWhile(() =>
        {
            _image.fillAmount = Mathf.Clamp01(_image.fillAmount + speed);

            return (_image.fillAmount < 1.0f);
        });

        endAction?.Invoke();
    }

    #endregion
}
