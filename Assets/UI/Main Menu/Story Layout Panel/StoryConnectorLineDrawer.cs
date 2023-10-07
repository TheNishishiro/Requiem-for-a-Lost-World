using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.Main_Menu.Story_Layout_Panel;
using UnityEngine;
using Random = UnityEngine.Random;

public class StoryConnectorLineDrawer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private List<RectTransform> _storyEntryRects;
    
    void Start()
    {
        var tiles = FindObjectsOfType<StoryTile>()
            .OrderBy(x => x.loreEntry?.EntryNumber)
            .Select(x => new
            {
                rect = x.GetComponent<RectTransform>(),
                animator = x.GetComponent<EaseInOutAnimator>()
            })
            .ToList();
        _storyEntryRects = tiles.Select(x => x.rect).ToList();

        for (var i = 0; i < tiles.Count; i++)
        {
            tiles[i].animator.ShowPanel(i * 0.05f);
        }
        
        lineRenderer.positionCount = _storyEntryRects.Count;
    }

    private void Update()
    {
        var offset = new Vector3(0, 0, -10);
        for (var i = 0; i < _storyEntryRects.Count; i++)
        {
            var rectTransform = _storyEntryRects[i];

            var worldPosition = rectTransform.TransformPoint(rectTransform.rect.center) + offset;
            lineRenderer.SetPosition(i, worldPosition); 
        }
    }
}
