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

    Action _onFinish = null;
    Action<int> _onTransition = null;
    Action<float> _onPlaying = null;
    Action _onOtherPlay = null;
    Action _onEnd = null;

    Enum _state = null;
    int _layer = 0;
    float _normalizeTime = 0.0f;

    /// <summary>
    /// コンストラクト（Animatorを取得）
    /// </summary>
    /// <param name="animator"></param>
    public AnimationEventBinder(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// アニメーションのパラメータを設定
    /// </summary>
    /// <param name="state"></param>
    /// <param name="layer"></param>
    /// <param name="normalizeTime"></param>
    /// <returns></returns>
    public AnimationEventBinder SetParam(Enum state, int layer = 0, float normalizeTime = 0.0f)
    {
        _state = state;
        _layer = layer;
        _normalizeTime = normalizeTime;

        return this;
    }

    /// <summary>
    /// 指定したアニメーションを再生
    /// </summary>
    public void Play()
    {
        if (_state == null)
        {
            Debug.LogError("AnimationEventBinder::Play FAIL. Call SetParam befor Play.");
            return;
        }

        _animator.Play(_state, _layer, _normalizeTime, _onFinish, _onTransition, _onPlaying, _onOtherPlay, _onEnd);

        _state = null;
        _onFinish = null;
        _onTransition = null;
        _onPlaying = null;
        _onOtherPlay = null;
        _onEnd = null;
    }

    /// <summary>
    /// 成功時の処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindFinish(Action onEvent)
    {
        _onFinish = onEvent;
        return this;
    }

    /// <summary>
    /// 遷移時の処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindTransition(Action<int> onEvent)
    {
        _onTransition = onEvent;
        return this;
    }

    /// <summary>
    /// 失敗時の処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindOtherPlay(Action onEvent)
    {
        _onOtherPlay = onEvent;
        return this;
    }

    /// <summary>
    /// 終了時の共通処理を登録
    /// </summary>
    /// <param name="onEvent"></param>
    /// <returns></returns>
    public AnimationEventBinder BindEnd(Action onEvent)
    {
        _onEnd = onEvent;
        return this;
    }

    /// <summary>
    /// アニメーション中のコールバックを登録
    /// </summary>
    /// <param name="onEvent">normalizeTimeが引数として取得できる</param>
    /// <returns></returns>
    public AnimationEventBinder BindPlaying(Action<float> onEvent)
    {
        _onPlaying = onEvent;
        return this;
    }
}
