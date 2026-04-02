using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DreamTeamMobile
{
    public class GoogleAnalytics
    {
        private GoogleAnalyticsSettings _settings;
        private readonly GoogleAnalyticsGA4Api _gaClient;
        private static GoogleAnalytics _instance;

        public static GoogleAnalytics Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GoogleAnalytics();
                return _instance;
            }
        }

        private GoogleAnalytics()
        {
            _settings = Resources.LoadAll<GoogleAnalyticsSettings>("GoogleAnalyticsSettings").FirstOrDefault();
            EnsureValidSettings();

            _gaClient = new GoogleAnalyticsGA4Api(_settings.GA4MeasurementId, _settings.GA4StreamApiSecret, GetDeviceId());

            if (_settings.TrackApplicationErrors)
            {
                Application.logMessageReceived += Application_logMessageReceived;
            }

            if (_settings.TrackSceneChangeEvents)
            {
                SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            }
        }

        public void TrackError(Exception ex, [CallerMemberName] string memberName = "")
        {
            if (ex == null)
                return;

            EnsureValidSettings();
            Debug.LogError($"[DTM GA4] Tracking error: {ex}");
            _gaClient.TrackEvent($"Error_{memberName}", new Dictionary<string, object>
            {
                {"Message", ex.Message }
            });
        }

        /// <summary>
        /// Limitations:
        /// - Requests can have a maximum of 25 events.
        /// - Events can have a maximum of 25 parameters.
        /// - Events can have a maximum of 25 user properties.
        /// - User property names must be 24 characters or fewer.
        /// - User property values must be 36 characters or fewer.
        /// - Event names must be 40 characters or fewer, can only contain alpha-numeric characters and underscores, and must start with an alphabetic character.
        /// - Parameter names including item parameters must be 40 characters or fewer, can only contain alpha-numeric characters and underscores, and must start with an alphabetic character.
        /// - Parameter values including item parameter values must be 100 characters or fewer.
        /// - Item parameters can have a maximum of 10 custom parameters.
        /// </summary>
        public void TrackEvent(string eventName, Dictionary<string, object> eventParams = null)
        {
            EnsureValidSettings();
            Debug.Log($"[DTM GA4] Tracking event: {eventName}, params: {eventParams?.Count}");
            _gaClient.TrackEvent(eventName, eventParams);
        }

        private string GetDeviceId()
        {
            var deviceId = SystemInfo.deviceUniqueIdentifier;
            if (!string.IsNullOrWhiteSpace(deviceId) && deviceId != "n/a")
                return deviceId;

            deviceId = PlayerPrefs.GetString(nameof(deviceId));
            if (string.IsNullOrWhiteSpace(deviceId))
            {
                deviceId = Guid.NewGuid().ToString();
                PlayerPrefs.SetString(nameof(deviceId), deviceId);
            }

            return deviceId;
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            TrackEvent($"SceneLoaded_{arg1.name}");
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType logType)
        {
            if (logType == LogType.Exception || logType == LogType.Error)
            {
                var ex = new Exception(condition);
                ex.Data[nameof(stackTrace)] = stackTrace;
                ex.Data[nameof(logType)] = logType;
                TrackError(ex);
            }
        }

        private void EnsureValidSettings()
        {
            if (_settings == null)
            {
                throw new InvalidOperationException("GoogleAnalytics settings are not found. Please use the Window/GoogleAnalytics menu option to generate the settings.");
            }

            if (string.IsNullOrWhiteSpace(_settings.GA4MeasurementId))
            {
                throw new InvalidOperationException("GoogleAnalytics GA4MeasurementId is not configured properly. Please use the Window/GoogleAnalytics menu option to locate and configure the settings.");
            }


            if (string.IsNullOrWhiteSpace(_settings.GA4StreamApiSecret))
            {
                throw new InvalidOperationException("GoogleAnalytics GA4StreamApiSecret is not configured properly. Please use the Window/GoogleAnalytics menu option to locate and configure the settings.");
            }
        }
    }
}