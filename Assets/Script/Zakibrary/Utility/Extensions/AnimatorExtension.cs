using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animatorの拡張クラス
/// </summary>
public static class AnimatorExtension
{
    /// <summary>
    /// <para>指定した列挙型がアニメーションステートのハッシュを持つのであれば再生する</para>
    /// ※現在のステートを指定した場合は無効
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state">アニメーションステートのハッシュの数値を持つ列挙型</param>
    /// <param name="layer">レイヤー番号</param>
    /// <param name="playingCallback">再生中のコールバック（normalizeTimeを引数に持ってこれる）</param>
    /// <param name="successAction">予定されていた再生終了時に呼ばれる処理</param>
    /// <param name="failureAction">予定されていなかった再生終了時に呼ばれる処理</param>
    /// <param name="endAction">再生終了時に共通に呼ばれる処理</param>
    public static void Play(this Animator self, System.Enum state, int layer = 0, System.Action<float> playingCallback = null,
        System.Action successAction = null, System.Action failureAction = null, System.Action endAction = null)
    {
        self.Play(StateToInt32(state), layer);

        if (playingCallback != null || successAction != null || failureAction != null || endAction != null)
        {
            GlobalCoroutine.Instance.StartCoroutine(ObserveLayerCoroutine(self, layer, playingCallback, successAction, failureAction, endAction));
        }
    }

    /// <summary>
    /// 指定した列挙型がアニメーションステートのハッシュを持つのであれば再生する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state">アニメーションステートのハッシュの数値を持つ列挙型</param>
    /// <param name="layer">レイヤー番号</param>
    /// <param name="normalizedTime">0～１を指定可能</param>
    /// <param name="playingCallback">再生中のコールバック（normalizeTimeを引数に持ってこれる）</param>
    /// <param name="successAction">予定されていた再生終了時に呼ばれる処理</param>
    /// <param name="failureAction">予定されていなかった再生終了時に呼ばれる処理</param>
    /// <param name="endAction">再生終了時に共通に呼ばれる処理</param>
    public static void ForcePlay(this Animator self, System.Enum state, int layer = 0, float normalizedTime = 0.0f,
        System.Action<float> playingCallback = null, System.Action successAction = null, System.Action failureAction = null, System.Action endAction = null)
    {
        self.Play(StateToInt32(state), layer, normalizedTime);

        if (successAction != null || failureAction != null || endAction != null)
        {
            GlobalCoroutine.Instance.StartCoroutine(ObserveLayerCoroutine(self, layer, playingCallback, successAction, failureAction, endAction));
        }
    }

    /// <summary>
    /// シーケンサーを作成する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static AnimationEventBinder CreateSequencer(this Animator self)
    {
        return new AnimationEventBinder(self);
    }

    ///// <summary>
    ///// アニメーションに単純なコールバックを持たせて再生させれるクラスを生成する
    ///// </summary>
    ///// <param name="self"></param>
    ///// <returns></returns>
    //public static AnimationStateSetter CreateStateSetter(this Animator self)
    //{
    //    return new AnimationStateSetter(self);
    //}

    /// <summary>
    /// 指定したステートが再生中かどうか
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state">ステートの列挙型</param>
    /// <param name="layer">レイヤー番号(ステート名のLの後の数字)</param>
    /// <param name="enableTransition">遷移中も再生中とみなすか</param>
    /// <returns></returns>
    public static bool IsPlayingState(this Animator self, System.Enum state, int layer = 0, bool enableTransition = true)
    {
        var isPlaying = self.GetCurrentAnimatorStateInfo(layer).fullPathHash == System.Convert.ToInt32(state);
        var isTransition = (enableTransition) ? self.IsInTransition(layer) : false;
        return (isPlaying || !isPlaying && isTransition);
    }

    /// <summary>
    /// 指定したステートが再生中かどうか
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state">ステートのハッシュ</param>
    /// <param name="layer">レイヤー番号(ステート名のLの後の数字)</param>
    /// <param name="enableTransition">遷移中も再生中とみなすか</param>
    /// <returns></returns>
    public static bool IsPlayingState(this Animator self, int state, int layer = 0, bool enableTransition = true)
    {
        var isPlaying = self.GetCurrentAnimatorStateInfo(layer).fullPathHash == state;
        var isTransition = (enableTransition) ? self.IsInTransition(layer) : false;
        return (isPlaying || !isPlaying && isTransition);
    }

    /// <summary>
    /// レイヤー単位でアニメーション中か
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool IsPlaying(this Animator self, int layer = 0)
    {
        return self.GetCurrentAnimatorStateInfo(layer).normalizedTime <= 1.0f;
    }

    /// <summary>
    /// レイヤー単位で再生中のハッシュを返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static int PlayingStateHash(this Animator self, int layer = 0)
    {
        return self.GetCurrentAnimatorStateInfo(layer).fullPathHash;
    }

    /// <summary>
    /// 指定したレイヤーで再生中のアニメーションを監視するコルーチン
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer">レイヤー番号</param>
    /// <param name="playingCallback">再生中のコールバック（normalizedTimeを引数として持ってこれる）</param>    
    /// <param name="successAction">アニメーションが予測通りに遷移or終了した際に呼ばれる処理</param>
    /// <param name="failureAction">アニメーションが予測とは違う遷移をした際に呼ばれる処理</param>
    /// <param name="endAction">アニメーションが終了した際に共通で呼ばれる処理</param>
    /// <returns></returns>
    public static IEnumerator ObserveLayerCoroutine(this Animator self, int layer, System.Action<float> playingCallback = null,
        System.Action successAction = null, System.Action failureAction = null, System.Action endAction = null, System.Action onTransition = null)
    {
        yield return null;

        // 再生中のアニメーションステートを取得
        var state = self.GetCurrentAnimatorStateInfo(layer).fullPathHash;
        while (true)
        {
            playingCallback?.Invoke(self.GetCurrentAnimatorStateInfo(layer).normalizedTime);

            // 次のアニメーションに遷移開始したら
            if (self.IsInTransition(layer))
            {
                yield return new WaitWhile(() =>
                {
                    //// 遷移せずにアニメーションが切り替わったら
                    //if (self.GetCurrentAnimatorStateInfo(layer).fullPathHash != state)
                    //{
                    //    failureAction?.Invoke();
                    //    return false;
                    //}

                    return (self.IsInTransition(layer));
                });

                onTransition?.Invoke();
                break;
            }

            // アニメーションが最後まで再生されたら
            if (!IsPlaying(self, layer))
            {
                successAction?.Invoke();
                break;
            }

            // 遷移せずにアニメーションが切り替わったら
            if (self.GetCurrentAnimatorStateInfo(layer).fullPathHash != state)
            {
                failureAction?.Invoke();
                break;
            }

            yield return null;
        }

        // 共通の終了処理
        endAction?.Invoke();
    }

    /// <summary>
    /// 再生中のNormalizedTimeを返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static float GetNormalizedTime(this Animator self, int layer) => self.GetCurrentAnimatorStateInfo(layer).normalizedTime;

    /// <summary>
    /// ステート列挙型をint型に変換する
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    static int StateToInt32(System.Enum state) => System.Convert.ToInt32(state);

    /// <summary>
    /// 指定したステートのレイヤー（重いため非推奨）
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    static int StateLayer(System.Enum state, int hash)
    {
        var name = System.Enum.GetName(state.GetType(), hash);
        return int.Parse(name.Substring(name.Length - 1));
    }
}
