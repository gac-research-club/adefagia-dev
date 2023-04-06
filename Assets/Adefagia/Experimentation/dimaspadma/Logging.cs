using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Logging : MonoBehaviour
{
    private const string Path = "Assets/Adefagia/Resources/";
    
    public static void LogStep(string step)
    {
        var path = Path + "Log.txt";
        
        if (File.Exists(path))
        {
            path = IncrementLogName(0);
        }
        
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(step);
        }	
    }

    private string IncrementLogName(int count)
    {
        
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
            Debug.LogWarning("File Map not found", this);
        }
        catch (DirectoryNotFoundException)
        {
            Debug.LogWarning("File Map not found", this);
        }
        

        // Serialize string
        // Debug.Log(map.Length);
        // return CleanInput(text);
        return text;
    }
}
