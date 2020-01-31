/// <summary>
/// シングルトン基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> where T : class, new()
{
    /// <summary>
    /// インスタンス
    /// </summary>
    protected static T _instance = null;

    /// <summary>
    /// インスタンスを取得
    /// </summary>
    public static T Instance => _instance = _instance ?? new T();

    /// <summary>
    /// インスタンスを破棄する
    /// </summary>
    public static void Destroy()
    {
        _instance = null;
    }
}
