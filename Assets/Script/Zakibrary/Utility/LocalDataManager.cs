using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ローカルデータを管理する
/// </summary>
public static class LocalDataManager
{
    /// <summary>
    /// ローカルデータを保存するファイルのパス
    /// </summary>
    static string _localDataPath => Path.Combine(Application.dataPath, "LocalData");

    /// <summary>
    /// データを 暗号化・JSON化 し保存する
    /// </summary>
    /// <typeparam name="T">Serializableなクラス</typeparam>
    /// <param name="data"></param>
    public static void SaveLocalDataToJson<T>(T data) where T : class, new()
    {
        if (!Directory.Exists(_localDataPath))
        {
            Directory.CreateDirectory(_localDataPath);
        }

        using (var streamWriter = new StreamWriter(Path.Combine(_localDataPath, typeof(T).Name)))
        {
            var text = JsonUtility.ToJson(data);
            text = AvoEx.AesEncryptor.Encrypt(text);
            streamWriter.Write(text);
        }
    }

    /// <summary>
    /// 複合化されたJSONデータを取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T LoadLocalData<T>() where T : class, new()
    {
        if (!File.Exists(Path.Combine(_localDataPath, typeof(T).Name)))
        {
            SaveLocalDataToJson(new T());
        }

        using (var streamReader = new StreamReader(Path.Combine(_localDataPath, typeof(T).Name)))
        {
            var json = streamReader.ReadToEnd();
            json = AvoEx.AesEncryptor.DecryptString(json);
            return JsonUtility.FromJson<T>(json);
        }
    }
}
