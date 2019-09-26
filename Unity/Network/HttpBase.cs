using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using EFO.Unity.AsyncOperation;


namespace EFO.Unity.Network.Http {

    public static class HttpTools {
        public static void setHeaders(UnityWebRequest req, List<KeyValuePair<string, string>> headers) {
            foreach (KeyValuePair<string, string> header in headers) {
                req.SetRequestHeader(header.Key, header.Value);
            }
        }
    }

    public enum NetworkErrorCode {
        None                    = 0,
        NotConnected            = -1,
        BadRequest              = HttpStatusCode.BadRequest,
        Forbidden               = HttpStatusCode.Forbidden,
        InternalServerError     = HttpStatusCode.InternalServerError,
        NotFound                = HttpStatusCode.NotFound,
        UnsuppoertedMediaType   = HttpStatusCode.UnsupportedMediaType,
    }

    public struct ResponseData {
        public readonly string Data;
        public readonly bool IsSucceeded;
        public readonly long Code;

        public ResponseData(bool isSuccessful, long code, string data = null) {
            IsSucceeded = isSuccessful;
            this.Data = data;

            Code = NetworkTools.IsNetworkReachable() ? 
                code : (int)NetworkErrorCode.NotConnected;
        }
    }

    public class PostRequest: AsyncRequest<ResponseData> {
        private readonly string _uri;
        private readonly string _data;
        private readonly MonoBehaviour _mono;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        public PostRequest (MonoBehaviour mono ,string uri, string data) {
            _uri = uri;
            _data = data;
            _mono = mono;
        }

        public void SetHeader(string key, string value) {
            _headers.Add(new KeyValuePair<string, string>(key, value));
        }

        protected override void SendOperation() {
            _mono.StartCoroutine(Send());
        }

        private new IEnumerator Send() {
            using (var req = UnityWebRequest.Post(_uri, "")) {
                var data = new UploadHandlerRaw(Encoding.UTF8.GetBytes(_data));
                data.contentType = "application/json";

                req.uploadHandler = data;
                
                HttpTools.setHeaders(req ,_headers);
                yield return req.SendWebRequest();

                base.data = new ResponseData(
                     (!req.isNetworkError || !req.isHttpError)
                    , req.responseCode
                    , req.downloadHandler.text
                    );

                base.isDone = true;
            }
        }
    }

    public class GetRequest: AsyncRequest<ResponseData> {

        private string _uri;
        private MonoBehaviour _mono;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        public GetRequest (MonoBehaviour mono, string uri) {
            _uri = uri;
            _mono = mono;
        }

        public void SetHeader(string key, string value) {
            _headers.Add(new KeyValuePair<string, string>(key, value));
        }

        protected override void SendOperation() {
            _mono.StartCoroutine(Send());
        }

        private new IEnumerator Send() {
            using (var req = UnityWebRequest.Get(_uri)) {
                
                HttpTools.setHeaders(req ,_headers);
                yield return req.SendWebRequest();

                base.data = new ResponseData(
                     (!req.isNetworkError || !req.isHttpError)
                    , req.responseCode
                    , req.downloadHandler.text
                    );

                base.isDone = true;
            }
        }
    }

    public class PutRequest: AsyncRequest<ResponseData> {
        private string _uri;
        private string _data;
        private MonoBehaviour _mono;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        public PutRequest (MonoBehaviour mono, string uri, string data) {
            _uri = uri;
            _data = data;
            _mono = mono;
        }

        public void SetHeader(string key, string value) {
            _headers.Add(new KeyValuePair<string, string>(key, value));
        }

        protected override void SendOperation() {
            _mono.StartCoroutine(Send());
        }

        private new IEnumerator Send() {
            using (var req = UnityWebRequest.Put(_uri, _data)) {

                HttpTools.setHeaders(req ,_headers);
                yield return req.SendWebRequest();

                base.data = new ResponseData(
                     (!req.isNetworkError || !req.isHttpError)
                    , req.responseCode
                    , req.downloadHandler.text
                    );

                base.isDone = true;
            }
        }
    }

    public class DeleteRequest: AsyncRequest<ResponseData> {
        private string _uri;
        private MonoBehaviour _mono;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();

        public DeleteRequest (MonoBehaviour mono, string uri, string data) {
            _uri = uri;
            _mono = mono;
        }

        public void SetHeader(string key, string value) {
            _headers.Add(new KeyValuePair<string, string>(key, value));
        }

        protected override void SendOperation() {
            _mono.StartCoroutine(Send());
        }

        private new IEnumerator Send() {
            using (var req = UnityWebRequest.Delete(_uri)) {
                
                HttpTools.setHeaders(req ,_headers);
                yield return req.SendWebRequest();

                base.data = new ResponseData(
                     (!req.isNetworkError || !req.isHttpError)
                    , req.responseCode
                    , req.downloadHandler.text
                    );

                base.isDone = true;
            }
        }
    }

    public class PatchRequest : AsyncRequest<ResponseData> {

        public PatchRequest() {
            throw new NotImplementedException();
        }
        protected override void SendOperation() {
            throw new NotImplementedException();
        }
    }
}