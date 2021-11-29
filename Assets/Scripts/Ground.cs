using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Ground : MonoBehaviourPunCallbacks
{
    // 땅도 직접 동기화가 되어야하므로, 땅에도 포톤뷰가 존재해야한다.


    public Texture2D srcTexture;

    Texture2D newTexture;
    SpriteRenderer sr;

    float worldWidth, worldHeight;
    int pixelWidth, pixelHeight;

    public int radius = 70;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer> ();
        newTexture = Instantiate(srcTexture);

        newTexture.Apply();
        MakeSprite();

        worldWidth = sr.bounds.size.x;
        worldHeight = sr.bounds.size.y;
        pixelWidth = sr.sprite.texture.width;
        pixelHeight = sr.sprite.texture.height;

        Debug.Log("World: " + worldWidth + ", " + worldHeight + " Pixel: " + pixelWidth + ", " + pixelHeight);

        gameObject.AddComponent<PolygonCollider2D>();
    }
    //scale 을 파라미터로 받아서, 폭발 범위 조절
    public void MakeAHole(PolygonCollider2D c2d, float scale)
    {
        Vector2Int colliderCenter = WorldToPixel(c2d.bounds.center);
        int applicateRadius = (int)(radius * scale);
        int px, nx, py, ny, distance;
        for (int i = 0; i < applicateRadius; i++)
        {
            distance = Mathf.RoundToInt(Mathf.Sqrt(applicateRadius * applicateRadius - i * i));
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

        //지형에 닿아서 폭발시 폴리곤 콜라이더를 삭제하고 새롭게 만드는듯
        //이렇게 하면 폭발 지점이 아니라 기존의 더룬 지형이 변경됨
        //어차피 이 함수를 , 동시에, 모든 클라이언트에 뿌려주면 됨. 
        Destroy(gameObject.GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public void SetGroundInGround(Collider2D collision)
    {
        //if (!collision.GetComponent<CircleCollider2D>()) return;
        if (!collision.GetComponent<PolygonCollider2D>()) return;

        //MakeAHole(collision.GetComponent<CircleCollider2D>());
        //expScale은 vector값이라서 sqrMagnitud / 3에 sqrt적용해서 scale을 측정
        if (!collision.GetComponentInParent<AssetProjectile>()
            .ExceptField && !collision.GetComponentInParent<AssetProjectile>()
            .Teleport)
        {
            MakeAHole(collision.GetComponent<PolygonCollider2D>()
            , collision.GetComponentInParent<AssetProjectile>()
            .ExpScale);
        }
    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //if (!collision.GetComponent<CircleCollider2D>()) return; (X)


        //if (!collision.GetComponent<PolygonCollider2D>()) return;

        //MakeAHole(collision.GetComponent<CircleCollider2D>());
        //expScale은 vector값이라서 sqrMagnitud / 3에 sqrt적용해서 scale을 측정
        
        //if (!collision.GetComponentInParent<AssetProjectile> ()
        //    .ExceptField && !collision.GetComponentInParent<AssetProjectile> ()
        //    .Teleport) {
        //    MakeAHole(collision.GetComponent<PolygonCollider2D>()
        //    ,collision.GetComponentInParent<AssetProjectile> ()
        //    .ExpScale);
        //}

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
        sr.sprite = Sprite.Create(newTexture, new Rect(0 , 0, newTexture.width, newTexture.height), Vector2.one * 0.5f, 15f);
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
    }


}
