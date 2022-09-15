using UnityEngine.Networking;

namespace _Project.Scripts.Utility
{
    public class ResponseData
    {
        public readonly long ResponseCode;
        public readonly DownloadHandler DownloadHandler;
        public readonly bool IsError;

        public ResponseData(UnityWebRequest request)
        {
            ResponseCode = request.responseCode;
            DownloadHandler = request.downloadHandler;
            IsError = ResponseCode != 200 && ResponseCode != 201;
        }
    }
}