using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Animatorの拡張クラス
/// </summary>
public static class AnimatorExtension
{
    /// <summary>
    /// シーケンサーを作成する
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    public static AnimationEventBinder CreateSequencer(this Animator self) => new AnimationEventBinder(self);

    /// <summary>
    /// レイヤー単位でアニメーション中か
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool IsPlaying(this Animator self, int layer = 0) => self.GetCurrentAnimatorStateInfo(layer).normalizedTime <= 1.0f;

    /// <summary>
    /// レイヤー単位で再生中のハッシュを返す
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static int GetPlayingStateHash(this Animator self, int layer = 0) => self.GetCurrentAnimatorStateInfo(layer).fullPathHash;

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
    public static int GetStateToInt32(this Animator self, System.Enum state) => System.Convert.ToInt32(state);

    /// <summary>
    /// イベントをバインドしアニメーションを再生する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="state"></param>
    /// <param name="layer"></param>
    /// <param name="normalizedTime"></param>
    /// <param name="onFinish">最後まで再生されたら呼ばれる</param>
    /// <param name="onTransition">遷移成功時に呼ばれる</param>
    /// <param name="onPlaying">再生中呼ばれる</param>
    /// <param name="onOtherPlay">他のアニメーションが再生されたら呼ばれる</param>
    /// <param name="onEnd">終了時に必ず呼ばれる</param>
    public static void Play(this Animator self, System.Enum state, int layer = 0, float normalizedTime = 0.0f,
        System.Action onFinish = null, System.Action<int> onTransition = null,
        System.Action<float> onPlaying = null, System.Action onOtherPlay = null, System.Action onEnd = null)
    {
        Play(self, GetStateToInt32(self, state), layer, normalizedTime, onFinish, onTransition, onPlaying, onOtherPlay, onEnd);
    }

    /// <summary>
    /// イベントをバインドしアニメーションを再生する
    /// </summary>
    /// <param name="self"></param>
    /// <param name="stateFullHashPath"></param>
    /// <param name="layer"></param>
    /// <param name="normalizedTime"></param>
    /// <param name="onFinish">最後まで再生されたら呼ばれる</param>
    /// <param name="onTransition">遷移成功時に呼ばれる</param>
    /// <param name="onPlaying">再生中呼ばれる</param>
    /// <param name="onOtherPlay">他のアニメーションが再生されたら呼ばれる</param>
    /// <param name="onEnd">終了時に必ず呼ばれる</param>
    public static void Play(this Animator self, int stateFullHashPath, int layer = 0, float normalizedTime = 0.0f,
    System.Action onFinish = null, System.Action<int> onTransition = null,
    System.Action<float> onPlaying = null, System.Action onOtherPlay = null, System.Action onEnd = null)
    {
        self.Play(stateFullHashPath, layer, normalizedTime);

        if (onFinish == null || onTransition == null || onPlaying == null || onOtherPlay == null || onEnd == null)
        {
            GlobalCoroutine.Instance.StartCoroutine(PlayCoroutine(self, stateFullHashPath, layer, onFinish, onTransition, onPlaying, onOtherPlay, onEnd));
        }
    }

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
    /// アニメーションを再生しバインドしたイベントを実行するコルーチン
    /// </summary>
    /// <param name="self"></param>
    /// <param name="stateFullHashPath"></param>
    /// <param name="layer"></param>
    /// <param name="onFinish">最後まで再生されたら呼ばれる</param>
    /// <param name="onTransition">遷移成功時に呼ばれる</param>
    /// <param name="onPlaying">再生中呼ばれる</param>
    /// <param name="onOtherPlay">他のアニメーションが再生されたら呼ばれる</param>
    /// <param name="onEnd">終了時に必ず呼ばれる</param>
    /// <returns></returns>
    public static IEnumerator PlayCoroutine(this Animator self, int stateFullHashPath, int layer,
        System.Action onFinish = null,
        System.Action<int> onTransition = null,
        System.Action<float> onPlaying = null,
        System.Action onOtherPlay = null,
        System.Action onEnd = null)
    {
        yield return null;

        while (true)
        {
            onPlaying?.Invoke(self.GetCurrentAnimatorStateInfo(layer).normalizedTime);

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

                onTransition?.Invoke(self.GetCurrentAnimatorStateInfo(layer).fullPathHash);
                break;
            }

            // アニメーションが最後まで再生されたら
            if (!IsPlaying(self, layer))
            {
                onFinish?.Invoke();
                break;
            }

            // 遷移せずにアニメーションが切り替わったら
            if (self.GetCurrentAnimatorStateInfo(layer).fullPathHash != stateFullHashPath)
            {
                onOtherPlay?.Invoke();
                break;
            }

            yield return null;
        }

        // 共通の終了処理
        onEnd?.Invoke();
    }

    ///// <summary>
    ///// 指定したステートのレイヤー（重いため非推奨）
    ///// </summary>
    ///// <param name="state"></param>
    ///// <returns></returns>
    //public static int StateLayer(this Animator self, System.Enum state, int hash)
    //{
    //    var name = System.Enum.GetName(state.GetType(), hash);
    //    return int.Parse(name.Substring(name.Length - 1));
    //}
}
