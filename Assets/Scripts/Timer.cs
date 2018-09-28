using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Timer
{
    public readonly float MaxTime;

    private float time;
    public float TimeLeft
    {
        get
        {
            return time;
        }
    }

    public Timer(float max_time)
    {
        MaxTime = max_time;
    }

    public bool tick(float delta)
    {
        time -= delta;
        return time <= 0;
    }

    public void Reset()
    {
        time = MaxTime;
    }
}
