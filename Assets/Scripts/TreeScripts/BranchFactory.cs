using UnityEngine;

public class BranchFactory : IFactory
{
    private const string BranchPath = "BranchLevel";

    private Branch _branchPrefab;
    private int _branchLevel;

    public BranchFactory(int branchLevel)
    {
        _branchLevel = branchLevel;
    }

    public void LoadResources()
    {

        _branchPrefab = Resources.Load<Branch>(BranchPath + _branchLevel);
    }

    public MonoBehaviour Create(Vector2 position, Quaternion rotation, Transform parent)
    {
        Branch branch = UnityEngine.Object.Instantiate(_branchPrefab, position, rotation);
        branch.transform.SetParent(parent);
        return branch;
    }
}
