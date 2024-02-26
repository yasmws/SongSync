using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Note : MonoBehaviour
{
    double timeInstantiated;
    public float duration;
    public float assignedTime;

    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = SongManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void Update()
    {
        double timeSinceInstantiated = SongManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
        float longNoteHeight = gameObject.transform.GetChild(0).localScale.y;
        
        if (timeSinceInstantiated > duration && (transform.localPosition.y + longNoteHeight) < SongManager.Instance.noteDespawnY)
        {
            
            Destroy(gameObject);
        }
        else
        {
            var notePosition = transform.localPosition;
            notePosition.y = SongManager.Instance.noteSpawnY + (SongManager.Instance.noteDespawnY - SongManager.Instance.noteSpawnY) * t;
            transform.localPosition = notePosition;
        }
    }
}
