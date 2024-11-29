using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D defaultCursor;      // 기본 커서
    public Texture2D attackCursor;       // 공격 카드 드래그 시 커서
    public Texture2D skillCursor;        // 스킬 카드 드래그 시 커서
    public Vector2 hotSpot = Vector2.zero; // 커서의 중심점

    private void Start()
    {
        // 게임 시작 시 기본 커서로 설정
        SetDefaultCursor();
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, hotSpot, CursorMode.Auto);
    }

    public void SetAttackCursor()
    {
        Cursor.SetCursor(attackCursor, hotSpot, CursorMode.Auto);
    }

    public void SetSkillCursor()
    {
        Cursor.SetCursor(skillCursor, hotSpot, CursorMode.Auto);
    }
}