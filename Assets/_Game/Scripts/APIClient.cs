using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class APIClient : MonoBehaviour
{
    private string baseUrl = "https://trail-api-y0t9.onrender.com/";

    public IEnumerator StartGame(System.Action<TaskData> callback)
    {
        using (UnityWebRequest www = new UnityWebRequest(baseUrl + "start", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(new byte[0]);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log(json);
                var tdata = JsonConvert.DeserializeObject<TaskData>(json);
                callback(tdata);
            }
        }
    }

    public IEnumerator Submit(SubmitData data, System.Action<TaskData> callback)
    {
        string playerjson = JsonConvert.SerializeObject(data);
        Debug.Log("SUBMIT JSON: " + playerjson);

        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(playerjson);

        using (UnityWebRequest www = new UnityWebRequest(baseUrl + "submit", "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                Debug.Log("RESPONSE JSON: " + json);

                // Deserialize response into TaskData
                var tdata = JsonConvert.DeserializeObject<TaskData>(json);
                callback(tdata);
            }
        }
    }

    public IEnumerator ClearLast(System.Action<TaskData> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(baseUrl + "clear_last", ""))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                var tdata = JsonConvert.DeserializeObject<TaskData>(json);
                callback(tdata);
            }
        }
    }

    public IEnumerator ClearAll(System.Action<TaskData> callback)
    {
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(baseUrl + "clear_all", ""))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                var tdata = JsonConvert.DeserializeObject<TaskData>(json);
                callback(tdata);
            }
        }
    }
}

[System.Serializable]
public class Point
{
    public int x;
    public int y;
}

[System.Serializable]
public class TaskData
{
    public Dictionary<string, Point> task;
    public string level;
    public bool success;                    // true if correct sequence
    public string status;                   // "continue" or "done"
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public T target;
    public SerializationWrapper(T target) { this.target = target; }
}

[System.Serializable]
public class SubmitData   // For request
{
    public List<List<string>> connections;
}