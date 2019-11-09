using System.Collections;
using System;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HTTPRequest : MonoBehaviour
{
    public enum State
    {
        Waiting,
        Prepared,
        Sending,
        Finish
    }

    // 少し重たい処理なので、使うときはHTTPManagerと連携して管理するべき
    public static HTTPRequest GenerateInstance()
    {
        string goUuid = Guid.NewGuid().ToString();
        GameObject instanceGo = new GameObject(goUuid);
        GameObject.DontDestroyOnLoad(instanceGo);
        return instanceGo.AddComponent<HTTPRequest>();
    }

    private UnityWebRequest webRequest = null;
    private DownloadHandler customDownloadHandler = null;

    private State state = State.Waiting;
    public State CurrentState
    {
        get
        {
            return state;
        }
    }

    public void Reset()
    {
        if (webRequest != null && !webRequest.isDone)
        {
            webRequest.Abort();
        }
        state = State.Waiting;
    }

    public void Abort()
    {
        if (webRequest != null)
        {
            webRequest.Abort();
        }
    }

    public void UpdateDownloadHandler(DownloadHandler downloadHandler)
    {
        if (webRequest == null)
        {
            customDownloadHandler = downloadHandler;
        }
        else
        {
            webRequest.downloadHandler = downloadHandler;
            customDownloadHandler = null;
        }
    }

    public void Request(string url,
                               HTTPMethods methods = HTTPMethods.Get,
                               Dictionary<string, string> headers = null,
                               WWWForm param = null,
                               Action<DownloadHandler> onSuccess = null,
                               Action<float> onProgress = null,
                               Action<DownloadHandler> onError = null)
    {
        StartCoroutine(RequestSync(url: url, methods: methods, param: param, headers: headers, onSuccess: onSuccess, onProgress: onProgress, onError: onError));
    }

    public IEnumerator RequestSync(string url,
                               HTTPMethods methods = HTTPMethods.Get,
                               Dictionary<string, string> headers = null,
                               WWWForm param = null,
                               Action<DownloadHandler> onSuccess = null,
                               Action<float> onProgress = null,
                               Action<DownloadHandler> onError = null)
    {
        switch (methods)
        {
            case HTTPMethods.Get:
                StringBuilder getUrl = new StringBuilder(url);
                if (param != null)
                {
                    getUrl.Append("?");
                    getUrl.Append(Encoding.UTF8.GetString(param.data));
                }
                webRequest = UnityWebRequest.Get(getUrl.ToString());
                break;
            case HTTPMethods.Post:
                webRequest = UnityWebRequest.Post(url, param);
                break;
            case HTTPMethods.Put:
                byte[] bodyData = null;
                if (param != null)
                {
                    bodyData = param.data;
                }
                webRequest = UnityWebRequest.Put(url, bodyData);
                break;
            case HTTPMethods.Delete:
                StringBuilder deleteUrl = new StringBuilder(url);
                if (param != null)
                {
                    deleteUrl.Append("?");
                    deleteUrl.Append(Encoding.UTF8.GetString(param.data));
                }
                webRequest = UnityWebRequest.Delete(deleteUrl.ToString());
                break;
            case HTTPMethods.Head:
                StringBuilder headUrl = new StringBuilder(url);
                if (param != null)
                {
                    headUrl.Append("?");
                    headUrl.Append(Encoding.UTF8.GetString(param.data));
                }
                webRequest = UnityWebRequest.Head(headUrl.ToString());
                break;
        }
        if (headers != null)
        {
            foreach (KeyValuePair<string, string> header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }
        }
        if (customDownloadHandler != null)
        {
            webRequest.downloadHandler = customDownloadHandler;
        }

        // 通信の前後でRequestとResponseのLogをとり、通信にかかった時間を計測する
        Stopwatch stopWatch = new Stopwatch();
        HTTPLogger.LogRequest(webRequest, param, headers);
        stopWatch.Start();
        webRequest.SendWebRequest();
        while (!webRequest.isDone)
        {
            if (onProgress != null) onProgress(webRequest.downloadProgress);
            yield return null;
        }
        if (onProgress != null) onProgress(1.0f);
        stopWatch.Stop();
        HTTPLogger.LogResponse(webRequest, stopWatch.Elapsed, param);

        if (webRequest.isHttpError)
        {
            if (onError != null)
            {
                onError(webRequest.downloadHandler);
            }
        }
        else
        {
            if (onSuccess != null)
            {
                onSuccess(webRequest.downloadHandler);
            }
        }
        yield return webRequest.downloadHandler;
        customDownloadHandler = null;
    }
}