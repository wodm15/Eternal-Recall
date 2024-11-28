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
        //캐릭터용 + 패시브 스킬
        public string Name;
        public int Hp;
        public int MaxHp;
        public int LuckPercent; // 아이템 획득 확률 증가
        public int DownSpeed; // 지나가는 스피드 내리기

        //스킬
        public int TheWorld; //3초간 멈추기
        public int PassTicket; //스테이지 1개 바로 패스

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
    public int DownSpeed
    {
        get { return _gameData.DownSpeed;}
        set { _gameData.DownSpeed= value; }
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

    #region 컬렉션 & 프로젝트


    #endregion


    public void Init()
    {
        //초기 세팅
		StartData data = Managers.Data.Start;
    	Hp = data.maxHp;
        LuckPercent = data.LuckPercent;
        DownSpeed = data.DownSpeed;
        TheWorld = data.TheWorld;
        PassTicket = data.PassTicket;

        // LuckPercent;


        // DownSpeed= data.DownSpeed; // 지나가는 스피드 내리기


        // TheWorld; //3초간 멈추기


        // public int PassTicket; //스테이지 1개 바로 패스

        // ReApplyCollectionStats(); //컬렉션 스테이터스 추가
    
    }
}