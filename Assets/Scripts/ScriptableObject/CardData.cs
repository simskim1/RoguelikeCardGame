// CardData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "ScriptableObjects/CardData", order = 1)]
public class CardData : ScriptableObject
{
    [Header("기본 정보")]
    public string cardName;         // 카드 이름
    [TextArea]
    public string description;      // 카드 설명 
    public Sprite cardImage;        // 카드 일러스트
    public int energyCost;          // 소모 에너지

    [Header("분류 정보")]
    public CardType cardType;
    public CardRarity rarity;
    public TargetType targetType;

    [Header("효과 수치")]
    public int damage;              // 공격력
    public int block;               // 방어력

}

/*
이미지 저작자 링크
<a href="https://www.flaticon.com/kr/free-icons/" title="공격 아이콘">공격 아이콘 제작자: Hilmy Abiyyu A. - Flaticon</a>--01_Attack
<a href="https://www.flaticon.com/kr/free-icons/" title="방패 아이콘">방패 아이콘 제작자: kliwir art - Flaticon</a>--02_Guard
*/