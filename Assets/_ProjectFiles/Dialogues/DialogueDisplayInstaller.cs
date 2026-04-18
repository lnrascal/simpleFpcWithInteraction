using UnityEngine;
using Zenject;

public class MainSceneInstaller : MonoInstaller
{
    [SerializeField] private FPController fpController;
    [SerializeField] private DialogueDisplay dialogueDisplay;
    [SerializeField] private QuestManager questManager;
    public override void InstallBindings() {
        Container.Bind<FPController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<DialogueDisplay>().FromComponentInHierarchy().AsSingle();
        Container.Bind<QuestManager>().FromComponentInHierarchy().AsSingle();
    }
}
