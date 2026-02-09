using System.Collections.Generic;
using _Project.Scripts.Game;
using UnityEngine;
using Newtonsoft.Json;

namespace _Project.Scripts.Analytics
{
	public class AnalyticsWrapper
	{
		private const string AmplitudeFileName = "AmplitudeKey";
		
		private string _instanceName;
		private string _apiKey;
		private string _serverURL;
		private bool _logs;
		private bool _trackSessionEvents;
		
		private Amplitude _amplitude;
		private GameController _controller;

		private readonly Dictionary<AnalyticsEvent, string> _eventNames = new()
		{
			{ AnalyticsEvent.GameStarted, "game-start" },
			{ AnalyticsEvent.LevelStarted, "level-start" },
			{ AnalyticsEvent.LevelUnblocked, "level-unblocked" },
			{ AnalyticsEvent.LevelCompleted, "level-complete" }
		};

		public AnalyticsWrapper(GameController controller)
		{
			_controller = controller;
			LoadAmplitudeData();
			InitializeService();
		}

		// I'm simulation the process of load the key from somewhere
		private void LoadAmplitudeData()
		{
			var file = Resources.Load<TextAsset>(AmplitudeFileName);
			var data = JsonConvert.DeserializeObject<AmplitudeData>(file.text);
			
			_instanceName = data.instanceName;
			_apiKey = data.apiKey;
			_serverURL = data.serverURL;
			_logs = data.logs;
			_trackSessionEvents = data.trackSessionEvents;
		}

		private void InitializeService()
		{
			_amplitude = Amplitude.getInstance(_instanceName);
			_amplitude.logging = _logs;
			_amplitude.trackSessionEvents(_trackSessionEvents);
			_amplitude.init(_apiKey);
		}

		// Not ideal to store a reference to the model data in this class.
		// We could create a better way to gather the data for each Analytics Event, but that would be an overengineering for this assignment
		public void SendEvent(AnalyticsEvent eventId)
		{
			var eventName = _eventNames[eventId];
			var model = _controller.GameModel;
			var properties = new Dictionary<string, object>()
			{
				{"platform", Application.platform.ToString()},
				{ "currentLevel", model.CurrentLevel },
				{ "maxLevel", model.MaxLevel },
				{ "score", model.Score }
			};
			
			Amplitude.getInstance().logEvent(eventName, properties);
		}
	}

	internal class AmplitudeData
	{
		public string instanceName;
		public string apiKey;
		public string serverURL;
		public bool logs;
		public bool trackSessionEvents;
	}
}