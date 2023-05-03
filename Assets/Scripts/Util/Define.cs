using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int BOSS_INDEX = 100;
    public const int SHOP_INDEX = 200;
    public const int EVENT_INDEX = 300;
    public const int REST_INDEX = 400;
    public const int NEGO_INDEX = 1000;
    public const int FIGHT_INDEX = 2000;
    public const int NEGOFAIL_INDEX = 3000;
    public const int BOSS_AFTER_INDEX = 4000;
    public const int BOSS_NEGO_INDEX = 5000;

    public const int MOB_TYPES = 3;

    //방향을 나타내는 Enum
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Count
    }

    //방향에 맞는 이동 벡터2 값을 담은 딕셔너리
    public static Dictionary<Direction, Vector2> directionVectors = new Dictionary<Direction, Vector2> 
    {
        { Direction.Up, Vector2.up },
        { Direction.Right, Vector2.right },
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left }
    };

    //방 종류
    public enum EventType
    {
        Start,
        Shop,
        Rest,
        Event,
        Enemy,
        Boss,
        Count
    }

    public enum ThemeType
    {
        None,
        Pirate,
        Druids,
        Saintess,
        Mecha,
        Final
    }

    //랜덤하게 min과 max 사이의 count개의 수를 고르는 함수 생성.
    public static List<int> GenerateRandomNumbers(int min, int max, int count)
    {
        if (count == 0) return null;

        List<int> randomNumbers = new List<int>(count);

        for (int i = 0; i < count;)
        {
            int number = Random.Range(min, max);

            if (!randomNumbers.Contains(number))
            {
                randomNumbers.Add(number);
                i++;
            }
        }
        return randomNumbers;
    }
}
