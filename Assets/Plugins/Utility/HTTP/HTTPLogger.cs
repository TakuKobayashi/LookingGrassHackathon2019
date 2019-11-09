using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPLogger
{
    static HTTPLogger()
    {
    }

    public static bool Enabled = false;

    static void Log(string text)
    {
        if (Enabled) UnityEngine.Debug.Log(text);
    }

    /// <summary>
    /// <para>(実機では記録しない)RequestLog(HTTP通信のRequest(送信)する直前に呼ばれてログとして出力する)</para>
    /// <para>【第1引数】(これから送信する)UnityWebRequestのHTTPRequest</para>
    /// <para>【第2引数】requestとして送りたいパラメーター</para>
    /// <para>【第3引数】requestのHeaderとして送りたいKey, Valueの情報</para>
    /// </summary>
    public static void LogRequest(UnityWebRequest req, WWWForm requestParams, Dictionary<string, string> requestHeader)
    {
        if (Enabled) Log(RequestAsString(req, requestParams, requestHeader));
    }

    /// <summary>
    /// <para>(実機では記録しない)ResponseLog(HTTP通信が完了したときに呼ばれてログとして出力する)</para>
    /// <para>【第1引数】(送信した)UnityWebRequestのHTTPRequest</para>
    /// <para>【第2引数】ベンチマーク(サーバーへ送信してから受信までにかかった時間)</para>
    /// <para>【第3引数】requestとして送ったパラメーター</para>
    /// </summary>
    public static void LogResponse(UnityWebRequest req, TimeSpan ts, WWWForm requestParams)
    {
        if (Enabled) Log(ResponseAsString(req, ts, requestParams));
    }

    private static string RequestAsString(UnityWebRequest req, WWWForm requestParams, Dictionary<string, string> requestHeaders)
    {
        StringBuilder reqStr = new StringBuilder();
        string queryUrl = req.url;
        string upperMethod = req.method.ToUpper();
        if (requestParams != null && (upperMethod == "POST" || upperMethod == "PUT"))
        {
            queryUrl += "?" + Encoding.UTF8.GetString(requestParams.data);
        }
        reqStr.Append("-> " + req.method.ToUpper() + " " + queryUrl);
        reqStr.Append("\n\n");
        if (requestHeaders != null)
        {
            StringBuilder reqHeaderStr = new StringBuilder();
            foreach (var reqHeader in requestHeaders)
            {
                reqHeaderStr.Append(reqHeader.Key);
                reqHeaderStr.Append(": ");
                reqHeaderStr.Append(reqHeader.Value);
                reqHeaderStr.Append("\n");
            }
            reqStr.Append(reqHeaderStr.ToString());
            reqStr.Append("\n\n");
        }
        return reqStr.ToString();
    }

    private static string ResponseAsString(UnityWebRequest req, TimeSpan ts, WWWForm requestParams)
    {
        StringBuilder resStr = new StringBuilder();
        if (req == null)
        {
            resStr.Insert(0, "<color=red>");
            resStr.Append("Invalid Request");
            resStr.Append("</color>");
        }
        else
        {
            string queryUrl = req.url;
            string upperMethod = req.method.ToUpper();
            if (requestParams != null && (upperMethod == "POST" || upperMethod == "PUT"))
            {
                queryUrl += "?" + Encoding.UTF8.GetString(requestParams.data);
            }
            resStr.Append("<- " + req.method.ToUpper() + " " + queryUrl);
            resStr.Append("\n");
            resStr.Append(req.responseCode);
            resStr.Append("\n");
            resStr.Append("  (" + DiagnosticDataAsString(req, ts) + ")");
            if (req.isHttpError)
            {
                resStr.Insert(0, "<color=red>");
                resStr.Append("</color>");
            }
            resStr.Append("\n\n");
            Dictionary<string, string> responseHeaders = req.GetResponseHeaders();
            StringBuilder resHeaderStr = new StringBuilder();
            foreach (var resHeader in responseHeaders)
            {
                resHeaderStr.Append(resHeader.Key);
                resHeaderStr.Append(": ");
                resHeaderStr.Append(resHeader.Value);
                resHeaderStr.Append("\n");
            }
            resStr.Append(resHeaderStr.ToString());
            resStr.Append("\n\n");
            resStr.Append(req.downloadHandler.text);
            resStr.Append("\n\n");
            if (!string.IsNullOrEmpty(req.error))
            {
                resStr.Append("Exception: " + req.error);
                resStr.Append("\n\n");
            }
        }
        return resStr.ToString();
    }

    private static string DiagnosticDataAsString(UnityWebRequest req, TimeSpan ts)
    {
        List<string> optStr = new List<string>();
        optStr.Add("RTT: " + Convert.ToInt32(ts.TotalMilliseconds) + "ms");
        Dictionary<string, string> responseHeaders = req.GetResponseHeaders();
        if (responseHeaders.ContainsKey("x-runtime"))
        {
            optStr.Add("Server: " + (int)(Convert.ToDouble(responseHeaders["x-runtime"]) * 1000) + "ms");
        }
        if (req.downloadHandler.data != null)
        {
            double len = (double)req.downloadHandler.data.Length;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            optStr.Add(String.Format("Size: {0:0.#} {1}", len, sizes[order]));
        }
        return String.Join(" | ", optStr.ToArray());
    }
}