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
        if (GUILayout.Button("Mesh 생성버튼"))
        {
            script.GenerateMesh();
        }

    }
}


// 메쉬를 생성하는 주 스크립트
public class StaticMeshGen : MonoBehaviour
{
    // 메쉬를 생성하고 설정하는 메서드
    public Material material;
    private float height= 3;
    private void Start()
    {
        GenerateMesh();
    }
    public void GenerateMesh()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        MeshRenderer mr = GetComponent<MeshRenderer>();

        if (mf == null) mf = this.AddComponent<MeshFilter>();
        if (mr == null) mr = this.AddComponent<MeshRenderer>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            // 오각형 그리기

            // 가운데 삼각형            
            // 앞면
            new Vector3(0.0f, 0.0f, 0.0f), // 0 기준            
            new Vector3(-2.0f, -6.0f, 0.0f), // 1
            new Vector3(2.0f, -6.0f, 0.0f), // 2

            // 왼쪽 삼각형
            new Vector3(-3.3f, -2.2f, 0.0f), // 3          

            // 오른쪽 삼각형
            new Vector3(3.3f, -2.2f, 0.0f), // 4

            // 뒷면
            // 가운데 삼각형            
            new Vector3(0.0f, 0.0f, height), // 5 기준            
            new Vector3(-2.0f, -6.0f, height), // 6
            new Vector3(2.0f, -6.0f, height), // 7

            // 왼쪽 삼각형
            new Vector3(-3.3f, -2.2f, height), // 8          

            // 오른쪽 삼각형
            new Vector3(3.3f, -2.2f, height), // 9

            // 별 다리 그리기
            // 앞면
            new Vector3(4.06f,2.09f,0f), // 10
            new Vector3(6.47f,-5.28f,0f), // 11
            new Vector3(0f,-10.00f,0f), // 12
            new Vector3(-6.47f,-5.28f,0f), // 13
            new Vector3(-4.06f, 2.09f, 0f), // 14 

            // 뒷면
            new Vector3(4.06f,2.09f, height), // 15
            new Vector3(6.47f,-5.28f, height), // 16
            new Vector3(0f,-10.00f, height), // 17
            new Vector3(-6.47f,-5.28f,height), // 18
            new Vector3(-4.06f, 2.09f, height), // 19 

            // 옆면

                                    
        };

        // 삼각형 인덱스 정의
        int[] triangles = new int[]
        {
            // 오각형 가운데
            // 오각형 앞면
            0,2,1,
            3,0,1,
            0,4,2,
            
            // 오각형 뒷면
            5,6,7,
            5,8,6,
            7,9,5,

            // 옆면 왼쪽
            3,5,0,
            8,5,3,
            3,1,6,
            8,3,6,
            // 옆면 오른쪽
            0,5,9,
            0,9,4,
            2,4,7,
            7,4,9,

            // 아랫면
            7,1,2,
            6,1,7,

            //----------------------------------------------------------------

            // 별 그리기
            // 앞면
            4,0,10,
            2,4,11,
            1,2,12,
            3,1,13,
            0,3,14,

            // 뒷면
            5,9,15,
            9,7,16,
            6,17,7,
            8,18,6,
            19,8,5,
            
            // 옆면
            0,5,15,
            0,15,10,

            4,10,15,
            4,15,9,

            16,4,9,
            16,11,4,

            16,2,11,
            16,7,2,

            12,2,7,
            12,7,17,

            12,6,1,
            12,17,6,

            6,13,1,
            6,18,13,

            18,3,13,
            18,8,3,

            8,14,3,
            8,19,14,

            19,0,14,
            19,5,0,
        };

        // 법선 벡터 계산
        Vector3[] normals = new Vector3[vertices.Length];

        // 각 삼각형에 대한 외적 계산
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

            normals[triangles[i]] += normal;
            normals[triangles[i + 1]] += normal;
            normals[triangles[i + 2]] += normal;
        }

        // 법선을 정규화
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = normals[i].normalized;
        }

        // Mesh 객체에 꼭짓점, 삼각형, 법선 할당
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        // Material 할당
        if (material != null)
            mr.materials = new Material[] { material };


        mf.mesh = mesh;
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

