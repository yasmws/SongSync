using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;


public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public float songDelayInSeconds;
    public int inputDelayInMilliSeconds;
    public double marginOfError;

    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;

    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }
    public static MidiFile midiFile;

    private void Start()
    {
        Instance = this;

        if(Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            ReadFromWebsite();
        }
        else
        {
            ReadFromFile();
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    private void ReadFromWebsite()
    {
        throw new NotImplementedException();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Note[notes.Count];
        notes.CopyTo(array, 0);
        Invoke(nameof(StartSong), songDelayInSeconds);

    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    public void StartSong()
    {
        audioSource.Play();
    }
}
