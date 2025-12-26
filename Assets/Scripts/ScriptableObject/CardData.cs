// CardData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    [Header("기본 정보")]
    public string cardName;         
    [TextArea]
    public string description;       
    public Sprite cardImage;        
    public int energyCost;          

    [Header("분류 정보")]
    public CardType cardType;
    public CardRarity rarity;
    public TargetType targetType;

    [Header("효과 수치")]
    public int value;

}

/*
이미지 저작자 링크
<a href="https://www.flaticon.com/kr/free-icons/" title="공격 아이콘">공격 아이콘 제작자: Hilmy Abiyyu A. - Flaticon</a>--01_Attack
<a href="https://www.flaticon.com/kr/free-icons/" title="방패 아이콘">방패 아이콘 제작자: kliwir art - Flaticon</a>--02_Guard
*/