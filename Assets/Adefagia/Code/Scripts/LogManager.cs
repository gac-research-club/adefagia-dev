using System;
using System.Collections;
using Adefagia.RobotSystem;
using System.Collections.Generic;
using System.IO;
using Adefagia;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogManager : MonoBehaviour {

    public int maxLog = 10;
    [SerializeField] private GameObject logPanel; 
    [SerializeField] private GameObject textObject;     
    private static string _path;
    private static List<Robot> _listRobot = new List<Robot>();

    private Dictionary<string, List<float>> _totalDamage = new Dictionary<string, List<float>>();

    private List<Log> logList = new List<Log>();
    
    public enum LogText
    {
        Info,
        Danger,
        Common,
        Warning
    }

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
        
        // Create Logs directory
        Directory.CreateDirectory(Path.Combine(Application.dataPath, "Logs"));
        
        // Write log file
        _path = Path.Combine(Application.dataPath, "Logs", currentTime.ToString("ddMMyyyy-hhmmss") + ".txt");
    }

    public Color32 GetColor32(LogText logText){
        if(logText == LogText.Danger){
            return new Color32(255,0,0,255);
        }else if(logText == LogText.Info){
            return new Color32(255,0,255,255);
        }else if(logText == LogText.Warning){
            return new Color32(255,255,0,255);
        }
        return new Color32(255,255,255,255);
    }
    
    public void LogStep(string text, LogText logText = LogText.Common)
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
        newTextComponent.color = GetColor32(logText);
        newLog.textObject = newTextComponent;

        logList.Add(newLog);

        using (StreamWriter sw = File.AppendText(_path))
        {
            sw.WriteLine(text);
        }	   
    }

    public void AddDeadRobot(Robot robot){
        Color32 redColor = new Color32(255, 0, 0, 255);
        LogStep($"{robot.Name} is Dead", LogText.Danger);
        _listRobot.Add(robot);
    }

    public void ObstacleDestroyed(){

    }

    public void DamageCalculation(string team, float damage){
        if (_totalDamage.ContainsKey(team)){
            
            _totalDamage[team].Add(damage);
        
        } else {
            List<float> newDamage = new List<float>();
            newDamage.Add(damage);

            _totalDamage.Add(team, newDamage);
        }
    }

    public Dictionary<string, List<float>> GetDamageCalculation(){
        return _totalDamage;
    }

    public List<Robot> GetListRobot(){
        return _listRobot;
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
        
        return text;
    }
}

[System.Serializable]
public class Log
{
    public string text;
    public TMP_Text textObject;
}


