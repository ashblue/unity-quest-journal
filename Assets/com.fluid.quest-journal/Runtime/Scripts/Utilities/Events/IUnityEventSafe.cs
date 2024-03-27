namespace CleverCrow.Fluid.QuestJournals.Utilities {
    public interface IUnityEventSafe : IUnityEventReadOnly {
        void Invoke ();
    }

    public interface IUnityEventSafe<T> : IUnityEventReadOnly<T> {
        void Invoke (T arg);
    }

    public interface IUnityEventSafe<T1, T2> : IUnityEventReadOnly<T1, T2> {
        void Invoke (T1 arg1, T2 arg2);
    }

    public interface IUnityEventSafe<T1, T2, T3> : IUnityEventReadOnly<T1, T2, T3> {
        void Invoke (T1 arg1, T2 arg2, T3 arg3);
    }
}
