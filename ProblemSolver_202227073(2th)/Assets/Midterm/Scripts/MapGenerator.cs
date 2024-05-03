using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    public GameObject redWallPrefab;  // ���� �� ������Ʈ csv:1
    public GameObject greenSpacePrefab;  // ��� ���� ������Ʈ csv:0
    public GameObject highWallPrefab;  // ���� �� ������Ʈ csv:2
    public GameObject terrain;  // Terrain ��ü�� ���� ����

    void Start()
    {
        GenerateMapFromCSV("Assets/Midterm/Resources/map.csv");
    }

    void GenerateMapFromCSV(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int width = lines[0].Split(',').Length;
        int height = lines.Length;
        float spacing = 1.0f;  // ������Ʈ ���� ������ ����
        float startPosX = -15f;  // ���� ��ġ X
        float startPosZ = 15f;   // ���� ��ġ Z

        for (int y = 0; y < height; y++)
        {
            string[] line = lines[y].Split(',');
            for (int x = 0; x < width; x++)
            {
                GameObject toInstantiate;
                float posY;  // Y�� ��ġ�� ������ ����

                switch (line[x])
                {
                    case "1":
                        toInstantiate = redWallPrefab;
                        posY = 0.25f;  // ���� ���� �⺻ Y�� ��ġ
                        break;
                    case "2":
                        toInstantiate = highWallPrefab;  // ���� �� �ν��Ͻ�ȭ
                        posY = 1.15f;  // ���� ���� Y�� ��ġ�� 1.15f�� ����
                        break;
                    default:
                        toInstantiate = greenSpacePrefab;  // �⺻������ ��� ���� ó��
                        posY = 0.5f;  // ��� ������ �⺻ Y�� ��ġ
                        break;
                }
                float posX = startPosX + x * spacing;
                float posZ = startPosZ - y * spacing;
                GameObject instance = Instantiate(toInstantiate, new Vector3(posX, posY, posZ), Quaternion.identity);
                instance.transform.parent = terrain.transform;  // Terrain�� �θ�� ����
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
