using System;
using System.Xml.Linq;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ValidFileManager
{
    public string _path = Application.persistentDataPath + "/SaveData.json";
    
    public void Init()
    {
    }

    public int CheckJson()
    {
        if (!File.Exists(_path))
        {
            Debug.Log("파일이 존재하지 않습니다.");
            return 0;
        }

        string fileStr = File.ReadAllText(_path);
        GameData data = null;

        try
        {
            data = JsonUtility.FromJson<GameData>(fileStr); // JSON 데이터 파싱
        }
        catch (ArgumentException ex)
        {
            Debug.LogError($"JSON 파싱 오류: {ex.Message}");
            DeleteFile(); // 파싱 오류가 나면 파일 삭제
            return -1;
        }

        if (data == null) 
        {
            Debug.Log("JSON 데이터 변환 실패. 파일이 잘못되었습니다.");
            DeleteFile();
            return -1;
        }

        string xmlData = ConvertJsonToXml(data); 

        if (xmlData.Contains("Invalid JSON data")) // 데이터값 이상하면 바로 삭제
        {
            Debug.Log("XML 변환 중 오류 발생. 잘못된 JSON 데이터입니다.");
            DeleteFile();
            return -1;
        }

        // Debug.Log($"XML Data: {xmlData}");

        int validationResult = ValidateAndUpdateXmlAttributes(ref xmlData);
        if (validationResult == -1)
        {
            Debug.Log("XML 이상 발견 ");
            return -1;
        }
        else
        {
            Debug.Log("XML 이상 없음");
            return 0;
        }
    }

public int ValidateAndUpdateXmlAttributes(ref string xmlData)
{
    try
    {
        XElement root = XElement.Parse(xmlData);

        string[] expectedAttributes = new[]
        {
            "ID", "DifficultyLevel", "Hp", "maxHp", "LuckPercent", "ExpendTime",
            "DownSpeed", "TheWorld", "PassTicket", "Stage", "GuessTimer",
            "Defence", "HintKey", "Avoid", "ClothesIndex", "BGM", "BirdPercent" ,
            "Revive" , "ReviveLife" , "Unlocked"
        };

        var actualAttributes = root.Attributes().Select(attr => attr.Name.LocalName).ToList();
        // Debug.Log("실제 속성: " + string.Join(", ", actualAttributes));

        var missingAttributes = expectedAttributes.Except(actualAttributes).ToList();
    
        var extraAttributes = actualAttributes.Except(expectedAttributes).ToList();

        // 누락된 속성 확인 및 처리
        if (missingAttributes.Any())
        {
            Debug.Log("누락된 속성: " + string.Join(", ", missingAttributes));
            DeleteFile();
            return -1; 
        }

        // 불필요한 속성 확인 및 처리
        if (extraAttributes.Any())
        {
            Debug.Log("불필요한 속성: " + string.Join(", ", extraAttributes));
            DeleteFile();  
            return -1;  
        }

        xmlData = root.ToString();
        return 0; 
    }
    catch (Exception ex)
    {
        Debug.Log($"오류 발생: {ex.Message}");
        DeleteFile(); 
        return -1;
    }
}



private string ConvertJsonToXml(GameData data)
{
    try
    {
        XElement root = new XElement("GameData",
            new XAttribute("ID", data.StatData.ID),
            new XAttribute("DifficultyLevel", data.DifficultyLevel),
            new XAttribute("Hp", data.Hp),
            new XAttribute("maxHp", data.MaxHp),
            new XAttribute("LuckPercent", data.LuckPercent),
            new XAttribute("ExpendTime", data.ExpendTime),
            new XAttribute("DownSpeed", data.DownSpeed),
            new XAttribute("TheWorld", data.TheWorld),
            new XAttribute("PassTicket", data.PassTicket),
            new XAttribute("Stage", data.Stage),
            new XAttribute("GuessTimer", data.GuessTimer),
            new XAttribute("Defence", data.Defence),
            new XAttribute("HintKey", data.HintKey),
            new XAttribute("Avoid", data.Avoid),
            new XAttribute("ClothesIndex", data.ClothesIndex),
            new XAttribute("BGM", data.BGM),
            new XAttribute("BirdPercent", data.BirdPercent),
            new XAttribute("Revive", data.Revive),
            new XAttribute("ReviveLife", data.ReviveLife),
            new XAttribute("Unlocked", data.Unlocked)
        );

        return root.ToString(); // XML 반환
    }
    catch (Exception ex)
    {
        Debug.LogError($"JSON -> XML 변환 중 오류 발생: {ex.Message}");

        // 반환할 빈 XML 문자열을 정의하고 반환
        XElement errorRoot = new XElement("GameData", new XAttribute("Error", "Invalid JSON data"));
        return errorRoot.ToString();
    }
}


    public void DeleteFile()
    {
        try
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
                Debug.Log("파일이 삭제되었습니다.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("파일 삭제 중 오류 발생: " + ex.Message);
        }
    }
}