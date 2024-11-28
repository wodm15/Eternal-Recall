using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

    public enum Scene
	{
		Unknown,
		Lobby,
		Game,
	}

    public enum Sound
	{
		Bgm,
		Effect,
		Speech,
		Max,
	}

	public enum Emotion
	{
		None,
		Idle,
		Cry,
		Red,
		Black,
		Angry,
		Sweat,
	}
	public enum Skill
	{
		None,
	}
	public enum Shop
	{
		None,
		Healing, // 20퍼 힐링
		gambleHealing, // 10~ 30퍼 힐링
		GetExpendTime, //시간 늘리기 스킬 1개 30퍼 획득
		GetTheWorld, //캐릭터 움직임 느리게 스킬 30퍼 획득
		FreePass, //한번 살아남기 10퍼 획득
		upLuck, //행운 올리기(확률 10퍼씩 증가)
	}

	public const int MAX_COLLECTION_COUNT = 100;
	public const int MAX_SKILL_COUNT = 50;
	public const int MAX_ENDING_COUNT = 50;


	//닉네임 입력
	public const int WriteNickname = 6500;
	
	public const int StartButtonText = 10000;
	public const int ContinueButtonText = 10001;
	public const int CollectionButtonText = 10002;
	public const int Intro1 = 20019;
	public const int Intro2 = 20020;
	public const int Intro3 = 20021;
}
