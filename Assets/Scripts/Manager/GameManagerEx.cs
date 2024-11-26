using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;


[Serializable]
public class PlayerState
    {
        public Emotion state = Emotion.None;
        public bool dialogueEvent = false;
        public bool goHomeEvent = false;
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
        public string Name;
        public int Hp;
        public int MaxHp;
        // public int Mp;
        // public int MaxMp;
        public int LuckPercent;

        public int PassingSpeed;
        public int LimitTime; //캐릭터 guess 리미트 시간
        public int MaxLimitTime;
        public float SpendTime;

        //얻은 스킬
        public Skill[] Skills = new Skill[MAX_SKILL_COUNT];
        
        //데이터 저장 상태 (스탯용)
        public CollectionState[] Collections = new CollectionState[MAX_COLLECTION_COUNT];

        // 클리어 한 엔딩
	    public CollectionState[] Endings = new CollectionState[MAX_ENDING_COUNT];

    }

public class GameManagerEx
{
    GameData _gameData = new GameData();
    public GameData SaveData { get { return _gameData; } set { _gameData = value; } }

    #region 스탯
    public string Name
    {
        get { return _gameData.Name; }
        set { _gameData.Name = value; }
    }
    public int Hp
    {
        get { return _gameData.Hp; }
        set { _gameData.Hp = Mathf.Clamp(value, 0, MaxHp); }
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
    public int PassingSpeed
    {
        get { return _gameData.PassingSpeed;}
        set { _gameData.PassingSpeed= value; }
    }
    public int LimitTime
    {
        get { return _gameData.LimitTime;}
        set { _gameData.LimitTime = Mathf.Clamp(value, 0, MaxLimitTime);}
    }
    public int MaxLimitTime
    {
        get { return _gameData.MaxLimitTime;}
        set { _gameData.MaxLimitTime = value;}
    }

    #endregion

    #region 컬렉션 & 프로젝트


    #endregion
}

