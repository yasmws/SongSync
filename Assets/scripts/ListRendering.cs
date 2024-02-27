using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ListRendering : MonoBehaviour
{
    public GameObject CellPrefab;
    public string folderPath = "Assets/Audio";
    // Start is called before the first frame update
    void Start()
    {
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
                GameObject obj = Instantiate(CellPrefab);
                obj.name = folderName;
                obj.transform.SetParent(this.gameObject.transform, false);
   
                TextMeshProUGUI textMeshPro = obj.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();

                // Check if the TextMeshProUGUI component is present
                if (textMeshPro != null)
                {
                    textMeshPro.text = folderName;
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component not found on child.");
                }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
