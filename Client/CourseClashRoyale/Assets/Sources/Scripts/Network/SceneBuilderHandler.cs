using Unity.Netcode;
using UnityEngine;

public class SceneBuilderHandler : NetworkBehaviour
{
    private NetworkSceneBuilder _sceneBuilder;

    private void Start()
    {
        GameSceneDependency gameEntryPoint = FindObjectOfType<GameSceneDependency>();
        _sceneBuilder = GetComponent<NetworkSceneBuilder>();
        gameEntryPoint.InitSceneBuilder(_sceneBuilder);

        if (IsServer)
            return;

        Debug.Log(_sceneBuilder);

        BuildLevelServerRpc(_sceneBuilder);
    }

    [ServerRpc(RequireOwnership = false)]
    private void BuildLevelServerRpc(NetworkBehaviourReference networkBehaviourReference)
    {
        networkBehaviourReference.TryGet(out NetworkBehaviour networkBehaviour);

        //networkBehaviour.GetComponent<NetworkSceneBuilder>().Build();
        //networkBehaviour.GetComponent<NetworkSceneBuilder>().CreateTowers();

        Debug.Log("spawn");
        _sceneBuilder.Build();
        _sceneBuilder.CreateTowers();
    }
}
