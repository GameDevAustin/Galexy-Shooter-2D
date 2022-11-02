using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLoop : MonoBehaviour
{
    private MeshRenderer _mr;
    [SerializeField] private int _scroll = 25;

    // Start is called before the first frame update
    void Start()
    {
        _mr = GetComponent<MeshRenderer>();

       
    }

    // Update is called once per frame
    void Update()
    {
        Material mat = _mr.material;
        Vector2 offset = mat.mainTextureOffset;
        offset.y += Time.deltaTime / _scroll;
        mat.mainTextureOffset = offset;
    }
}
