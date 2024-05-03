using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    public GameObject redWallPrefab;  // 빨간 벽 오브젝트 csv:1
    public GameObject greenSpacePrefab;  // 녹색 공간 오브젝트 csv:0
    public GameObject highWallPrefab;  // 높은 벽 오브젝트 csv:2
    public GameObject terrain;  // Terrain 객체에 대한 참조

    void Start()
    {
        GenerateMapFromCSV("Assets/Midterm/Resources/map.csv");
    }

    void GenerateMapFromCSV(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);
        int width = lines[0].Split(',').Length;
        int height = lines.Length;
        float spacing = 1.0f;  // 오브젝트 간의 간격을 설정
        float startPosX = -15f;  // 시작 위치 X
        float startPosZ = 15f;   // 시작 위치 Z

        for (int y = 0; y < height; y++)
        {
            string[] line = lines[y].Split(',');
            for (int x = 0; x < width; x++)
            {
                GameObject toInstantiate;
                float posY;  // Y축 위치를 설정할 변수

                switch (line[x])
                {
                    case "1":
                        toInstantiate = redWallPrefab;
                        posY = 0.25f;  // 낮은 벽의 기본 Y축 위치
                        break;
                    case "2":
                        toInstantiate = highWallPrefab;  // 높은 벽 인스턴스화
                        posY = 1.15f;  // 높은 벽의 Y축 위치를 1.15f로 설정
                        break;
                    default:
                        toInstantiate = greenSpacePrefab;  // 기본값으로 녹색 공간 처리
                        posY = 0.5f;  // 녹색 공간의 기본 Y축 위치
                        break;
                }
                float posX = startPosX + x * spacing;
                float posZ = startPosZ - y * spacing;
                GameObject instance = Instantiate(toInstantiate, new Vector3(posX, posY, posZ), Quaternion.identity);
                instance.transform.parent = terrain.transform;  // Terrain을 부모로 설정
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
