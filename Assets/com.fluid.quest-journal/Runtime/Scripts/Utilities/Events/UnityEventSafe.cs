using UnityEngine.Events;

namespace CleverCrow.Fluid.QuestJournals.Utilities {
    /// <summary>
    /// Unity events designed for external public class safety. Only allows for basic subscribe and unsubscribe behaviors via IUnityEventReadOnly
    /// </summary>
    [System.Serializable]
    public class UnityEventSafe : UnityEvent, IUnityEventSafe {
    }

    public class UnityEventSafe<T1> : UnityEvent<T1>, IUnityEventSafe<T1> {
    }

    public class UnityEventSafe<T1, T2> : UnityEvent<T1, T2>, IUnityEventSafe<T1, T2> {
    }

    public class UnityEventSafe<T1, T2, T3> : UnityEvent<T1, T2, T3>, IUnityEventSafe<T1, T2, T3> {
    }
}
