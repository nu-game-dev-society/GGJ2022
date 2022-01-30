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

    public Material[] Materials
    {
        get
        {
            if (this.materials == null || this.materials.Length == 0)
                this.materials = this.GetMaterials().ToArray();
            return this.materials;
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        foreach (Material mat in this.Materials)
        {
            Color newEmissionColor = Color.Lerp(mat.GetColor("_EmissionColor"), targetEmissionColour, Time.deltaTime * 10f);
            mat.SetColor("_EmissionColor", newEmissionColor);
        }
    }

    public void SetColor(Color newEmissionColor)
    {
        foreach (Material mat in this.Materials)
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
        foreach (Material mat in this.Materials)
        {
            mat.EnableKeyword("_EMISSION");
        }
    }

    public void TurnOff()
    {
        foreach (Material mat in this.Materials)
        {
            mat.DisableKeyword("_EMISSION");
        }
    }
}
