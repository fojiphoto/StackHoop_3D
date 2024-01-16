using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
//using Facebook.Unity;

public class FirebaseInitialize : MonoBehaviour
{
    public static FirebaseInitialize instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    protected bool firebaseInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        InitializeFirebase();
        GameAnalyticsSDK.GameAnalytics.Initialize();
#endif
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(
          FirebaseAnalytics.UserPropertySignUpMethod,
          "Google");
        // Set the user ID.
        //FirebaseAnalytics.SetUserId("user_analytics");
        // Set default session duration values.
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
        

        AnalyticsLogin();
    }

    // End our analytics session when the program exits.
    void OnDestroy() { }

    public void AnalyticsLogin()
    {
        // Log an event with no parameters.
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
        Debug.Log("Firebase Initalized Successfully");
    }
    public void PurchaseEvent()
    {
        try
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventPurchase);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    public void ShareEvent()
    {
        try
        {
            FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventShare);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void LogEvent(string evt, string key, string value)
    {
        // Log an event with a float.

        FirebaseAnalytics.LogEvent(evt);
        //FirebaseAnalytics.LogEvent(evt, key, value);
    }
    
    public void LogEvent1(string value)
    {
        FirebaseAnalytics.LogEvent(value);
        GameAnalyticsSDK.GameAnalytics.NewDesignEvent(value);
    }

    public void AnalyticsScore()
    {
        // Log an event with an int parameter.
        Debug.Log("Logging a post-score event.");
        FirebaseAnalytics.LogEvent(
          FirebaseAnalytics.EventPostScore,
          FirebaseAnalytics.ParameterScore,
          42);
    }

    public void AnalyticsGroupJoin()
    {
        // Log an event with a string parameter.
        Debug.Log("Logging a group join event.");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventJoinGroup, FirebaseAnalytics.ParameterGroupId,
          "spoon_welders");
    }

    public void AnalyticsLevelUp()
    {
        // Log an event with multiple parameters.
        Debug.Log("Logging a level up event.");
        FirebaseAnalytics.LogEvent(
          FirebaseAnalytics.EventLevelUp,
          new Parameter(FirebaseAnalytics.ParameterLevel, 5),
          new Parameter(FirebaseAnalytics.ParameterCharacter, "mrspoon"),
          new Parameter("hit_accuracy", 3.14f));
    }

    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
        Debug.Log("Reset analytics data.");
        FirebaseAnalytics.ResetAnalyticsData();
    }




    // Get the current app instance ID.
    //public Task<string> DisplayAnalyticsInstanceId()
    //{
    //    return FirebaseAnalytics.GetAnalyticsInstanceIdAsync().ContinueWithOnMainThread(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            Debug.Log("App instance ID fetch was canceled.");
    //        }
    //        else if (task.IsFaulted)
    //        {
    //            Debug.Log(String.Format("Encounted an error fetching app instance ID {0}",
    //                                    task.Exception.ToString()));
    //        }
    //        else if (task.IsCompleted)
    //        {
    //            Debug.Log(String.Format("App instance ID: {0}", task.Result));
    //        }
    //        return task;
    //    }).Unwrap();
    //}




}
