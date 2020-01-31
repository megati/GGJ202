using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションステートを監視する
/// </summary>
public class AnimationStateMachineObserver : StateMachineBehaviour
{
    /// <summary>
    /// 再生時に呼ばれるイベント
    /// </summary>
    System.Action<int, int> _onStart = null;

    /// <summary>
    /// 再生終了時に呼ばれるイベント
    /// </summary>
    System.Action<int, int> _onEnd = null;

    /// <summary>
    /// 再生中に呼ばれるイベント
    /// </summary>
    System.Action<int, int> _onPlaying = null;

    /// <summary>
    /// 再生時に呼ばれるイベントを登録する
    /// </summary>
    /// <param name="callback"></param>
    public void BindStartEvent(System.Action<int, int> callback)
    {
        _onStart += callback;
    }

    /// <summary>
    /// 再生時に呼ばれるイベントを解除する
    /// </summary>
    /// <param name="callback"></param>
    public void UnbindStartEvent(System.Action<int, int> callback)
    {
        _onStart -= callback;
    }

    /// <summary>
    /// 再生時に呼ばれるイベントを全て解除する
    /// </summary>
    public void UnbindAllStartEvent()
    {
        _onStart = null;
    }

    /// <summary>
    /// 再生終了時に呼ばれるイベントを登録する
    /// </summary>
    /// <param name="callback"></param>
    public void BindEndEvent(System.Action<int, int> callback)
    {
        _onEnd += callback;
    }

    /// <summary>
    /// 再生終了時に呼ばれるイベントを解除する
    /// </summary>
    /// <param name="callback"></param>
    public void UnbindEndEvent(System.Action<int, int> callback)
    {
        _onEnd -= callback;
    }

    /// <summary>
    /// 再生後に呼ばれるイベントを全て解除する
    /// </summary>
    public void UnbindAllEndEvent()
    {
        _onEnd = null;
    }

    /// <summary>
    /// 再生中に呼ばれるイベントを登録する
    /// </summary>
    /// <param name="callback"></param>
    public void BindPlayingEvent(System.Action<int, int> callback)
    {
        _onPlaying += callback;
    }

    /// <summary>
    /// 再生中に呼ばれるイベントを解除する
    /// </summary>
    /// <param name="callback"></param>
    public void UnbindPlayingEvent(System.Action<int, int> callback)
    {
        _onPlaying -= callback;
    }

    /// <summary>
    /// 再生中に呼ばれるイベントを全て解除する
    /// </summary>
    public void UnbindAllPlayingEvent()
    {
        _onPlaying = null;
    }

    /// <summary>
    /// [UnityMethod] アニメーション再生時に呼ばれる
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(stateInfo.fullPathHash);
        _onStart?.Invoke(stateInfo.fullPathHash, layerIndex);
    }

    /// <summary>
    /// [UnityMethod] アニメーション中に呼ばれる
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _onPlaying?.Invoke(stateInfo.fullPathHash, layerIndex);
    }

    /// <summary>
    /// [UnityMethod] アニメーション終了時に呼ばれる
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _onEnd?.Invoke(stateInfo.fullPathHash, layerIndex);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
