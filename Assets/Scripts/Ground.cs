using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public Texture2D srcTexture;

    Texture2D newTexture;
    SpriteRenderer sr;

    float worldWidth, worldHeight;
    int pixelWidth, pixelHeight;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer> ();
        newTexture = Instantiate(srcTexture);
        // newTexture = new Texture2D(100, 20);

        // for (int i = 0; i < newTexture.width; i++ )
        // {
        //     for (int j = 0; j < newTexture.height; j++ )
        //     {
        //         newTexture.SetPixel(i, j, Color.white);
        //     }
        // }

        newTexture.Apply();
        MakeSprite();

        worldWidth = sr.bounds.size.x;
        worldHeight = sr.bounds.size.y;
        pixelWidth = sr.sprite.texture.width;
        pixelHeight = sr.sprite.texture.height;

        Debug.Log("World: " + worldWidth + ", " + worldHeight + " Pixel: " + pixelWidth + ", " + pixelHeight);

        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void MakeAHole(PolygonCollider2D c2d)
    {
        Vector2Int colliderCenter = WorldToPixel(c2d.bounds.center);
        int radius = Mathf.RoundToInt(c2d.bounds.size.x / 2 * pixelWidth / worldWidth);

        int px, nx, py, ny, distance;
        for (int i = 0; i < radius; i++)
        {
            distance = Mathf.RoundToInt(Mathf.Sqrt(radius * radius - i * i));
            //Debug.Log(distance);
            for (int j = 0; j < distance; j++)
            {
                px = colliderCenter.x + i;
                nx = colliderCenter.x - i;
                py = colliderCenter.y + j;
                ny = colliderCenter.y - j;

                newTexture.SetPixel(px, py, Color.clear);
                newTexture.SetPixel(nx, py, Color.clear);
                newTexture.SetPixel(px, ny, Color.clear);
                newTexture.SetPixel(nx, ny, Color.clear);
            }
        }

        newTexture.Apply();
        MakeSprite();

        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.GetComponent<CircleCollider2D>()) return;
        if (!collision.GetComponent<PolygonCollider2D>()) return;

        //MakeAHole(collision.GetComponent<CircleCollider2D>());
        MakeAHole(collision.GetComponent<PolygonCollider2D>());
    }

    public void MakeDot(Vector3 pos)
    {
        Debug.Log(pos);
        Vector2Int pixelPosition = WorldToPixel(pos);

        Debug.Log(pos + "/" + pixelPosition);

        newTexture.SetPixel(pixelPosition.x, pixelPosition.y, Color.clear);

        newTexture.Apply();
        MakeSprite();
    }

    void MakeSprite()
    {
        sr.sprite = Sprite.Create(newTexture, new Rect(0 , 0, newTexture.width, newTexture.height), Vector2.one * 0.5f);
    }

    private Vector2Int WorldToPixel(Vector3 pos)
    {
        Vector2Int pixelPosition = Vector2Int.zero;

        var dx = pos.x - transform.position.x;
        var dy = pos.y - transform.position.y;

        pixelPosition.x = Mathf.RoundToInt(0.5f * pixelWidth + dx * (pixelWidth / worldWidth));
        pixelPosition.y = Mathf.RoundToInt(0.5f * pixelHeight + dy * (pixelHeight / worldHeight));

        return pixelPosition;
    }
}
