using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIcon : MonoBehaviour
{
    [SerializeField]
    Sprite SearchStatusIcon = null;
    [SerializeField]
    Sprite WaringStatusIcon = null;
    [SerializeField]
    Sprite DangerStatusIcon = null;

    Image image = null;

    Dictionary<Enemy, bool> isChasedEnemies = new Dictionary<Enemy, bool>();

    void Awake()
    {
        image = GetComponent<Image>();
    }

    /// <summary>
    /// 探索状態ステータスアイコンを表示する
    /// </summary>
    void ShowSearchStatusIcon() => image.sprite = SearchStatusIcon;

    /// <summary>
    /// 警告状態ステータスアイコンを表示する
    /// </summary>
    void ShowWaringStatusIcon() => image.sprite = WaringStatusIcon;

    /// <summary>
    /// 危険状態ステータスアイコンを表示する
    /// </summary>
    void ShowDangerStatusIcon() => image.sprite = DangerStatusIcon;

    /// <summary>
    /// 見つかった
    /// </summary>
    public void Waring(Enemy enemy)
    {
        if (!isChasedEnemies.ContainsKey(enemy))
        {
            isChasedEnemies.Add(enemy, false);
        }

        foreach (var isChasedEnemy in isChasedEnemies.Values)
        {
            if(isChasedEnemy)
            {
                ShowDangerStatusIcon();
            }
            else
            {
                ShowWaringStatusIcon();
            }
        }
    }

    /// <summary>
    /// 追いかけられた
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void Danger(Enemy enemy)
    {
        if(!isChasedEnemies.ContainsKey(enemy))
        {
            isChasedEnemies.Add(enemy, true);
        }

        ShowDangerStatusIcon();

        BgmSpeaker.Instance.PlayChaseBgm();
    }

    /// <summary>
    /// 逃れた
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void Escaped(Enemy enemy)
    {
        if (!isChasedEnemies.ContainsKey(enemy))
        {
            isChasedEnemies.Add(enemy, false);
        }
        else
        {
            isChasedEnemies[enemy] = false;
        }

        foreach (var isChasedEnemy in isChasedEnemies.Values)
        {
            if (isChasedEnemy)
            {
                ShowDangerStatusIcon();
            }
            else
            {
                ShowSearchStatusIcon();

                BgmSpeaker.Instance.PlayDefaultBgm();
            }
        }
    }
}
