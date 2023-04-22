using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    //������ ��Ÿ���� Enum
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
        Count
    }

    public static Direction GetDirectionByVector(Vector2 directionVector)
    {
        return Direction.Up;
    }

    //���⿡ �´� �̵� ����2 ���� ���� ��ųʸ�
    public static Dictionary<Direction, Vector2> directionVectors = new Dictionary<Direction, Vector2> 
    {
        { Direction.Up, Vector2.up },
        { Direction.Right, Vector2.right },
        { Direction.Down, Vector2.down },
        { Direction.Left, Vector2.left }
    };

    //�� ����
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

    //�����ϰ� min�� max ������ count���� ���� ���� �Լ� ����.
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
