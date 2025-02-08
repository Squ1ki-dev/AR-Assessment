using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Code.Services
{
    public class AddressableAssetProvider : IAssetLoader
    {
        public async UniTask<GameObject> LoadAssetAsync(string assetName)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(assetName);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result;
            else
            {
                Debug.LogError($"Failed to load asset with name {assetName}");
                return null;
            }
        }

        public void ReleaseAsset(GameObject asset)
        {
            Addressables.ReleaseInstance(asset);
        }
    }
}
