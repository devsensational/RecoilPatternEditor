using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RecoilGenerator : MonoBehaviour
{
    // Inspector
    [Header("File settings")]
    public TextAsset JsonFile;
    public string fileName = "";

    [Header("Generator options")]
    public GameObject pointPrefab;

    public int pointCount = 30;

    // private
    List<LineRenderer>      lineRenderers   = new List<LineRenderer>();
    List<RecoilPoint>       points          = new List<RecoilPoint>();
    List<RecoilPointData>   pointDatas      = new List<RecoilPointData>();
    UJsonUtility            jsonUtility     = new UJsonUtility();

    // Unity Lifecycle
    void Start()
    {
        jsonUtility.filePath = Path.Combine(Application.dataPath, "Json/");
    }
    void Update()
    {
        LineRendering();
        CalculatePoint();
    }

    // JSON script methods
    public void LoadScripts()
    {
        List<RecoilPointData> loadedData = jsonUtility.LoadJsonFile<List<RecoilPointData>>(JsonFile);

        pointCount = loadedData.Count + 1;
        SetPointCount();

        for(int i = 1; i < points.Count; i++)
        {
            points[i].transform.position = points[i - 1].transform.position + new Vector3(loadedData[i - 1].x, loadedData[i - 1].y);
        }

    }

    public void SaveScripts()
    {
        jsonUtility.fileName = fileName;
        jsonUtility.filePath = Path.Combine(Application.dataPath, $"Json/{fileName}");
        jsonUtility.SaveJsonFile<List<RecoilPointData>>(pointDatas);
       
    }

    // Line render methods
    void InitLineRenderer() // 
    {
        if (points.Count < 2)
        {
            Debug.LogError("리스트에 최소한 두 개 이상의 게임 오브젝트가 필요합니다.");
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            GameObject lineObject = new GameObject("LineRendererObject_" + i);
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            // LineRenderer setting
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            // Line color (if req)
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;

            lineRenderers.Add(lineRenderer);
        }
    }

    void LineRendering()
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            if (points[i] != null && points[i + 1] != null)
            {
                lineRenderers[i].SetPosition(0, points[i].transform.position);
                lineRenderers[i].SetPosition(1, points[i + 1].transform.position);
            }
        }
    }

    // Generator methods
    public void SetPointCount()
    {
        if( points.Count < pointCount )
        {
            int count = pointCount - points.Count;
            for (int i = 0; i < count; i++)     GenerateNewPoint();
            for (int i = 0; i < count - 1; i++) GenerateNewPointData();
            InitLineRenderer();
        }
        else if ( points.Count > pointCount )
        {
            int count = points.Count - pointCount;
            for (int i = 0; i < count; i++)     RemovePoint();
            for (int i = 0; i < count - 1; i++) RemovePoint();
        }
    }

    void GenerateNewPoint()
    {
        RecoilPoint ptr = Instantiate(pointPrefab).GetComponent<RecoilPoint>();
        points.Add(ptr);
        ptr.InitRecoilPoint(points.Count);
    }

    void GenerateNewPointData()
    {
        pointDatas.Add(new RecoilPointData());
    }

    void RemovePoint()
    {
        RecoilPoint ptr = points[points.Count - 1];
        points.RemoveAt(points.Count - 1);
        Destroy(ptr.gameObject);
        
        LineRenderer lineRendererPtr = lineRenderers[points.Count - 1];
        lineRenderers.RemoveAt(lineRenderers.Count - 1);
        Destroy(lineRendererPtr.gameObject);
    }

    void RemovePointData()
    {
        pointDatas.RemoveAt(points.Count - 1);
    }

    void CalculatePoint()
    {
        for (int i = 1; i < points.Count; i++)
        {
            Vector3 pos = points[i].transform.position - points[i - 1].transform.position;
            pointDatas[i - 1].x = pos.x;
            pointDatas[i - 1].y = pos.y;
        }
    }
}
