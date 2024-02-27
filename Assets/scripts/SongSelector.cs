using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SongSelector : MonoBehaviour
{
    public Dropdown folderDropdown;
    public Dropdown songDropdown;
    public AudioSource audioSource;
    public string path = string.Empty;
    public ScreenManager screenManager;
    public SongManager songManager;
    private string folderPath = "Assets/Audio";


    private string selectedFolder;

    void Start()
    {
        // Check if the folder exists
        if (Directory.Exists(folderPath))
        {
            // Get all subdirectories (folders) inside the target folder
            string[] subDirectories = Directory.GetDirectories(folderPath);

            // Log information about each subdirectory
            foreach (string subDirectory in subDirectories)
            {
                // Get the name of the subdirectory
                string folderName = Path.GetFileName(subDirectory);

                // Log the folder information
                Debug.Log("Folder Name: " + folderName);
                Debug.Log("Full Path: " + subDirectory);
            }
        }
        else
        {
            // Log an error if the folder doesn't exist
            Debug.LogError("The folder '" + folderPath + "' does not exist.");
        }
    }

    
    public void OnFolderSelectionChanged()
    {
        // Update selected folder and populate song dropdown with songs from the selected folder
        selectedFolder = folderDropdown.options[folderDropdown.value].text;
        string[] songs = Directory.GetFiles(selectedFolder, "*.mp3");
        songDropdown.ClearOptions();
        songDropdown.AddOptions(new List<string>(songs));
    }

    public void OnSongSelectionChanged()
    {
        // Load and play the selected song
        string selectedSong = songDropdown.options[songDropdown.value].text;
        AudioClip audioClip = Resources.Load<AudioClip>(selectedSong.Replace("Assets/Resources/", "").Replace(".mp3", ""));
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}