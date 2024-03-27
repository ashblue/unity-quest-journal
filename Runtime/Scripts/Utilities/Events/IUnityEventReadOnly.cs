using UnityEngine.Events;

namespace CleverCrow.Fluid.QuestJournals.Utilities {
    public interface IUnityEventReadOnly {
        void AddListener (UnityAction call);
        void RemoveListener (UnityAction call);
    }

    public interface IUnityEventReadOnly<T> {
        void AddListener (UnityAction<T> call);
        void RemoveListener (UnityAction<T> call);
    }

    public interface IUnityEventReadOnly<T1, T2> {
        void AddListener (UnityAction<T1, T2> call);
        void RemoveListener (UnityAction<T1, T2> call);
    }

    public interface IUnityEventReadOnly<T1, T2, T3> {
        void AddListener (UnityAction<T1, T2, T3> call);
        void RemoveListener (UnityAction<T1, T2, T3> call);
    }
}
