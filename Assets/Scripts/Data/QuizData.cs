using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class QuizData
{
	[XmlAttribute]
	public int ID;
	[XmlAttribute]
	public string kor;
    [XmlAttribute]
    public int Difficulty;
    [XmlAttribute]
    public string quizType;

	[XmlAttribute]
	public string eng;

}

[Serializable, XmlRoot("ArrayOfQuizData")]
public class QuizDataLoader : ILoader<int, QuizData>
{
	[XmlElement("QuizData")]
	public List<QuizData> _quizData = new List<QuizData>();

	public Dictionary<int, QuizData> MakeDic()
	{
		Dictionary<int, QuizData> dic = new Dictionary<int, QuizData>();

		foreach (QuizData data in _quizData)
			dic.Add(data.ID, data);

		return dic;
	}

	public bool Validate()
	{
		return true;
	}
}