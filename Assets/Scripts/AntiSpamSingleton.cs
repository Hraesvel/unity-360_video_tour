using System;

public sealed class AntiSpamSingleton : IDisposable
{
    private static AntiSpamSingleton _instance;
    private static readonly object padlock = new object();

    public AntiSpamSingleton()
    {
        IsTransitioning = false;
    }

    public static bool IsActive => _instance != null;

    public static AntiSpamSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null)
                {
                    _instance = new AntiSpamSingleton();
                    _instance.IsTransitioning = false;
                }
            }

            return _instance;
        }
    }

    public bool IsTransitioning { get; set; }

    public Controls PlayerCtrl { get; set; }


    public void Dispose()
    {
        _instance = null;
    }
}