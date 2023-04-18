using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logging
{
    private static string _path;

    public Logging()
    {
        DateTime currentTime = DateTime.Now;
        // Debug.Log(currentTime);

        // Create Logs directory
        Directory.CreateDirectory(Path.Combine(Application.dataPath, "Logs"));
        
        // Write log file
        _path = Path.Combine(Application.dataPath, "Logs", currentTime.ToString("ddMMyyyy-hhmmss") + ".txt");
    }
    
    public void LogStep(string step)
    {
        using (StreamWriter sw = File.AppendText(_path))
        {
            sw.WriteLine(step);
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
