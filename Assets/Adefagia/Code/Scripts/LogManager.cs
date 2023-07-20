using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Adefagia;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {

    public int maxLog = 10;
    [SerializeField] private GameObject logPanel; 
    [SerializeField] private GameObject textObject; 
    private static string _path;
    
    List<Log> logList = new List<Log>();

    private void Awake()
    {
        // Set into gameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.logManager = this;
        }
    }

    public void Start()
    {
        DateTime currentTime = DateTime.Now;
        Debug.Log(logPanel);
        Debug.Log(textObject);

        // Create Logs directory
        Directory.CreateDirectory(Path.Combine(Application.dataPath, "Logs"));
        
        // Write log file
        _path = Path.Combine(Application.dataPath, "Logs", currentTime.ToString("ddMMyyyy-hhmmss") + ".txt");
    }

    
    public void LogStep(string text)
    { 
        if(logList.Count > maxLog){
            Destroy(logList[0].textObject.gameObject);
            logList.Remove(logList[0]);
        }
            
        Log newLog = new Log();
        
        newLog.text = text;
        
        GameObject newText = Instantiate(textObject, logPanel.transform.position, Quaternion.identity, logPanel.transform);
        
        TMP_Text newTextComponent = newText.GetComponent<TMP_Text>();
        newTextComponent.text = newLog.text;
        newLog.textObject = newTextComponent;

        // newLog.textObject.text = ;

        logList.Add(newLog);

        using (StreamWriter sw = File.AppendText(_path))
        {
            sw.WriteLine(text);
        }	   
    }

    private string ReadFile(string pathFile)
    {
        var text = "";

        try
        {
            // read from file
            StreamReader reader = new StreamReader(pathFile);
            text = reader.ReadToEnd();

        }
        catch (FileNotFoundException)
        {
            Debug.LogWarning("File Map not found");
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogWarning("File Map not found"); 
        }
        

        // Serialize string
        // Debug.Log(map.Length);
        // return CleanInput(text);
        return text;
    }
}

[System.Serializable]
public class Log
{
    public string text;
    public TMP_Text textObject;
}


