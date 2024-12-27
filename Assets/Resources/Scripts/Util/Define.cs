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
	public const int DefaultCollectionIndex = 2;
	public const int MAX_COLLECTION_COUNT = 21;
	public const int ErrorCodeInt = -100;
	public const string ErrorCodeString = "Error";

	public const int GiveHintNormal = 5;
	public const int GiveHintHard = 3;
	public const int GiveHintUnLimted = 1;
	//현재 코디 옷 (19개 이므로)
	public const int MAX_STAT_COUNT = 50;
	public const int MAX_ENDING_COUNT = 50;

	//틀린 1개당 데미지
	public const int Damage = 10;
	//게임 종료 End
	public const int NormalGameEnd = 30;
	public const int HardGameEnd = 50;
	public const int UnLimitedGameEnd = 70;
	public const int NightmareGameEnd = 100;

	//뽑기 가격
	public const int PickupCost = 500;

	//픽업 옷 번호
	public const int sailer = 0;
	public const int nightWear = 10;
	public const int Maid = 4;
	public const int Bikini = 3;
	public const int magic = 17;
	public const int Swimsuit2 = 18;
	public const int weddingDress = 1;
	public const int Hanbok = 9;
	public const int Yukata = 7;
	public const int Princess = 13;

	//난이도 별 인덱스 범위
	public const int NormalIndex = 5;
	public const int HardIndex = 10;
	public const int MaxIndexEyebrow = 5;
	public const int MaxIndexEmotion = 6;
	public const int UnlimitedIndex = 20; //눈썹과 감정 제외
	public const int MaxAnimationIndex = 10;

	//움직임 제어
	public const float GuessPlayerSpeed = 5.0f;
	public const float BirdSpeed = 5.0f;


	//닉네임 입력
	public const int WriteNickname = 6500;	
	public const int NicknameConfirm = 6501;
	public const int GOBACKText = 6502;

	public const int CorrectText = 6503;
	public const int WrongText = 6504;
	public const int WrongButAvoid = 6505;
	public const int DifficultyNormal = 6506;
	public const int DifficultyHard = 6507;
	public const int DifficultyUnlimited = 6508;
	public const int SaveNothing = 6509;
	public const int SaveButEnd = 6510;
	public const int DeleteFile = 6511;
	public const int FetchFile = 6512;
	public const int GameOverText = 6513;
	public const int GameOverTitle = 6514;
	public const int ReviveText = 6515;
	public const int ReviveSayText = 6516;
	public const int WhenWrongText = 6517;
	public const int UnlockHardText = 6518;
	public const int UnlockUnlimitedText = 6519;
	public const int Pickup1 = 6520;
	public const int Pickup2 = 6521;
	public const int Pickup3 = 6522;
	public const int Pickup4 = 6523;
	public const int Pickup5 = 6524;
	public const int PickPickText = 6525;
	public const int NoMoneyText = 6526;
	public const int Pickup1Percent = 6527;
	public const int Pickup2Percent = 6528;
	public const int Pickup3Percent = 6529;
	public const int Pickup4Percent = 6530;
	public const int Pickup5Percent = 6531;
	public const int YesGet = 6532;
	public const int NoGet = 6533;
	public const int Pickup6 = 6534;
	public const int Pickup7 = 6535;
	public const int Pickup8 = 6536;
	public const int Pickup9 = 6537;
	public const int Pickup10 = 6538;
	public const int Pickup6Percent = 6539;
	public const int Pickup7Percent = 6540;
	public const int Pickup8Percent = 6541;
	public const int Pickup9Percent = 6542;
	public const int Pickup10Percent = 6543;
	public const int ToggleText1 = 6544;
	public const int ToggleText2 = 6545;
	public const int DifficultyNightmare = 6546;


	public const int StartButtonText = 10000;
	public const int ContinueButtonText = 10001;
	public const int CollectionButtonText = 10002;
	public const int QuitButtonText = 10003;
	public const int PickupText = 10004;
	public const int ShopButtonText = 10005;
	public const int SkillChoseText = 100;
	public const int Intro1 = 20019;
	public const int Intro2 = 20020;
	public const int Intro3 = 20021;
	public const int DataResetConfirm = 20022;
	public const int CollectionTitle = 20023;
	public const int ConfirmButtonText = 20024;
	public const int RemindButtonText = 20025;
	public const int PurchaseText1 = 20026;
	public const int PurchaseText2 = 20027;
	public const int PurchaseText3 = 20028;
	public const int PurchaseText4 = 20029;
	public const int PurchaseText5 = 20030;
	public const int PurchaseText6 = 20031;
	public const int PurchaseText = 20032;


	// public const int HairQuestion = 30000;
	// public const int ClothesQuestion= 30001;
	// public const int EyebrowQuestion = 30002;
	// public const int EyeQuestion = 30003;
	// public const int MouthQuestion = 30004;
	// public const int AnimationQuestion = 30005;
    public static int[] CharacterSaying = new int[51]
    {
        30000, 30001, 30002, 30003, 30004, 30005, 30006, 30007, 30008, 30009,
        30010, 30011, 30012, 30013, 30014, 30015, 30016, 30017, 30018, 30019,
        30020, 30021, 30022, 30023, 30024, 30025, 30026, 30027, 30028, 30029,
        30030, 30031, 30032, 30033, 30034, 30035, 30036, 30037, 30038, 30039,
        30040, 30041, 30042, 30043, 30044, 30045, 30046, 30047, 30048, 30049,
		30050
    };

	
	public const int NormalGameEndText = 40000;
	public const int HardGameEndText = 40001;
	public const int UnLimitedGameEndText = 40002;
	public const int NightmareGameEndText = 40003;

	public const int GoToTitleText = 50001;

	public const int CollectionPageTitle = 50002;
	public const int CollectionPageTab1 = 50003;
	public const int CollectionPageTab2 = 50004;
	public const int AvoidButtonText = 50005;
	public const int AvoidSucessText = 50006;
	public const int AvoidFailText = 50007;
	public const int GuessPlayerText = 50008;
	public const int AnswerPlayerText = 50009;
	public const int WarningInputText = 50010;
	public const int LuckSkillText = 50011;
	public const int AlreadyGet = 50012;
	


}
