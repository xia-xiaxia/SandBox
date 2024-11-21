using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float crosshairSize = 30f; // ����׼�Ǵ�С
    public Color crosshairColor = Color.green; // ׼����ɫ

    void OnGUI()
    {
        // ����׼�ǵ�λ��
        float x = Screen.width / 2f;
        float y = Screen.height / 2f;

        // ����׼����ɫ
        GUI.color = crosshairColor;

        // ����׼��
        // ����
        GUI.DrawTexture(new Rect(x - crosshairSize / 2, y - 2, crosshairSize, 4), Texture2D.whiteTexture);
        // ����
        GUI.DrawTexture(new Rect(x - 2, y - crosshairSize / 2, 4, crosshairSize), Texture2D.whiteTexture);
    }
}
