using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PrallaxBackground : MonoBehaviour
{
    [SerializeField]private Material bg1, bg2, bg3, bg4;

    private float offset1, offset2, offset3, offset4;


    private void Update()
    {
        offset1 += Time.deltaTime * 0.01f;
        offset2 += Time.deltaTime * 0.02f;
        offset3 += Time.deltaTime * 0.03f;
        offset4 += Time.deltaTime * 0.04f;
        bg1.mainTextureOffset = new Vector2(offset1, 0);
        bg2.mainTextureOffset = new Vector2(offset2, 0);
        bg3.mainTextureOffset = new Vector2(offset3, 0);
        bg4.mainTextureOffset = new Vector2(offset4, 0);
    }

}
