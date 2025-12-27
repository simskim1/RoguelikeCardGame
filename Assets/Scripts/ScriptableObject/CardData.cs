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
<a href="https://www.flaticon.com/kr/free-icons/" title="전기 아이콘">전기 아이콘 제작자: Freepik - Flaticon</a>--Energy

<a href="https://www.flaticon.com/kr/free-icons/" title="유령 아이콘">유령 아이콘 제작자: Good Ware - Flaticon</a>--Ghost

<a href="https://www.flaticon.com/kr/free-icons/" title="공격 아이콘">공격 아이콘 제작자: Hilmy Abiyyu A. - Flaticon</a>--01_Attack
<a href="https://www.flaticon.com/kr/free-icons/" title="방패 아이콘">방패 아이콘 제작자: kliwir art - Flaticon</a>--02_Guard
<a href="https://www.flaticon.com/kr/free-icons/" title="카타나 아이콘">카타나 아이콘 제작자: Jesus Chavarria - Flaticon</a>--03_StrongAttack
*/