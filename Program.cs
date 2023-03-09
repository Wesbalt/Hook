using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static Hook.Logger;

namespace Hook
{
    static class Program
    {
        [STAThread]
        private static void Main()
        {
            Logger.Info("Entry point");
            using (var screenManager = new ScreenManager())
                screenManager.Run();
            Logger.Info("Exit point");
        }
    }
}
