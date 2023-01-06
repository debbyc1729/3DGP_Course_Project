using UnityEngine;

public class GrayScale : MonoBehaviour
{
    public Shader shader;
    Material material;
    PlayerInfoMgr info;

    // Start is called before the first frame update
    void Start()
    {
        this.material = new Material(this.shader);
        info = FindObjectOfType<PlayerInfoMgr>();
    }

    // Update is called once per frame
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        SetUniform();
        Graphics.Blit(source, destination, this.material);
    }

    void SetUniform()
    {
        float saturation = info.GetHp() * 2f;
        if (saturation > 1f) saturation = 1f;
        if (saturation < 0f) saturation = 0f;

        float value = saturation + 0.3f;
        if (value > 1f) value = 1f;
        if (value < 0f) value = 0f;

        this.material.SetFloat("_Saturation", saturation);
        this.material.SetFloat("_Value", saturation + 0.3f);
    }
}
