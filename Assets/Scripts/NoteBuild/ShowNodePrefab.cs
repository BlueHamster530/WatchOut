using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNodePrefab : MonoBehaviour
{
    [SerializeField]
    Sprite[] images;
    SpriteRenderer renderer;
    // Start is called before the first frame update

    public void Init(int ImageType)
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = images[ImageType];
        Destroy(this.gameObject, 2.0f);
    }

}
