using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Messaging;

namespace NORK
{
    public class Manager_Firebase : MonoBehaviour
    {
        public string token;
        FirebaseApp _app;
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        private void Awake()
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                if (task.Result == DependencyStatus.Available)
                {
                    _app = FirebaseApp.DefaultInstance;
                    FirebaseMessaging.TokenReceived += OnTokenReveived;
                    FirebaseMessaging.MessageReceived += OnMessageReveiced;
                }
                else
                {
                    Debug.LogError("[FIREBASE] Could not resolve all dependencies: " + task.Result);
                }
            });
        }
#endif
        public void GetToken()
        {
            FirebaseMessaging.GetTokenAsync().ContinueWith(n =>
            {
                token = n.Result;
                Debug.Log(n.Result);
            });
        }
        private void OnTokenReveived(object sender, TokenReceivedEventArgs e)
        {
            if (e != null)
            {
                token = e.Token;
                Debug.LogFormat("[FIREBASE] Token: {0}", e.Token);
            }
        }

        private void OnMessageReveiced(object sender, MessageReceivedEventArgs e)
        {
            if (e != null && e.Message != null && e.Message.Notification != null)
            {
                Debug.LogFormat("[FIREBASE] From: {0}, Title: {1}, Text: {2}, {3}",
                    e.Message.From,
                    e.Message.Notification.Title,
                    e.Message.Notification.Body);
                foreach (var item in e.Message.Data)
                {
                    Debug.Log($"{item.Key} : {item.Value}");
                }
            }
        }
    }
}
