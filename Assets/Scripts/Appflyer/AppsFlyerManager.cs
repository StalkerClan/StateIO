using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

public class AppsFlyerManager : MonoBehaviour, IAppsFlyerConversionData
{
    private static AppsFlyerManager instance;
    public static AppsFlyerManager Instance { 
        get 
        { 
            if(instance == null)
            {
                GameObject ga = new GameObject("AppsFlyer");
                instance = ga.AddComponent<AppsFlyerManager>();
                DontDestroyOnLoad(ga);
            }

            return instance;
        } 
    }


#if UNITY_IOS
    string appId = "1610052186";
    string appKey = "G3MBmMRHTuEpXbqyqSWGeK";
#else
    string appId = "com.alienshooter.galaxy.attack2";
    string appKey = "Gio8zFFVzAZJGr9sbHXoVb";
#endif

    public void Init()
    {
        AppsFlyer.initSDK(appKey, appId, this);
        AppsFlyer.startSDK();
        //
        //Dictionary<string, string> eventValue = new Dictionary<string, string>();
        //eventValue.Add("af_time_session", Static.CurrentRealTimeInSecond + "");
        //AppsFlyer.sendEvent("af_time_session", eventValue);
    }

    public void TrackAppsFlyerPurchase(string purchaseId, decimal cost, string currency)
    {
        float fCost = (float)cost;
        fCost *= 0.63f;
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add(AFInAppEvents.REVENUE, fCost.ToString());
        eventValue.Add(AFInAppEvents.CURRENCY, currency);
        eventValue.Add(AFInAppEvents.QUANTITY, "1");
        AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, eventValue);
    }

    public void TrackAdWatched()
    {
        AppsFlyer.sendEvent("ad_watched", new Dictionary<string, string>());
    }

    public void SendEvent(string eventName, Dictionary<string, string> pairs)
    {
        AppsFlyer.sendEvent(eventName, pairs);
    }

    // Check conversion data
    public void onConversionDataSuccess(string conversionData)
    {
        //AppsFlyer.AFLog("onConversionDataSuccess", conversionData);
        //Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);

        ////https://app.appsflyer.com/com.alienshooter.galaxy.attack2?pid=GG&c=Invader_and_cpv_US_PLY_v1220_100_0226_en

        //foreach (string key in conversionDataDictionary.Keys)
        //{
        //    Debug.Log("Appsflyer: " + key + " : " + conversionDataDictionary[key]);
        //}

        //bool isFirstLaunch = (bool)conversionDataDictionary["is_first_launch"];
        //string status = (string)conversionDataDictionary["af_status"];

        //if(status == "Non-organic")
        //{
        //    string sourceID = (string)conversionDataDictionary["media_source"];
        //    string campaign = (string)conversionDataDictionary["campaign"];
        //}
        //else
        //{

        //}
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("onConversionDataFail", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }
}
