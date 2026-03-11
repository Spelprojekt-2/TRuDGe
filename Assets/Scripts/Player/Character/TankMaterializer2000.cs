using UnityEngine;
using System.Collections.Generic;

public class TankMaterializer2000 : MonoBehaviour
{
    [System.Serializable]
    public struct MaterialScheme
    {
        public Material Hull;
        public Material Fenders;
        public Material Turret;
        public Material Wheels;
        public MaterialScheme(Material hullMat, Material fenderMat, Material turretMat, Material wheelMat)
        {
            Hull = hullMat;
            Fenders = fenderMat;
            Turret = turretMat;
            Wheels = wheelMat;
        }
    }

    [SerializeField] private List<MaterialScheme> materialSchemes;
    [SerializeField] private List<SkinnedMeshRenderer> hullParts;
    [SerializeField] private List<SkinnedMeshRenderer> fenderParts;
    [SerializeField] private List<SkinnedMeshRenderer> turretParts;
    [SerializeField] private List<SkinnedMeshRenderer> wheelParts;

    public void SwitchMaterialScheme(int scheme)
    {
        foreach (var part in hullParts)
        {
            part.SetMaterials(new List<Material>(){materialSchemes[scheme].Hull});
        }
        foreach (var part in fenderParts)
        {
            part.SetMaterials(new List<Material>(){materialSchemes[scheme].Fenders});
        }
        foreach (var part in turretParts)
        {
            part.SetMaterials(new List<Material>(){materialSchemes[scheme].Turret});
        }
        foreach (var part in wheelParts)
        {
            part.SetMaterials(new List<Material>(){materialSchemes[scheme].Wheels});
        }
    }
}