using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DefaultNamespace.Data;
using DefaultNamespace.Data.DataStructures;
using DefaultNamespace.Data.Statuses;
using Lexone.UnityTwitchChat;
using TMPro;
using UI.Shared;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class TwitchPollManager : MonoBehaviour
    {
        public static TwitchPollManager instance;
        private int pollDuration => SaveFile.Instance.ConfigurationFile.TwitchPollDuration;
        [SerializeField] private TextMeshProUGUI textTitle;
        [SerializeField] private TextMeshProUGUI textTimeLeft;
        [SerializeField] private GameObject integrationWindow;
        [SerializeField] private List<PollOption> uiPollOptions;
        [SerializeField] private FancyNotificationDisplay pollResultPanel;
        private bool _isPollRunning;
        private float _pollTimeLeft;
        private readonly Dictionary<int, PollEntry> _pollCounts = new ();
        private readonly List<string> _votedNames = new ();

        private void Start()
        {
            if (instance == null)
                instance = this;
            
            if (TwitchIntegrationManager.instance.IsEnabledAndConnected())
                StatusEffectManager.instance.AddEffect(StatusEffectType.TwtichIntegration, 1);
        }

        private void Update()
        {
            if (!TwitchIntegrationManager.instance.IsEnabledAndConnected())
                return;
            integrationWindow.SetActive(IsAnyPoolRunning());
            if (_pollTimeLeft > 0)
                _pollTimeLeft -= Time.unscaledDeltaTime;
            if (_pollTimeLeft < 0)
                _pollTimeLeft = 0;

            if (!IsAnyPoolRunning()) return;
            
            textTimeLeft.text = $"{_pollTimeLeft:0.00}s.";
        }

        public void StartPoll<T>(string title, string[] pollChoices, Action<int, List<T>> callback, List<T> inputArray)
        {
            if (IsAnyPoolRunning())
                return;
            
            textTitle.text = title;
            _pollCounts.Clear();
            _votedNames.Clear();
            uiPollOptions.ForEach(x => x.gameObject.SetActive(false));
            for (var i = 1; i <= pollChoices.Length; i++)
            {
                var pollEntry = new PollEntry()
                {
                    id = i,
                    text = pollChoices[i - 1],
                    voteCount = 0
                };
                _pollCounts.TryAdd(i, pollEntry);
                uiPollOptions[i-1].Setup(pollEntry, _votedNames);
                uiPollOptions[i-1].gameObject.SetActive(true);
            }

            IRC.Instance.OnChatMessage += OnChatMessage;
            StartCoroutine(WaitForPollResults(callback, inputArray));
        }

        private void OnChatMessage(Chatter obj)
        {
            if (int.TryParse(obj.message.Trim(), out var index) && _pollCounts.TryGetValue(index, out var count))
            {
                if (_votedNames.Contains(obj.login)) 
                    return;
                _votedNames.Add(obj.login);
                count.voteCount++;
            }
        }

        private IEnumerator WaitForPollResults<T>(Action<int, List<T>> callback, List<T> inputArray)
        {
            try
            {
                _isPollRunning = true;
                _pollTimeLeft = pollDuration;
                yield return new WaitForSecondsRealtime(pollDuration);
                var result = GetPollResult();
                pollResultPanel.Display(_pollCounts[result+1].text, "Poll result");
                callback(result, inputArray);
            }
            finally
            {
                IRC.Instance.OnChatMessage -= OnChatMessage;
                _isPollRunning = false;
            }
        }

        public bool IsAnyPoolRunning()
        {
            return _isPollRunning;
        }

        private int GetPollResult()
        {
            return _pollCounts.OrderByDescending(x => x.Value.voteCount).ThenBy(_ => Random.value).First().Key - 1;
        }
    }
}