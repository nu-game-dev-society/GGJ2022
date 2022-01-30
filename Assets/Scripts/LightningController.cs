using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LightningController : MonoBehaviour
{
    Material[] materials;
    Color targetEmissionColour;

    private void Start()
    {
        this.materials = this.GetMaterials().ToArray();
    }

    private void Update()
    {
        foreach (Material mat in this.materials)
        {
            Color newEmissionColor = Color.Lerp(mat.GetColor("_EmissionColor"), targetEmissionColour, Time.deltaTime * 10f);
            mat.SetColor("_EmissionColor", newEmissionColor);
        }
    }

    public void SetColor(Color newEmissionColor)
    {
        foreach (Material mat in this.materials)
        {
            mat.SetColor("_EmissionColor", newEmissionColor);
        }
    }

    public void SetTargetColor(Color targetEmissionColour)
    {
        this.targetEmissionColour = targetEmissionColour;
    }

    public void TurnOn()
    {
        foreach (Material mat in this.materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    public void TurnOff()
    {
        foreach (Material mat in this.materials)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }
}
