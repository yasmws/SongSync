using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public int noteRestriction;
    public SongManager songManager;
    public KeyCode input;
    public GameObject notePrefab;
    List<Note> notes = new List<Note>();
    public Color laneColor;
    public List<NoteData> timeStamps = new List<NoteData>();
    private SpriteRenderer hitboxRenderer;


    int spawnIndex = 0;
    int inputIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hitboxRenderer = transform.Find("Hitbox").GetComponent<SpriteRenderer>();
        hitboxRenderer.color = laneColor;
    }
    private List<Melanchall.DryWetMidi.Interaction.Note> RemoveOverlappingNotes(ICollection<Melanchall.DryWetMidi.Interaction.Note> notes)
    {
        var allNotesArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(allNotesArray, 0);
        var filteredNotesArray = new List<Melanchall.DryWetMidi.Interaction.Note>();

        foreach(var note in allNotesArray)
        {
            if (note.NoteNumber == noteRestriction)
            {
                if (filteredNotesArray.Count > 0 && filteredNotesArray[^1].Time < note.Time && filteredNotesArray[^1].EndTime > note.Time) continue;
                else filteredNotesArray.Add(note);
            }
        }

        return filteredNotesArray;
    }
    public void SetTimeStamps(ICollection<Melanchall.DryWetMidi.Interaction.Note> notes)
    {
        var notesArray = RemoveOverlappingNotes(notes);

        foreach (var note in notesArray)
        {
            if (note.NoteNumber == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                var metricTimeSpanDuration = TimeConverter.ConvertTo<MetricTimeSpan>(note.Length, SongManager.midiFile.GetTempoMap());
                var duration = (double)metricTimeSpanDuration.Minutes * 60f + metricTimeSpanDuration.Seconds + (double)metricTimeSpanDuration.Milliseconds / 1000f;
                timeStamps.Add(new NoteData((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f, duration));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(input))
        {
            hitboxRenderer.color = Color.black;
        }

        if (Input.GetKeyUp(input))
        {
            hitboxRenderer.color = laneColor;
        }


        if (spawnIndex < timeStamps.Count)
        {

            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex].timestamp - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex].timestamp;
                note.GetComponent<Note>().duration = (float)timeStamps[spawnIndex].duration;
                Vector3 notePosition = note.transform.localPosition;
                notePosition.y = SongManager.Instance.noteSpawnY;
                note.transform.localPosition = notePosition;

                var childTransform = note.transform.GetChild(0);
                Vector3 scale = childTransform.localScale;
                scale.y = ((float) timeStamps[spawnIndex].duration * SongManager.Instance.noteDistanceToTap) / SongManager.Instance.noteTime;
                childTransform.localScale = scale;

                float halfHeight = scale.y / 2.0f;
                Vector3 newPosition = childTransform.localPosition;
                newPosition.y = halfHeight;
                childTransform.localPosition = newPosition;

                note.GetComponent<SpriteRenderer>().color = laneColor;
                childTransform.GetComponent<SpriteRenderer>().color = laneColor;

                //Debug.Log("on spawn: " + note.transform.localPosition);
                note.GetComponent<SpriteRenderer>().enabled = true;
                childTransform.GetComponent<SpriteRenderer>().enabled = true;

                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex].timestamp;
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                //print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }

    }
    private void Hit()
    {
        ScoreManager.Hit();
    }
    private void Miss()
    {
        ScoreManager.Miss();
    }
}
