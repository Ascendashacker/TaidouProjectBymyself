
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 将该脚本挂载在需要合并网格的模型上
/// </summary>
public class CombineMeshUtils:MonoBehaviour
{
    /// <summary>
    /// 合并网格
    /// </summary>
    private void CombineMeshes()
    {
        Dictionary<Material,List<MeshFilter>> matMeshDic = new Dictionary<Material, List<MeshFilter>> ();
        MeshRenderer[] meshRenderers = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        if (meshRenderers.Length <= 1) return;
        foreach (var meshRenderer in meshRenderers)
        {
            Material mat = meshRenderer.sharedMaterial;
            MeshFilter meshFilter = meshRenderer.gameObject.GetComponent<MeshFilter>();
            if (matMeshDic.ContainsKey(mat))
            {
                matMeshDic[mat].Add(meshFilter);
            }
            else
            {
                List<MeshFilter> meshFilters = new List<MeshFilter>();
                meshFilters.Add(meshFilter);
                matMeshDic.Add(mat, meshFilters);
            }
        }
        foreach (var mat in matMeshDic.Keys)
        {
            if (IsSurfaceNumberExceeding(matMeshDic[mat]))
            {
                continue;
            }
            GameObject obj = new GameObject ();
            obj.transform.SetParent(this.transform,true);
            CombineInstance[] combines = new  CombineInstance[matMeshDic[mat].Count];
            Material[] mats = new Material[1];
            int index = 0;
             mats[0] = mat;
            for (int i = 0; i < matMeshDic[mat].Count; i++)
            {
                MeshFilter meshFilter = matMeshDic[mat][i];
                if (meshFilter.mesh == null) continue;
                combines[index].mesh = meshFilter.mesh;
                combines[index++].transform = meshFilter.gameObject.GetComponent<MeshFilter>().transform.localToWorldMatrix;
                DestroyImmediate(meshFilter.gameObject);
            }
            obj.AddComponent<MeshFilter>().mesh = new Mesh();
            obj.AddComponent<MeshRenderer>();
            obj.GetComponent<MeshFilter>().mesh.CombineMeshes(combines);
            obj.GetComponent<MeshRenderer>().sharedMaterials = mats;
        }
    }

    /// <summary>
    /// 判断要合并的网格是否面数超过
    /// </summary>
    /// <param name="meshs">网格集合</param>
    /// <returns></returns>
    private bool IsSurfaceNumberExceeding(List<MeshFilter> meshs)
    {
        int vertics = 0;
        for (int i = 0; i < meshs.Count; i++)
        {
            vertics += meshs[i].mesh.vertices.Length;
        }
        if (vertics > 65536)
        {
            return true;
        }
        return false;
    }
}
