using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;


// 에디터에서 스크립트를 사용자 정의 인스펙터로 확장하는 클래스
[CustomEditor(typeof(StaticMeshGen))]
public class StaticMeshGenEditor : Editor
{
    //버튼만들기 예제
    // 인스펙터 GUI를 위한 오버라이드 메서드
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        StaticMeshGen script = (StaticMeshGen)target; // StaticMeshGen 타입의 현재 스크립트 인스턴스를 가져옴.
        // 인스펙터에 'Generate Mesh' 버튼을 생성하고, 이 버튼을 클릭하면 GenerateMesh 함수를 호출
        if (GUILayout.Button("Generate Mesh"))
        {
            script.GenerateMesh();
        }

    }
}


// 메쉬를 생성하는 주 스크립트
public class StaticMeshGen : MonoBehaviour
{
    // 메쉬를 생성하고 설정하는 메서드
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

        // 두 별의 꼭지점을 하나의 리스트로 합침
        List<Vector3> combinedVertices = new List<Vector3>(Star1);
        combinedVertices.AddRange(Star2); // 두 번째 별의 정점을 추가
        Star1 = combinedVertices.ToArray();
   
        mesh.vertices = Star1; // 메시에 정점 배열 할당

        mesh.triangles = GenerateTriangles(Star1);// 메시의 삼각형을 생성하고 할당

        mesh.RecalculateNormals();// 메시의 법선 재계산
  
        MeshFilter mf = GetComponent<MeshFilter>() ?? gameObject.AddComponent<MeshFilter>(); // MeshFilter 컴포넌트를 가져오거나 없으면 추가  
        MeshRenderer mr = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>(); // MeshRenderer 컴포넌트를 가져오거나 없으면 추가

        mf.mesh = mesh;// 메시를 MeshFilter 컴포넌트에 할당
        // 법선 벡터 계산 및 할당
        CalculateNormals(mesh, mesh.vertices, mesh.triangles);

        // 노란색 재질을 생성하고 메시에 적용
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>() ?? gameObject.AddComponent<MeshRenderer>();
        Material yellowMaterial = new Material(Shader.Find("Custom/CartoonShader"));
        meshRenderer.sharedMaterial = yellowMaterial;
    }

    // 주어진 반지름과 점의 수를 기준으로 별 모양의 꼭지점을 계산하는 메서드
    Vector3[] CalculateStarVertices(float innerRadius, float outerRadius, int numPoints)
    {
        List<Vector3> Star1 = new List<Vector3>();
        float angleStep = 360.0f / (numPoints * 2);  // 꼭지점 사이의 각도를 계산
        float angleOffset = 90.0f; // 첫 번째 꼭지점을 수직으로 올리기 위한 각도 조정

        Star1.Add(Vector3.zero);// 오각별의 중심점을 추가

        
        for (int i = 0; i < numPoints * 2; i++)// 별의 바깥쪽과 안쪽 꼭지점을 번갈아가면서 계산
        {
            float angle = Mathf.Deg2Rad * (i * angleStep + angleOffset); // 각도에 오프셋 추가
            float radius = (i % 2 == 0) ? outerRadius : innerRadius; // 짝수 인덱스에서는 외부 반지름, 홀수 인덱스에서는 내부 반지름 사용
            Vector3 vertex = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            Star1.Add(vertex);
        }

        // 첫 번째 외부 꼭지점을 다시 추가하여 별 모양이 닫히도록 함
        Star1.Add(Star1[1]); // 별의 외곽선이 닫히도록 마지막에 첫 번째 외부 꼭지점을 다시 추가

        return Star1.ToArray();
    }


    // 정점 배열을 기반으로 메쉬의 삼각형 배열을 생성하는 메서드
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

    // 메시의 법선을 수동으로 계산하고 설정하는 메서드
    public void CalculateNormals(Mesh mesh, Vector3[] vertices, int[] triangles)
    {
        Vector3[] normals = new Vector3[vertices.Length];

        // 모든 정점에 대해 법선을 0으로 초기화
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = Vector3.zero;
        }

        // 삼각형의 각 꼭지점에 대한 법선을 누적
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int index0 = triangles[i];
            int index1 = triangles[i + 1];
            int index2 = triangles[i + 2];

            // 삼각형의 법선 계산
            Vector3 triangleNormal = Vector3.Cross(
                vertices[index1] - vertices[index0],
                vertices[index2] - vertices[index0]).normalized;

            normals[index0] += triangleNormal;
            normals[index1] += triangleNormal;
            normals[index2] += triangleNormal;
        }

        // 누적된 법선을 정규화하여 평균화
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
        }

        // 메시에 법선 배열 할당
        mesh.normals = normals;
    }



    // Update is called once per frame
    void Update()
        {
        //스크립트 실행 사이클
        //OnInspectorGUI:
        //CalculateStarVertices 함수
        //innerRadius: 별의 내부 반지름으로, 별의 내부 코너(오목한 부분)의 반경을 나타냄
        //outerRadius: 별의 외부 반지름으로, 별의 외부 코너(볼록한 부분)의 반경을 나타냄
        //numPoints: 별의 꼭지점(볼록한 부분)의 수를 나타냅니다.일반적인 별은 5개의 꼭지점을 가지므로, 오각별을 나타내는 데 주로 사용
        //Unity 에디터에서 사용자 정의 인스펙터(GUI)를 만드는데 사용
        //"Generate Mesh" 버튼을 추가하고, 버튼을 클릭할 때 GenerateMesh 함수를 호출하여 메시를 생성하는 데 사용

        //GenerateMesh:
        //별의 꼭지점 계산: CalculateStarVertices 함수를 호출하여 첫 번째 별과 두 번째 별의 꼭지점을 계산
        //별 회전 및 이동: 두 번째 별의 꼭지점에 회전과 이동을 적용하여, 첫 번째 별과 구별되게 함
        //정점 배열 결합: 두 별의 정점 배열을 하나로 결합하여 메시의 정점 배열을 형성
        //삼각형 배열 생성: GenerateTriangles 함수를 통해 메시를 구성하는 삼각형의 인덱스 배열을 생성
        //메시 정의: 정점 배열과 삼각형 배열을 메시에 할당하고, 메시의 법선을 재계산함
        //메시 렌더링 설정: 메시를 포함할 MeshFilter 컴포넌트와 메시에 색상을 적용할 MeshRenderer 컴포넌트를 설정하거나 추가
        //재질 적용: 메시에 노란색 재질을 적용

        //CalculateStarVertices:
        //별 모양의 정점을 계산하는 함수
        //오각별(별 모양)의 꼭지점을 계산하기 위해, 외부 꼭지점과 내부 꼭지점을 번갈아가며 생성
        //이 함수는 중심점에서 시작하여 주어진 각도와 반지름을 사용해 별의 각 꼭지점의 위치를 계산
        //별 모양이 닫히도록 마지막에 첫 번째 꼭지점을 다시 배열에 추가

        //GenerateTriangles:
        //메시를 구성하는 삼각형의 인덱스 배열을 생성하는 함수
        //각 별의 앞면과 뒷면에 대한 삼각형을 생성하고, 두 별을 연결하는 측면에 대한 삼각형도 생성
        //이 함수는 vertices 배열을 입력으로 받아, 이 배열에서 삼각형을 구성하는 각 꼭지점의 인덱스를 계산하고 배열로 반환

    }
}
