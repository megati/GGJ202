using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataSave 
{
    /// <summary>
    /// ローカルデータを保存するファイルのパス
    /// </summary>
    string _localDataPath => Path.Combine(Application.dataPath, "LocalData");

    /// <summary>
    /// データを 暗号化・JSON化 し保存する
    /// </summary>
    /// <typeparam name="T">Serializableなクラス</typeparam>
    /// <param name="data"></param>
    public void SaveLocalDataToJson(BestTime data)
    {
        if (!Directory.Exists(_localDataPath))
        {
            Directory.CreateDirectory(_localDataPath);
        }

        using (var streamWriter = new StreamWriter(Path.Combine(_localDataPath, typeof(BestTime).Name)))
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
    public BestTime LoadLocalData()
    {
        if (!File.Exists(Path.Combine(_localDataPath, typeof(BestTime).Name)))
        {
            SaveLocalDataToJson(new BestTime());
        }

        using (var streamReader = new StreamReader(Path.Combine(_localDataPath, typeof(BestTime).Name)))
        {
            var json = streamReader.ReadToEnd();
            json = AvoEx.AesEncryptor.DecryptString(json);
            return JsonUtility.FromJson<BestTime>(json);
        }
    }
}
