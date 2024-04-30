using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] objects; // 오브젝트 프리팹 배열 (0: 없음, 1: 낮은 벽, 2: 높은 벽)
    private float planeWidth;
    private float planeHeight;

    void Start()
    {
        planeWidth = transform.localScale.x;
        planeHeight = transform.localScale.z;
        GenerateMap();
    }
    void GenerateMap()
    {
        string[] lines = File.ReadAllLines("Assets/Resources/Map.csv");
        int rows = lines.Length;
        int cols = lines[0].Split(',').Length;
        float PosX=-16f;
        float PosZ=15f;
        for (int i = 0; i < rows; i++)
        {
            string[] line = lines[i].Split(',');
            for (int j = 0; j < cols; j++)
            {
                int index = int.Parse(line[j]);
                if (index != 0) // 0이 아닐 때만 오브젝트 생성
                {
                    Instantiate(objects[index],new Vector3(PosX,0,PosZ), Quaternion.identity);
                    PosX += objects[1].transform.localScale.x;
                }
            }
            PosZ -= objects[1].transform.localScale.x;
            PosX = -16f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
