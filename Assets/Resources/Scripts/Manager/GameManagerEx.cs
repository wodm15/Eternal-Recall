using System;
using System.Collections.Generic;
using static Define;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;



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
        public string DifficultyLevel;
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
        public int Avoid;

        //스킬
        public int TheWorld; //3초간 멈추기
        public int PassTicket; //스테이지 1개 바로 패스
        public int HintKey;
        
        //컬렉션 +돈
        public int Money;
        public CollectionState[] Collections = new CollectionState[MAX_COLLECTION_COUNT];
        //난이도 해금
        public CollectionState[] Unlocked = new CollectionState[2];
        //코디
        public int ClothesIndex;
        public StatData StatData = new StatData();
        // // 클리어 한 엔딩
	    // public CollectionState[] Endings = new CollectionState[MAX_ENDING_COUNT];


        //그 전 질문
        public QuizData quizData = new QuizData();
        

        //현재 브금 노래
        public string BGM;

        //장애물 확률 
        public int BirdPercent;

        //광고보고 살아남기
        public int Revive;
        //방금 살아남기 한지 확인
        public bool ReviveLife;

        //배경화면
        public string BG;

    }

public class GameManagerEx
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }
    //난이도
    public string DifficultyLevel
    {
        get { return _gameData.DifficultyLevel;}
        set { _gameData.DifficultyLevel = value; DifficultySetting(DifficultyLevel);}
    }

    public int Stage
    {
        get { return _gameData.Stage;}
        set { _gameData.Stage = value; RefreshStatCollections(); }
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
        set { _gameData.Hp = value; RefreshStatCollections(); }
    }
    public int Defence
    {
        get {return _gameData.Defence;}
        set { _gameData.Defence = value;}
    }
    public int ExpendTime
    {
        get { return _gameData.ExpendTime;}
        set { _gameData.ExpendTime = value; }
    }
    public int MaxHp
    {
        get {return _gameData.MaxHp;}
        set { _gameData.MaxHp = value;}
    }

    public int LuckPercent
    {
        get { return _gameData.LuckPercent; }
        set { _gameData.LuckPercent = value;}
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

    public int Avoid
    {
        get {return _gameData.Avoid;}
        set {_gameData.Avoid=value; RefreshStatCollections();}
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
        set { _gameData.PassTicket=value; }
    }
    public int HintKey
    {
        get {return _gameData.HintKey;}
        set { _gameData.HintKey=value; RefreshStatCollections(); }
    }

    //코디
    public int ClothesIndex
    {
        get { return _gameData.ClothesIndex;}
        set { _gameData.ClothesIndex=value; }
    }

    #endregion

    public QuizData quizData
    {
        get { return _gameData.quizData; }
        set { _gameData.quizData = value; }
    }

    public string BGM
    {
        get { return _gameData.BGM;}
        set { _gameData.BGM=value;}
    }
    public int BirdPercent
    {
        get { return _gameData.BirdPercent;}
        set { _gameData.BirdPercent=value;}
    }
    public int Revive
    {
        get {return _gameData.Revive;}
        set { _gameData.Revive=value;} 
    }

    public bool ReviveLife 
    { 
        get { return _gameData.ReviveLife; } 
        set { _gameData.ReviveLife = value; } 
    }

    public string BG
    {
        get { return _gameData.BG;}
        set { _gameData.BG=value;}
    }

    #region 컬렉션 & 프로젝트

    public CollectionState[] Collections { get { return _gameData.Collections; } }
    public CollectionState[] Unlocked { get { return _gameData.Unlocked; } }
    public int Money { get { return _gameData.Money; } set { _gameData.Money = value; CheckMoney(); }  }
    public Action<CollectionData> OnNewCollection;

    public StatData StatData 
    {
         get { return _gameData.StatData; }
         set { _gameData.StatData = value; }
    }

    public void CheckMoney()
    {
        if(Money > 1000000)
            Money = 999999;
    }

    //실시간으로 업적 확인
    public void RefreshStatCollections()
        {

            foreach (CollectionData data in Managers.Data.CodyCollections)
            {
                
                if (Collections[data.ID ] != CollectionState.None)
				continue;

                if(data.purchase == 1)
                    continue;

                if(data.reqLevel != DifficultyLevel[..1])
                    continue;

                if (data.reqHp > Hp)
                    continue;
                if(data.reqStage > Stage)
                    continue;
                if(data.reqHintKey > HintKey)
                    continue;
    
                //옷얻는거 기록
                Collections[data.ID] = CollectionState.Done;
                Debug.Log("옷 기록");

                OnNewCollection?.Invoke(data);
            }
        }
	


    #endregion

public void Init()
{
    // 초기 세팅
    StartData data = Managers.Data.Start;

    DifficultyLevel = data.DifficultyLevel;
    Stage = data.Stage;
    Hp = data.maxHp;
    LuckPercent = data.LuckPercent;
    ExpendTime = data.ExpendTime;
    DownSpeed = data.DownSpeed;
    TheWorld = data.TheWorld;
    PassTicket = data.PassTicket;
    GuessTimer = data.GuessTimer;
    Defence = data.Defence;
    HintKey = data.HintKey;
    Avoid = data.Avoid;
    ClothesIndex = data.ClothesIndex;
    BGM = data.BGM;
    BirdPercent = data.BirdPercent;
    Revive = data.Revive;
    ReviveLife = false;
    BG = data.BG;

    // 기본 옷 상시 활성화
    if (Collections[Define.DefaultCollectionIndex] == CollectionState.None)
        Collections[Define.DefaultCollectionIndex] = CollectionState.Done;

}


    //랜덤 퀴즈 생성
    public QuizData LoadRandomQuiz()
    {
        QuizData randomQuiz = null;
        List<QuizData> filteredQuizzes = new List<QuizData>();

        foreach (QuizData quiz in Managers.Data.Quiz.Values)
        {
            // Difficulty 매핑을 Stage 값에 따라 설정
            int difficulty = 0;

            if (Stage >= 1 && Stage <= 2)
            {
                difficulty = 1;
            }
            else if (Stage >= 3 && Stage <= 9)
            {
                difficulty = 2;
            }
            else if (Stage >= 10 && Stage <= 19)
            {
                difficulty = 3;
            }
            else if (Stage >= 20 && Stage <= 24)
            {
                difficulty = 4;
            }
            else if (Stage >= 25 && Stage <= 30)
            {
                difficulty = 4;
            }

            //하드
            else if (Stage >= 31 && Stage <= 34)
            {
                difficulty = 3;
            }
            else if (Stage >= 35 && Stage <= 39)
            {
                difficulty = 4;
            }
            else if (Stage >= 40 && Stage <= 50)
            {
                difficulty = 5;
            }

            //언리미티드
            else if (Stage >= 50 && Stage <= 55)
            {
                difficulty = 4;
            }
            else if (Stage >= 55 && Stage <= 60)
            {
                difficulty = 5;
            }
            else if (Stage >= 55 && Stage <= 60)
            {
                difficulty = 6;
            }
            else if (Stage >= 60 && Stage <= 70)
            {
                difficulty = 7;
            }

            // difficulty에 맞는 퀴즈를 필터링
            if (quiz.Difficulty == difficulty)
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

    //난이도에 따른 설정
    public void DifficultySetting(string DifficultyLevel)
    {
        if(DifficultyLevel == "Normal")
        {
            Stage = 1;
            Avoid = 30;
            DownSpeed = 0;
            GuessTimer = 15;
            HintKey = 5;
        }
        if(DifficultyLevel == "Hard")
        {
            Stage = 30;
            Avoid = 10;
            DownSpeed = 0;
            GuessTimer = 15;
            HintKey = 3;
        }
        if(DifficultyLevel == "UnLimited")
        {
            Stage = 50;
            Avoid = 0;
            DownSpeed = 0;
            GuessTimer = 15;
            HintKey = 0;
        }

    }
    //배경 저장
    public void changeBG()
    {
        int BGIndex = Managers.Game.Stage / 10 + 1;
        BGIndex = Math.Min(BGIndex, 8); 
        Managers.Game.BG = $"{BGIndex}";
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

    //캐릭터 있을 경우 삭제 함수
	public void CharacterDelete()
    {
		// GameObject staticPlayer = GameObject.Find("StaticPlayer");
		// if (staticPlayer != null)
		// {
		// 	Managers.Resource.Destroy(staticPlayer);
		// }
		GameObject Stranger = GameObject.Find("Stranger");
		if (Stranger != null)
		{
			Managers.Resource.Destroy(Stranger);
		}
		GameObject Player = GameObject.Find("Player");
		if (Player != null)
		{
			Managers.Resource.Destroy(Player);
		}
	}
	public void StaticCharacterDelete()
    {
		GameObject staticPlayer = GameObject.Find("StaticPlayer");
		if (staticPlayer != null)
		{
			Managers.Resource.Destroy(staticPlayer);
		}
	}

    //광고
    public void InterstitialAd()
    {
        Managers.Ads.Init();
		Managers.Ads.ShowInterstitialAd();
    }
    public void RewardedAd()
    {
        Managers.Ads.Init();
		Managers.Ads.ShowRewardedAd();
    }
    

}