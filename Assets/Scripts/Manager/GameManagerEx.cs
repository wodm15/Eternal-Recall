using System;
using System.Collections.Generic;
using static Define;
using UnityEngine;
using System.IO;



[Serializable]
public class PlayerState
    {
        public Emotion state = Emotion.None;
        public bool dialogueEvent = false;
        public bool goHomeEvent = false;
    }

[Serializable]
public enum StrangerIndex
{
    HairIndex,
    ClothesIndex,
    EyebrowIndex,
    EyeIndex,
    MouthIndex,
    EmotionIndex,
    AnimationIndex,
}

public enum CollectionState
    {
        None,
        Uncheck,
        Done
    }

[Serializable]
public class GameData
    {
        //캐릭터용 + 패시브 스킬
        public int Stage;
        public string Name;
        public int Hp;
        public int MaxHp;
        public int LuckPercent; // 아이템 획득 확률 증가
        public int Defence;
        public int ExpendTime;
        public int DownSpeed; // 지나가는 스피드 내리기
        public int GuessTimer; //추측 시간

        //스킬
        public int TheWorld; //3초간 멈추기
        public int PassTicket; //스테이지 1개 바로 패스

        //얻은 스킬
        public Skill[] Skills = new Skill[MAX_SKILL_COUNT];
        
        //데이터 저장 상태 (스탯용)
        public CollectionState[] Collections = new CollectionState[MAX_COLLECTION_COUNT];

        // 클리어 한 엔딩
	    public CollectionState[] Endings = new CollectionState[MAX_ENDING_COUNT];

        //그 전 질문
        public QuizData quizData = new QuizData();

    }

public class GameManagerEx
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }
    public int Stage
    {
        get { return _gameData.Stage;}
        set { _gameData.Stage = value; }
    }
    public int[] StrangerIndex = new int[7];

    #region 스탯
    public string Name
    {
        get { return _gameData.Name; }
        set { _gameData.Name = value; }
    }
    public int Hp
    {
        get { return _gameData.Hp; }
        set { _gameData.Hp = value; }
    }
    public int Defence
    {
        get {return _gameData.Defence;}
        set { _gameData.Defence = value;}
    }
    public int ExpendTime
    {
        get { return _gameData.ExpendTime;}
        set { _gameData.ExpendTime = value;}
    }
    public int MaxHp
    {
        get {return _gameData.MaxHp;}
        set { _gameData.MaxHp = value; }
    }

    public int LuckPercent
    {
        get { return _gameData.LuckPercent; }
        set { _gameData.LuckPercent = value; }
    }
    public int DownSpeed
    {
        get { return _gameData.DownSpeed;}
        set { _gameData.DownSpeed= value; }
    }
    public int GuessTimer
    {
        get{ return _gameData.GuessTimer;}
        set { _gameData.GuessTimer=value;}
    }


    //스킬
    public int TheWorld
    {
        get { return _gameData.TheWorld;}
        set { _gameData.TheWorld = value; }
    }
    public int PassTicket
    {
        get { return _gameData.PassTicket;}
        set { _gameData.PassTicket=value;}
    }

    #endregion

    public QuizData quizData
    {
        get { return _gameData.quizData; }
        set { _gameData.quizData = value; }
    }

    #region 컬렉션 & 프로젝트


    #endregion


public void Init()
{
    // 초기 세팅
    StartData data = Managers.Data.Start;

    Stage = data.Stage;
    Hp = data.maxHp;
    LuckPercent = data.LuckPercent;
    DownSpeed = data.DownSpeed;
    TheWorld = data.TheWorld;
    PassTicket = data.PassTicket;
    GuessTimer = data.GuessTimer;
    Defence = data.Defence;
 
}


    //랜덤 퀴즈 생성
    public QuizData LoadRandomQuiz()
    {
        QuizData randomQuiz = null;
        List<QuizData> filteredQuizzes = new List<QuizData>();

        foreach (QuizData quiz in Managers.Data.Quiz.Values)
        {
            if ((Stage - 1) / 10 + 1 == quiz.Difficulty)
            {
                filteredQuizzes.Add(quiz);
            }
        }

        if (filteredQuizzes.Count > 0)
        {
            randomQuiz = filteredQuizzes[UnityEngine.Random.Range(0, filteredQuizzes.Count)];

        }
        else
        {
            UnityEngine.Debug.LogWarning("조건에 맞는 퀴즈가 없습니다.");
        }

        return randomQuiz;
    }


    #region Save & Load	
	public string _path = Application.persistentDataPath + "/SaveData.json";

	public void SaveGame()
	{
		string jsonStr = JsonUtility.ToJson(Managers.Game.SaveData);
		File.WriteAllText(_path, jsonStr);
		Debug.Log($"Save Game Completed : {_path}");
	}

	public bool LoadGame()
	{
		if (File.Exists(_path) == false)
			return false;

		string fileStr = File.ReadAllText(_path);
		GameData data = JsonUtility.FromJson<GameData>(fileStr);
		if (data != null)
		{
			Managers.Game.SaveData = data;
		}

		Debug.Log($"Save Game Loaded : {_path}");
		return true;
	}

    public bool HasSavedData()
    {
        return File.Exists(Application.persistentDataPath + "/SaveData.json");
    }
    
	#endregion
}