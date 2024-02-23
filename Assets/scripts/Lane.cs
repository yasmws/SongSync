using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections;
using System.Collections.Generic;
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
    public List<double> timeStamps = new List<double>();
    private SpriteRenderer hitboxRenderer;


    int spawnIndex = 0;
    int inputIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        hitboxRenderer = transform.Find("Hitbox").GetComponent<SpriteRenderer>();
        hitboxRenderer.color = laneColor;
    }
    public void SetTimeStamps(ICollection<Melanchall.DryWetMidi.Interaction.Note> notes)
    {
        var notesArray = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(notesArray, 0);
        foreach (var note in notesArray)
        {
            
            if (note.NoteNumber == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
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

            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                note.GetComponent<SpriteRenderer>().color = laneColor;
               spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
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
