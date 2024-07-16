using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecoilPoint : MonoBehaviour
{
    // Inspector
    public TMP_Text text;

    //public
    public RecoilPointData recoilPointData;

    // Unity lifecylce
    void Start()
    {
        recoilPointData = new RecoilPointData();
    }

    void Update()
    {
        
    }

    public void InitRecoilPoint(int number)
    {
        name = $"Recoil{number.ToString()}";
        text.text = number.ToString();
    }
}
