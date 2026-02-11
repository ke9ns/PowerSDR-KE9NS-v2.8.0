//=================================================================
// CWPTT.cs
//=================================================================
// PowerSDR is a C# implementation of a Software Defined Radio.
// KE9NS This was extracted from FlexCW.dll

#region Assembly FlexCW, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// C:\Users\RADIO\source\PowerSDR_v2.8.0\Source\Console\FlexCW.dll
// Decompiled with ICSharpCode.Decompiler 8.2.0.7535
#endregion


//using FlexCW;
using System;
using System.Collections.Generic;
using System.Diagnostics;
//using Multimedia;

namespace PowerSDR //FlexCW;
{
    public class CWPTT
    {
        public delegate void MoxCallback(bool val);

        public delegate void MuteCallback(bool val);

        private static MMTimer timer = new MMTimer();

        public static event MoxCallback MoxChanged;

        public static event MuteCallback MuteChanged;

        private static void OnMoxChanged(bool b)
        {
            CWPTT.MoxChanged?.Invoke(b);
        }

        private static void OnMuteChanged(bool b)
        {
            CWPTT.MuteChanged?.Invoke(b);
        }

        public static void Init()
        {
            timer.Mode = TimerMode.Periodic;
            timer.Period = 1;
            timer.Resolution = 0;
            timer.Tick += timer_Tick;
        }

        public static void Start()
        {
            timer.Start();
        }

        public static void Stop()
        {
            timer.Stop();
        }

        private static void timer_Tick(object sender, EventArgs e)
        {
            double currentTime = CWSensorItem.GetCurrentTime();
            CWKeyer.Advance(currentTime);
            if (CWKeyer.PTTQueueCount() > 0)
            {
                CWPTTItem cWPTTItem = CWKeyer.PTTQueuePeek();
                if (currentTime > cWPTTItem.Time)
                {
                    CWKeyer.PTTDequeue();
                    if (!cWPTTItem.Ignore)
                    {
                        OnMoxChanged(cWPTTItem.State);
                    }
                }
            }

            if (CWKeyer.MuteQueueCount() > 0)
            {
                CWMuteItem cWMuteItem = CWKeyer.MuteQueuePeek();
                if (currentTime > cWMuteItem.Time)
                {
                    CWKeyer.MuteDequeue();
                    OnMuteChanged(cWMuteItem.MOX);
                }
            }
        }
    }
}
#if false // Decompilation log
'170' items in cache
------------------
Resolve: 'mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
------------------
Resolve: 'System, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
WARN: Version mismatch. Expected: '2.0.0.0', Got: '4.0.0.0'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\System.dll'
------------------
Resolve: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Found single assembly: 'mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089'
Load from: 'C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.8\mscorlib.dll'
#endif
