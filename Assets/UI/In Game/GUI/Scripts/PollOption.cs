using System;
using System.Collections.Generic;
using DefaultNamespace.Data.DataStructures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PollOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private Slider sliderVoteBar;
    private List<string> _voters = new ();
    private PollEntry _pollEntry;

    public void Setup(PollEntry pollEntry, List<string> counter)
    {
        _pollEntry = pollEntry; 
        _voters = counter;
    }

    private void Update()
    {
        if (_pollEntry == null) return;
        
        textTitle.text = _pollEntry.GetDisplay();
        sliderVoteBar.minValue = 0;
        sliderVoteBar.maxValue = _voters.Count;
        sliderVoteBar.value = _pollEntry.voteCount;
    }
}
