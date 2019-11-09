using System.Collections;
using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class HTTPManager
{
    private HTTPManager()
    {
    }

    // Singleton
    private static HTTPManager instance;

    public static HTTPManager Instance
    {
        get
        {
            if (HTTPManager.instance == null)
            {
                HTTPManager.instance = new HTTPManager();
            }
            return HTTPManager.instance;
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
        HTTPRequest request = HTTPRequest.GenerateInstance();
        request.Request(
            url: url,
            methods: methods,
            headers: headers,
            param: param,
            onSuccess: (downloadHandler) =>
            {
                if (onSuccess != null) onSuccess(downloadHandler);
                GameObject.Destroy(request.gameObject);
            },
            onProgress: onProgress,
            onError: (downloadHandler) =>
            {
                if (onError != null) onError(downloadHandler);
                GameObject.Destroy(request.gameObject);
            }
        );
    }

    public IEnumerator RequestSync(string url,
                       HTTPMethods methods = HTTPMethods.Get,
                       Dictionary<string, string> headers = null,
                       WWWForm param = null,
                       Action<DownloadHandler> onSuccess = null,
                       Action<float> onProgress = null,
                       Action<DownloadHandler> onError = null)
    {
        HTTPRequest request = HTTPRequest.GenerateInstance();
        return request.RequestSync(
            url: url,
            methods: methods,
            headers: headers,
            param: param,
            onSuccess: (downloadHandler) =>
            {
                if (onSuccess != null) onSuccess(downloadHandler);
                GameObject.Destroy(request.gameObject);
            },
            onProgress: onProgress,
            onError: (downloadHandler) =>
            {
                if (onError != null) onError(downloadHandler);
                GameObject.Destroy(request.gameObject);
            });
    }
}