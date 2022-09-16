using UnityEngine.Networking;

namespace _Project.Scripts.Utility
{
    public class ResponseData
    {
        public readonly long ResponseCode;
        public readonly byte[] Data;
        public readonly string Text;
        public readonly bool IsError;

        public ResponseData(UnityWebRequest request)
        {
            ResponseCode = request.responseCode;
            Data = request.downloadHandler.data;
            Text = request.downloadHandler.text;
            IsError = ResponseCode != 200 && ResponseCode != 201;
        }
    }
}