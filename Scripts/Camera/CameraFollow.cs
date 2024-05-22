using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera camera;
	public Player player;
	public Vector2 minPos, maxPos;
	public float cameraSizeX;
	public float cameraSizeY;
    bool bossZoom;
    Vector3 bossPos;
    IEnumerator zoomInCor;

    void Start()
	{
        camera = GetComponent<Camera>();
		// 캐릭터의 위에 따라 카메라가 이동하도록 하는 메서드
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    void LateUpdate()
	{
		float posX = player.transform.position.x;
		float posY = player.transform.position.y;

		if (posX <= minPos.x + cameraSizeX)
			posX = minPos.x + cameraSizeX;
		if (posX >= maxPos.x - cameraSizeX)
			posX = maxPos.x - cameraSizeX;
		if (posY <= minPos.y + cameraSizeY)
			posY = minPos.y + cameraSizeY;
		if (posY >= maxPos.y - cameraSizeY)
			posY = maxPos.y - cameraSizeY;
        // 카메라 이동
        if (!player.isDie && bossZoom)           // 보스가 죽거나 출현할 때 보스한테 잠시 초점 맞추기
            transform.position = new Vector3(bossPos.x, bossPos.y, transform.position.z);
        else if (!player.isDie)
            transform.position = new Vector3(posX, posY, transform.position.z);
        
    }

    public void ZoomInCoroutine(Vector2 _pos)       // ZoomIn 코루틴 시작
    {
        zoomInCor = ZoomIn(_pos);
        StartCoroutine(zoomInCor);
    }
    public IEnumerator ZoomIn(Vector2 pos)      // ZoomIn 코루틴
    {
        bossZoom = true;
        bossPos = pos;
        Time.timeScale = 0.5f;              // 시간 0.5배속
        camera.orthographicSize = 3f;       // 카메라 사이즈를 3으로 변경
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            camera.orthographicSize -= 0.01f;
        }
        for (int j = 0; j < 100; j++)
        {
            yield return new WaitForSeconds(0.01f);
            camera.orthographicSize += 0.01f;
        }
        camera.orthographicSize = 5f;       // 원래대로
        Time.timeScale = 1f;
        bossZoom = false;
    }

    void OnDrawGizmos()
	{
		Vector3 p1 = new Vector3(minPos.x, maxPos.y, transform.position.z);
		Vector3 p2 = new Vector3(maxPos.x, maxPos.y, transform.position.z);
		Vector3 p3 = new Vector3(maxPos.x, minPos.y, transform.position.z);
		Vector3 p4 = new Vector3(minPos.x, minPos.y, transform.position.z);

		Gizmos.color = Color.green;
		Gizmos.DrawLine(p1, p2);
		Gizmos.DrawLine(p2, p3);
		Gizmos.DrawLine(p3, p4);
		Gizmos.DrawLine(p4, p1);
	}
}