using System.Collections.Generic;

public class EventManager : CoreComponent
{
    private Dictionary<E_Event, List<IListener>> _eventsTable = null;

    protected override void InitOptions()
    {
        _eventsTable = new Dictionary<E_Event, List<IListener>>();
    }

    public void AddListener(E_Event m_eventType, IListener m_listener)
    {
        if (_eventsTable.ContainsKey(m_eventType) == false)
            _eventsTable.Add(m_eventType, new List<IListener>());

        _eventsTable[m_eventType].Add(m_listener);
    }

    public void PlayEvent(E_Event m_eventType)
    {
        if (_eventsTable.ContainsKey(m_eventType) == false ||
            _eventsTable[m_eventType].Count <= 0)
        { throw new System.Exception($"{m_eventType} 이벤트 접근 문제발생"); }

        foreach (var listener in _eventsTable[m_eventType])
        {
            listener.OnEvent(m_eventType);
        }
    }

    public void RemoveListener(E_Event m_event, IListener m_listener)
    {
        _eventsTable[m_event].Remove(m_listener);
    }

    private void RefreshNulls()
    {
        foreach (var e in _eventsTable)
        {
            RefreshNulls(e.Key);
        }
    }

    private void RefreshNulls(E_Event m_eventType)
    {
        List<IListener> list = _eventsTable[m_eventType];

        for (int i = list.Count-1; i >= 0; i--)
        {
            if (list[i] == null)
                list.RemoveAt(i);
        }
    }
}
