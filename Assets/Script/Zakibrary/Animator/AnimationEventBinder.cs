using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションのイベントを登録して再生できる
/// </summary>
public class AnimationEventBinder
{
    Animator _animator = null;

    Action _onCompleted = null;
    Action _onFailed = null;
    Action _onFinished = null;
    Action<float> _onPlaying = null;

    Enum _state = default;
    int _layer = 0;

    /// <summary>
    /// コンストラクト（Animatorを取得）
    /// </summary>
    /// <param name="animator"></param>
    public AnimationEventBinder(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// アニメーション情報を設定
    /// </summary>
    /// <param name="state"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public AnimationEventBinder SetStateInfo(Enum state, int layer = 0)
    {
        _state = state;
        _layer = layer;

        return this;
    }

    /// <summary>
    /// 指定したアニメーションステートを再生
    /// </summary>
    /// <param name="state"></param>
    /// <param name="layer"></param>
    public void Play()
    {
        _animator.Play(_state, _layer, _onPlaying, _onCompleted, _onFailed, _onFinished);
    }

    /// <summary>
    /// 成功時の処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindCompletedEvent(Action onEvent)
    {
        _onCompleted = onEvent;
        return this;
    }

    /// <summary>
    /// 失敗時の処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindFailedEvent(Action onEvent)
    {
        _onFailed = onEvent;
        return this;
    }

    /// <summary>
    /// 終了時の共通処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindFinishedEvent(Action onEvent)
    {
        _onFinished = onEvent;
        return this;
    }

    /// <summary>
    /// アニメーション中のコールバックを登録
    /// </summary>
    /// <param name="onEvent">normalizeTimeが引数として取得できる</param>
    /// <returns></returns>
    public AnimationEventBinder BindPlayingEvent(Action<float> onEvent)
    {
        _onPlaying = onEvent;
        return this;
    }
}
