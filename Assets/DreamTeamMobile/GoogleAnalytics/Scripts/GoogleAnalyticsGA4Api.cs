using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace DreamTeamMobile
{
    /// <summary>
    /// Implementation of Google Analytics GA4 Measurement Protocol
    /// https://developers.google.com/analytics/devguides/collection/protocol/ga4/reference?client_type=gtag#payload
    /// </summary>
    public class GoogleAnalyticsGA4Api
    {
        private const string GA4ApiEndpoint = "https://www.google-analytics.com/mp/collect";
        private readonly string _measurementId;
        private readonly string _apiSecret;
        private readonly string _deviceId;
        private readonly int _defaultEngagementTimeInSec;
        private readonly string _sessionId;

        public GoogleAnalyticsGA4Api(string measurementId, string apiSecret, string deviceId, int defaultEngagementTimeInSec = 100)
        {
            if (string.IsNullOrWhiteSpace(measurementId))
                throw new ArgumentNullException(nameof(measurementId));

            if (string.IsNullOrWhiteSpace(apiSecret))
                throw new ArgumentNullException(nameof(apiSecret));

            _measurementId = measurementId;
            _apiSecret = apiSecret;
            _deviceId = deviceId;
            _defaultEngagementTimeInSec = defaultEngagementTimeInSec;
            _sessionId = Guid.NewGuid().ToString();
        }

        public void TrackEvent(string name, Dictionary<string, object> @params = null)
        {
            Track(name, @params);
        }

        private void Track(string name, Dictionary<string, object> @params)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("category");

            try
            {
                var eventParams = new Dictionary<string, object>
                {
                    { "session_id", _sessionId },
                    { "engagement_time_msec", _defaultEngagementTimeInSec }
                };

                if (@params != null)
                {
                    foreach (var item in @params.Take(23))
                        eventParams[item.Key] = item.Value;
                }

                var postData = new GA4Data
                {
                    client_id = _deviceId,
                    events = new GA4DataEvent[]
                    {
                        new GA4DataEvent
                        {
                            name = Uri.EscapeUriString(name),
                        }
                    }
                };

                var url = $"{GA4ApiEndpoint}?measurement_id={_measurementId}&api_secret={_apiSecret}";
                var postDataString = JsonUtility.ToJson(postData).Replace("\"<params>\"", eventParams.ToJson());
                GA4PostEventToGoogleAnalytics(url, postDataString);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log($"[DTM GA4] Failed to track GA event: {ex}");
            }
        }

        private void GA4PostEventToGoogleAnalytics(string url, string postDataString)
        {
            //Debug.Log($"[DTM GA4] About to send POST HTTP request to: {url}, payload: {postDataString}");
#if UNITY_WEBGL && !UNITY_EDITOR
            GA4PostEvent(url, postDataString);
#else
            var stringContent = new StringContent(postDataString, Encoding.UTF8, "application/json");
            new HttpClient().PostAsync(url, stringContent)
                .ContinueWith(t =>
                {
                    var response = t.Result;
                    if (!response.IsSuccessStatusCode)
                        UnityEngine.Debug.Log($"[DTM GA4] <NON_WEBGL> Failed to submit GA event: {response.StatusCode}");
                });
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GA4PostEvent(string url, string postDataString);
#endif
    }
}

[Serializable]
public class GA4Data
{
    public string client_id;
    public GA4DataEvent[] events;
}

[Serializable]
public class GA4DataEvent
{
    public string name;
    public string @params = "<params>";
}