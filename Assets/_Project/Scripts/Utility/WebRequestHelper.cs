using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Scripts.Utility
{
    public static class WebRequestHelper
    {
        public static async Task<ResponseData> AsyncGetRequest(string url, CancellationTokenSource cancelTokenSource)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);

            request.SendWebRequest();
            while (!request.isDone && cancelTokenSource is { IsCancellationRequested: false })
            {
                await Task.Yield();
            }

            var response = new ResponseData(request);

            if (response.IsError && !cancelTokenSource.IsCancellationRequested)
            {
                Debug.LogError($"Error While Sending {request.error}");
            }

            return new ResponseData(request);
        }
    }
}