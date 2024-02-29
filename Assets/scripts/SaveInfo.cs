using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveInfo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button listItem = this.GetComponent<Button>();

        var name = this.gameObject.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        var author = this.gameObject.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();



        listItem.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("song-folder", name.text);
            PlayerPrefs.Save();
            SceneManager.LoadScene("SongScene");

        });

    }

    // Update is called once per frame
    void Update()
    {

    }
}
