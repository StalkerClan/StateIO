using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using System.Threading.Tasks;
using System;

public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager instance;
    public static FirebaseManager Instance { get { return instance; } }

    private bool fireBaseReady = false;
    public bool FireBaseReady { get { return fireBaseReady; } }

    private bool configChecked = false;
    public bool ConfigChecked { get { return configChecked; } }

    private bool firebaseInitializing = false;

    private float lastGetConfigTime = 0;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    private void Start()
    {
        firebaseInitializing = true;
        AddFirebaseMessagingEvents();
        InitFireBase();
        StartCoroutine(WaitForFirebaseInit());
    }

    public void OnDestroy()
    {
        RemoveFirebaseMessagingEvents();
    }

    private void InitFireBase()
    {
        //Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
        // Initialize Firebase
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase Ready, Keep Going");
                Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                fireBaseReady = true;
            }
            else
            {
                Debug.LogError(string.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                configChecked = true;
            }
            firebaseInitializing = false;
        });
    }

    private IEnumerator WaitForFirebaseInit()
    {
        yield return new WaitUntil(() => !firebaseInitializing);
        yield return new WaitForSecondsRealtime(0.5f);

        if (fireBaseReady)
        {
            SetDefaultRemoteConfigValue();
            InitFirebaseMessaging();
            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            
        }
    }


    #region FirebaseConfig

    private void SetDefaultRemoteConfigValue()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();

        // These are the values that are used if we haven't fetched data from the
        // server
        // yet, or if we ask for values that the server doesn't have:
#if UNITY_ANDROID || UNITY_STANDALONE
        defaults.Add("key", 1);
#elif UNITY_IOS
        defaults.Add("key_ios", 1);
#endif

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(t =>
            {
                // [END set_defaults]
                FetchRemoteConfigAsync();
            });
    }

    // Start a fetch request.
    // FetchAsync only fetches new data if the current data is older than the provided
    // timespan.  Otherwise it assumes the data is "recent enough", and does nothing.
    // By default the timespan is 12 hours, and for production apps, this is a good
    // number. For this example though, it's set to a timespan of zero, so that
    // changes in the console will always show up immediately. 

    public void FetchRemoteConfigAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
            Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
            Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
            Debug.Log("Fetch completed successfully!");
        }

        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).",
                                   info.FetchTime));
                    CheckRemoteConfig();
                });

                break;
            case LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    private void CheckRemoteConfig()
    {
        FirebaseRemoteConfig config = FirebaseRemoteConfig.DefaultInstance;

#if UNITY_ANDROID || UNITY_STANDALONE
        int value = int.Parse(config.GetValue("key").StringValue);
#elif UNITY_IOS
        int value = int.Parse(config.GetValue("key_ios").StringValue);
#endif

        configChecked = true;
    }

    public void CheckConfig()
    {
        if (Time.time - lastGetConfigTime > 300)
        {
            StartCoroutine(ReCheckFirebase());
            lastGetConfigTime = Time.time;
        }
    }

    private IEnumerator ReCheckFirebase()
    {
        if (!fireBaseReady)
        {
            InitFireBase();
            yield return new WaitUntil(() => !firebaseInitializing);
            yield return new WaitForSecondsRealtime(0.5f);
        }

        if (fireBaseReady)
        {
            configChecked = false;
            FetchRemoteConfigAsync();
            //yield return new WaitUntil(() => configChecked);
            //PopupManager.Instance.ShowForceUpdate();
        }
    }

    #endregion FirebaseConfig


    #region FirebaseMessaging

    private void AddFirebaseMessagingEvents()
    {
        //Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
    }

    private void RemoveFirebaseMessagingEvents()
    {
        //Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        //Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }

    private void InitFirebaseMessaging()
    {

        //Firebase.Messaging.FirebaseMessaging.SubscribeAsync("TestTopic").ContinueWithOnMainThread(task => {
        //    LogTaskCompletion(task, "SubscribeAsync");
        //});
        //Debug.Log("Firebase Messaging Initialized");

        //// This will display the prompt to request permission to receive
        //// notifications if the prompt has not already been displayed before. (If
        //// the user already responded to the prompt, thier decision is cached by
        //// the OS and can be changed in the OS settings).
        //Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
        //  task =>
        //  {
        //      LogTaskCompletion(task, "RequestPermissionAsync");
        //  }
        //);
    }

    #endregion FirebaseMessaging
}
