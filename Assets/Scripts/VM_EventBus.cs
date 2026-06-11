using System;
using System.Collections.Generic;

public class VM_EventBus
{
    // 제작 순서 EventBus->Data->Engine->UI->Manager

    // 타입별 핸들러 저장소
    private Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate>();

    // 구독(subscribe)
    public void Subscribe<T>(Action<T> handler)
    {
        Type key = typeof(T);

        if (handlers.ContainsKey(key))
            handlers[key] = Delegate.Combine(handlers[key], handler);
        else
            handlers[key] = handler;
    }

    // 구독 해제 (Unsubscribe)
    public void Unsubscribe<T>(Action<T> handler)
    {
        Type key = typeof(T);

        if (handlers.ContainsKey(key))
        {
            handlers[key] = Delegate.Remove(handlers[key], handler);
            if (handlers[key] == null)
            {
                handlers.Remove(key);
            }
        }
    }

    // 발행 — 해당 타입 구독자 전체에게 전달 (Publish)
    public void Publish<T>(T message)
    {
        Type key = typeof(T);

        if (handlers.ContainsKey(key))
        {
            // handlers[key]의 type은 delegate
            ((Action<T>)handlers[key])?.Invoke(message);
        }
    }
}
