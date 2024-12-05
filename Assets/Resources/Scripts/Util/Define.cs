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

	public const int MAX_COLLECTION_COUNT = 50;

	//현재 코디 옷 (19개 이므로)
	public const int MAX_STAT_COUNT = 50;
	public const int MAX_ENDING_COUNT = 50;

	//틀린 1개당 데미지
	public const int Damage = 10;

	//닉네임 입력
	public const int WriteNickname = 6500;	
	public const int NicknameConfirm = 6501;
	public const int GOBACKText = 6502;

	public const int CorrectText = 6503;
	public const int WrongText = 6504;
	public const int WrongButAvoid = 6505;
	public const int StartButtonText = 10000;
	public const int ContinueButtonText = 10001;
	public const int CollectionButtonText = 10002;
	public const int SkillChoseText = 100;
	public const int Intro1 = 20019;
	public const int Intro2 = 20020;
	public const int Intro3 = 20021;
	public const int DataResetConfirm = 20022;

	public const int HairQuestion = 30000;
	public const int ClothesQuestion= 30001;
	public const int EyebrowQuestion = 30002;
	public const int EyeQuestion = 30003;
	public const int MouthQuestion = 30004;
	public const int AnimationQuestion = 30005;

	public const int GameEndText = 50000;
	public const int GoToTitleText = 50001;

	public const int CollectionPageTitle = 50002;
	public const int CollectionPageTab1 = 50003;
	public const int CollectionPageTab2 = 50004;
	public const int AvoidButtonText = 50005;
	public const int AvoidSucessText = 50006;
	public const int AvoidFailText = 50007;

}
