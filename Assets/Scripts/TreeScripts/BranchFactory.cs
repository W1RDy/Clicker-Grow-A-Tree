using UnityEngine;

public class BranchFactory : IFactory
{
    private const string BranchPath = "Branch";

    private Branch _branchPrefab;

    public void LoadResources()
    {
        _branchPrefab = Resources.Load<Branch>(BranchPath);
        Debug.Log("Branch");
    }

    public MonoBehaviour Create(Vector2 position, Quaternion rotation, Transform parent)
    {
        Branch branch = UnityEngine.Object.Instantiate(_branchPrefab, position, rotation);
        branch.transform.SetParent(parent);
        return branch;
    }
}
