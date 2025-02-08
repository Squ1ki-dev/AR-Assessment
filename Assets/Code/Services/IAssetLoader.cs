using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Services
{
    public interface IAssetLoader
    {
        UniTask<GameObject> LoadAssetAsync(string assetName);
        void ReleaseAsset(GameObject asset);
    }
}

