using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private List<CursorAnimation> cursorAnimationList; // 애니메이션 리스트

    public CursorAnimation cursorAnimation;

    public bool isClicked;
    public int currentFrame;    // 현재 프레임
    private float frameTimer;   // 프레임 타이머
    private int frameCount;     // 프레임 갯수

    public enum CursorType
    {
        Default,
        Click,
        Clicked
    }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isClicked = false;
        SetActiveCursorType(CursorType.Default);
    }

    void Update()
    {
        // 커서는 unscaledDeltaTime을 사용하여 timeScale이 0인 상황에 제외되어 커서 애니메이션은 작동되게 한다.
        frameTimer -= Time.unscaledDeltaTime;
        if (frameTimer <= 0f)
        {
            currentFrame = (currentFrame + 1) % frameCount;         // 현재프레임 +1 해주는 식
            frameTimer += cursorAnimation.frameRate[currentFrame];  // 현재프레임의 다음 넘어갈 시간
            Cursor.SetCursor(cursorAnimation.textureArray[currentFrame], cursorAnimation.offset, CursorMode.Auto);  // 커서 이미지 설정
        }
    }

    public void SetActiveCursorType(CursorType cursorType)  // 커서 타입을 설정하면 그 타입의 애니메이션 실행
    {
        SetActiveCursorAnimation(GetCursorAnimation(cursorType));
    }

    private CursorAnimation GetCursorAnimation(CursorType cursorType)   // 커서 타입의 애니메이션 Get 
    {
        foreach (CursorAnimation cursorAnimation in cursorAnimationList)
        {
            if (cursorAnimation.cursorType == cursorType)
                return cursorAnimation;
        }
        // Couldn't find this CursorType!
        return null;
    }

    private void SetActiveCursorAnimation(CursorAnimation cursorAnimation)  // 실행 될 애니메이션 값들 설정 
    {
        this.cursorAnimation = cursorAnimation;
        currentFrame = 0;
        frameTimer = cursorAnimation.frameRate[currentFrame];
        frameCount = cursorAnimation.textureArray.Length;
    }

    [System.Serializable]
    public class CursorAnimation
    {
        public CursorType cursorType;     // 커서의 타입
        public Texture2D[] textureArray;  // 커서 애니메이션
        public float[] frameRate;         // 애니메이션 사이사이 바뀌는 시간
        public Vector2 offset;            // 위치 설정
    }
}
