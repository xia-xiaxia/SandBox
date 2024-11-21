using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float crosshairSize = 30f; // 调整准星大小
    public Color crosshairColor = Color.green; // 准星颜色

    void OnGUI()
    {
        // 计算准星的位置
        float x = Screen.width / 2f;
        float y = Screen.height / 2f;

        // 设置准星颜色
        GUI.color = crosshairColor;

        // 绘制准星
        // 横线
        GUI.DrawTexture(new Rect(x - crosshairSize / 2, y - 2, crosshairSize, 4), Texture2D.whiteTexture);
        // 竖线
        GUI.DrawTexture(new Rect(x - 2, y - crosshairSize / 2, 4, crosshairSize), Texture2D.whiteTexture);
    }
}
