using System;
using System.Collections.Generic;
using UnityEngine;

class _TimeManagerSample : MonoBehaviour
{
    private void Start()
    {
        // Start a global timer for 10 seconds
        TimeManager._instance._SetNewTimer(_TimerNames.G_CoinAdTimer, 10f, true);

        // Check timer status
        _TimerStatus status = TimeManager._instance._GetTimerStatus(_TimerNames.G_CoinAdTimer);
        Debug.Log("Timer Status: " + status);

        // Get remaining seconds
        double remaining = TimeManager._instance._GetTimerRemainingSec(_TimerNames.G_CoinAdTimer);
        Debug.Log("Time Remaining: " + remaining);

        // Get formatted string for display
        string timerText = TimeManager._instance._GetStringTimerText(_TimerNames.G_CoinAdTimer);
        Debug.Log("Timer Text: " + timerText);
    }
}

/// <summary>
/// Manages multiple timers with global or local scope:
/// - Global timers persist across sessions and are saved/loaded automatically.
/// - Local timers reset when the scene or application starts.
/// 
/// Use _SetNewTimer to start a timer, _GetTimerStatus to check its state,
/// _GetTimerRemainingSec to get the remaining seconds, and _GetStringTimerText to get a formatted string.
/// </summary>
public class TimeManager : Singleton_Abs<TimeManager>
{
    private List<_TimerData> _allTimersData = new List<_TimerData>();

    private bool _haveTimersLoaded; // to avoid using this script when its not loaded yet

    private void Start()
    {
        DontDestroyOnLoad(transform.root);

        _CheckTimersLoaded();
    }
    public void _SetNewTimer(_TimerNames iName, float iDuration, bool iIsGlobal = true)
    {
        _CheckTimersLoaded();

        /// this is to avoid cases like : 4.999 being shown as 4 instead of 5 at the start
        /// the timer status is usually called right after creating a timer but there is 
        /// some milliseconds difference that cause us problems
        iDuration += 0.1f;

        DateTime iStartTime = DateTime.UtcNow;
        DateTime iEndTime = iStartTime.AddSeconds(iDuration);

        _TimerData timer = _allTimersData.Find(t => t._name == iName);
        if (timer != null)
            _allTimersData.Remove(timer);

        _TimerData._TimerType iType = iIsGlobal ? _TimerData._TimerType.Global : _TimerData._TimerType.Local;
        _allTimersData.Add(new _TimerData(iName, iStartTime, iEndTime, iType));
        _SaveData();
    }
    public _TimerStatus _GetTimerStatus(_TimerNames iName)
    {
        _CheckTimersLoaded();

        _TimerData timer = _allTimersData.Find(t => t._name == iName);

        if (timer == null)
            return _TimerStatus.NotFound;

        if (DateTime.UtcNow >= timer._EndTimeUtc)
            return _TimerStatus.Finished;
        else
            return _TimerStatus.InProgress;
    }
    public double _GetTimerRemainingSec(_TimerNames iName)
    {
        _CheckTimersLoaded();

        _TimerData timer = _allTimersData.Find(t => t._name == iName);

        if (timer == null)
            return -1;

        if (DateTime.UtcNow >= timer._EndTimeUtc)
            return 0;
        else
            return (timer._EndTimeUtc - DateTime.UtcNow).TotalSeconds;
    }

    /// <summary>
    /// change the Time Format as you see fit
    /// </summary>
    public string _GetStringTimerText(_TimerNames iName)
    {
        _CheckTimersLoaded();

        double remainingSec = _GetTimerRemainingSec(iName);
        if (remainingSec <= 0)
            return "0";

        TimeSpan ts = TimeSpan.FromSeconds(remainingSec);

        if (ts.Hours > 0)
            return string.Format("{0}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
        //else if (ts.Minutes > 0)
        return string.Format("{0}:{1:00}", ts.Minutes, ts.Seconds);
        //else
        //return string.Format("{0}", ts.Seconds.ToString("00"));
    }
    private void _CheckTimersLoaded()
    {
        if (!_haveTimersLoaded)
        {
            _LoadData();
            _haveTimersLoaded = true;
        }
    }

    #region Save/Load
    [CreateMonoButton("Save Data")]
    public void _SaveData()
    {
        foreach(var item in _allTimersData)
            if (item._timerType == _TimerData._TimerType.Local)
                _allTimersData.Remove(item);

        SaveTools._SaveListToDisk(ref _allTimersData, A.DataKey.timersData);
    }
    public void _LoadData()
    {
        SaveTools._LoadListFromDisk(ref _allTimersData, A.DataKey.timersData);
    }
    #endregion

    [Serializable]
    private class _TimerData
    {
        public _TimerNames _name;
        public long _startTimeTicks;
        public long _endTimeTicks;
        public _TimerType _timerType;

        public _TimerData(_TimerNames iName, DateTime iStartTime, DateTime iEndTime, _TimerType iTimerType)
        {
            _name = iName;
            _StartTimeUtc = iStartTime;
            _EndTimeUtc = iEndTime;
            _timerType = iTimerType;
        }
        public DateTime _StartTimeUtc
        {
            get { return new DateTime(_startTimeTicks, DateTimeKind.Utc); }
            set { _startTimeTicks = value.Ticks; }
        }
        public DateTime _EndTimeUtc
        {
            get { return new DateTime(_endTimeTicks, DateTimeKind.Utc); }
            set { _endTimeTicks = value.Ticks; }
        }
        public enum _TimerType
        {
            Global, Local
        }
    }
}
// Optional Naming : G => Global , L => Local
public enum _TimerNames
{
    G_CoinAdTimer,  // example: cooldown for showing rewarded ads to get coins
    L_IntraAdTimer  // example: cooldown for showing new intra ads on entering the game
}
public enum _TimerStatus
{
    NotFound, Finished, InProgress
}
