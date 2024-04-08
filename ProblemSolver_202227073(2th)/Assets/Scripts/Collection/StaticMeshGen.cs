using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //버튼만들기 예제
    // 인스펙터 GUI를 위한 오버라이드 메서드
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target;
        // 'Generate Mesh' 버튼이 눌리면 StaticMeshGen 스크립트의 GenerateMesh 함수를 호출
        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}

//메쉬만들기 예제
// 메쉬를 생성하는 주 스크립트
public class StaticMeshGen : MonoBehaviour
{
    // 메쉬를 생성하는 메서드
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        Vector3[] Star1 = CalculateStarVertices(0.5f, 1f, 5); // 첫 번째 별 정점 계산       
        Vector3[] Star2 = CalculateStarVertices(0.5f, 1f, 5); // 두 번째 별 정점 계산
        // 두 번째 별을 첫 번째 별을 바라보는 반대 방향으로 회전하고, Z축으로 -1만큼 이동
        Quaternion rotation = Quaternion.Euler(0, 180, 0); // Y축을 중심으로 180도 회전하여 반대 방향을 바라보게 함
        for (int i = 0; i < Star2.Length; i++)
        {
            Star2[i] = rotation * Star2[i]; // 회전 적용
            Star2[i] += new Vector3(0, 0, -1); // Z축으로 -1만큼 이동
        }

        // 두 별의 정점을 합침
        List<Vector3> combinedVertices = new List<Vector3>(Star1);
        combinedVertices.AddRange(Star2); // 두 번째 별의 정점을 추가
        Star1 = combinedVertices.ToArray();

        // 메시에 정점 배열 할당
        mesh.vertices = Star1;

        // 삼각형 배열 생성 및 할당
        mesh.triangles = GenerateTriangles(Star1);

        // 메시의 법선 재계산
        mesh.RecalculateNormals();

        // MeshFilter 및 MeshRenderer 컴포넌트 설정
        MeshFilter mf = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();

        mf.mesh = mesh;

        // 노란색 메터리얼 생성 및 적용
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
        Material yellowMaterial = new Material(Shader.Find("Custom/YellowShader"));
        meshRenderer.sharedMaterial = yellowMaterial;
    }

    // 별 모양의 정점을 계산하는 메서드
    Vector3[] CalculateStarVertices(float innerRadius, float outerRadius, int numPoints)
    {
        List<Vector3> Star1 = new List<Vector3>();
        float angleStep = 360.0f / (numPoints * 2); // 외부 및 내부 꼭지점을 위한 각도 스텝 꼭지점과 꼭지점 사이의 각도
        float angleOffset = 90.0f; // 첫 번째 꼭지점을 수직으로 올리기 위한 각도 조정

        // 중심점 추가 (오각별의 중심)
        Star1.Add(Vector3.zero);
        // 외부 및 내부 꼭지점을 계산하는 반복문
        for (int i = 0; i < numPoints * 2; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep + angleOffset); // 각도에 오프셋 추가
            float radius = (i % 2 == 0) ? outerRadius : innerRadius; // 짝수 인덱스에서는 외부 반지름, 홀수 인덱스에서는 내부 반지름 사용
            Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Star1.Add(vertex);
        }

        // 첫 번째 외부 꼭지점을 다시 추가하여 별 모양이 닫히도록 함
        Star1.Add(Star1[1]);

    return Star1.ToArray();
    }


    // 새로운 메소드: 정점 배열을 기반으로 삼각형 배열 생성
    int[] GenerateTriangles(Vector3[] vertices)
    {
        List<int> triangles = new List<int>();

        int halfLength = vertices.Length / 2; // Star1과 Star2 각각의 꼭지점 수

        // 각 별에 대해 앞면과 뒷면의 삼각형 생성
        for (int i = 0; i < halfLength - 1; i++)
        {
            int next = (i + 1) % (halfLength - 1);
            int nextPlusOne = (i + 2) % (halfLength - 1);

            // 앞면 삼각형
            triangles.Add(0); // 중심점
            triangles.Add(next); // 다음 꼭지점
            triangles.Add(i + 1); // 현재 꼭지점

            // 뒷면 삼각형 (반대 방향)
            triangles.Add(halfLength); // 중심점
            triangles.Add(halfLength + i + 1); // 현재 꼭지점
            triangles.Add(halfLength + next); // 다음 꼭지점
        }

        // Star1과 Star2를 연결하는 삼각형에 대해서도 동일하게 적용
        for (int i = 1; i < halfLength - 1; i++)
        {
            // Star1 -> Star2 -> Star2
            triangles.Add(i);
            triangles.Add(i + halfLength);
            triangles.Add(i + halfLength + 1);

            // 반대 방향 (뒷면)
            triangles.Add(i + halfLength + 1);
            triangles.Add(i + halfLength);
            triangles.Add(i);

            // Star1 -> Star2 -> Star1
            triangles.Add(i);
            triangles.Add(i + halfLength + 1);
            triangles.Add(i + 1);

            // 반대 방향 (뒷면)
            triangles.Add(i + 1);
            triangles.Add(i + halfLength + 1);
            triangles.Add(i);
        }

        return triangles.ToArray();
    }

    // Update is called once per frame
    void Update()
        {
        
   
        }
}
