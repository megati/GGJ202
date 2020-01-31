using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// EventTriggerの拡張クラス
/// </summary>
public static class EvenTriggerExtension
{
    /// <summary>
    /// イベントトリガーを作成する
    /// </summary>
    /// <param name="eventTriggerType">イベント条件</param>
    /// <param name="onEvent">イベント処理</param>
    /// <returns></returns>
    public static EventTrigger.Entry CreateEntry(this EventTrigger self, EventTriggerType eventTriggerType, UnityEngine.Events.UnityAction onEvent)
    {
        var entry = new EventTrigger.Entry
        {
            eventID = eventTriggerType
        };

        entry.callback.AddListener(_=> onEvent());

        return entry;

        #region サンプル

        //var trigger = gameObject.AddComponent<EventTrigger>();

        //trigger.triggers = new List<EventTrigger.Entry>
        //    {
        //        GetEntry(EventTriggerType.PointerClick, 〇〇),
        //        GetEntry(EventTriggerType.PointerUp,△△)
        //    };

        #endregion
    }
}
