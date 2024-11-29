using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class StartData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public int maxHp;
	[XmlAttribute]
	public int LuckPercent;

	[XmlAttribute]
    public int DownSpeed; // 지나가는 스피드 내리기

	[XmlAttribute]
    public int TheWorld; //3초간 멈추기

	[XmlAttribute]
    public int PassTicket; //스테이지 1개 바로 패스

	[XmlAttribute]
	public int Stage;



}