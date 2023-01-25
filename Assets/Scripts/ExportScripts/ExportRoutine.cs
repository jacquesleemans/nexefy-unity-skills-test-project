using UnityEngine;
using UnityEngine.AddressableAssets;

public class ExportRoutine : MonoBehaviour
{   
    [SerializeField] private SpawnItemList m_itemList = null;

    private AssetReferenceGameObject m_assetLoadedAsset;
    private GameObject m_instanceObject = null;

    //Jacques Change: Added UnityEvent to avoid tight coupling. Note: remember to link up in Editor.
    public UnityEngine.Events.UnityEvent<int, string, GameObject, System.Action<int>> OnModelLoaded;
    //////////////////////////////////////

    private void Start()
    {
        if (m_itemList == null || m_itemList.AssetReferenceCount == 0) 
        {
            Debug.LogError("Spawn list not setup correctly");
        }        
        LoadItemAtIndex(m_itemList, 0);
    }

    private void LoadItemAtIndex(SpawnItemList itemList, int index)
    {
        this.Cleanup();

        //Jacques Change: Prevent out of bounds
        if (index < 0)
        {
            return;
        }

        if (index >= itemList.AssetReferenceCount)
        {
            return;
        }
        //////////////////////////////////////
        
        this.m_assetLoadedAsset = itemList.GetAssetReferenceAtIndex(index);

        //Jacques Change: Prevent Null ref exception
        if (this.m_assetLoadedAsset == null)
        {
            Debug.LogError("m_assetLoadedAsset == null");
            return;
        }
        //////////////////////////////////////
        
        var spawnPosition = new Vector3();
        var spawnRotation = Quaternion.identity;
        var parentTransform = this.transform;
        
        var loadRoutine = this.m_assetLoadedAsset.LoadAssetAsync();
        
        loadRoutine.Completed += LoadRoutine_Completed;

        void LoadRoutine_Completed(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            //Jacques Change: Prevent Null ref exception
            if (obj.Result == null)
            {
                return;
            }
            //////////////////////////////////////
            
            m_instanceObject = Instantiate(obj.Result, spawnPosition, spawnRotation, parentTransform);
            
            this.OnModelLoaded?.Invoke(index, this.m_assetLoadedAsset?.Asset?.name, m_instanceObject, LoadItemAtIndex);
        }
    }

    /// <summary>
    /// Jacques Changes: Added additional calback function
    /// </summary>
    /// <param name="idx"></param>
    public void LoadItemAtIndex(int idx)
    {
        if (idx < 0)
        {
            Debug.LogError("Idx cant be less than zero");
            return;
        }

        if (idx >= this.m_itemList.AssetReferenceCount)
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            return;
#endif
            Debug.LogError("Idx cant be greater than the number of items");
            return;
        }
        
        this.Cleanup();
        
        this.LoadItemAtIndex(this.m_itemList, idx);
    }
    //////////////////////////////////////

    /// <summary>
    /// Cleanup
    /// </summary>
    private void Cleanup()
    {
        if (this.m_assetLoadedAsset != null)
        {
            this.m_assetLoadedAsset.ReleaseAsset();
            this.m_assetLoadedAsset.ReleaseInstance(m_instanceObject);
            
            //Might cause hickups in GC...
            Resources.UnloadUnusedAssets();
        }

        if (this.m_instanceObject != null) 
        {
            //Jacques Change: Editor vs Play Destroy
#if UNITY_EDITOR
            DestroyImmediate(this.m_instanceObject);
#else
    Destroy(m_instanceObject);
#endif  
            //////////////////////////////////////
        }
    }
}
