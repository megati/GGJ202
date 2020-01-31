using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// イベントを発火させる基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="I"></typeparam>
public class EventTriggerBase<T, I> : Singleton<T> where T : class, new() where I : IEventBase
{
    /// <summary>
    /// イベントを取得するリスナーのリスト
    /// </summary>
    protected List<I> _eventListeners = new List<I>();

    /// <summary>
    /// <para>リスナーを登録する</para>
    /// - Awakeで呼ぶ
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(I listener)
    {
        if (!_eventListeners.Contains(listener))
        {
            _eventListeners.Add(listener);
        }
    }

    /// <summary>
    /// <para>リスナーの登録を破棄する</para>
    /// - OnDestroyで呼ぶ
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveListener(I listener)
    {
        if (_eventListeners.Contains(listener))
        {
            _eventListeners.Remove(listener);
        }
    }

    #region フレーム

    ///// <summary>
    ///// 発火させる
    ///// </summary>
    //void CallEvent()
    //{
    //    for(int i=0;i<_eventListeners.Count;i++)
    //    {
    //        _eventListeners[i].Event();
    //    }
    //}

    #endregion
}
