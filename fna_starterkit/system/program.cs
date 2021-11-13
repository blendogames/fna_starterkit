

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace fna_starterkit
{
    //This is the entry point when the game is run.
    class Program
    {
#if WINDOWS
        [System.Runtime.InteropServices.DllImport("user32.dll")] //Adjust for Windows desktop scaling
        static extern bool SetProcessDPIAware();
#endif

        static void Main(string[] args)
        {

#if WINDOWS
            try
            {
                SetProcessDPIAware(); //Adjust for Windows scale
            }
            catch
            {
            }
#endif

            const string BASEFOLDERNAME = "base";

            string baseFolder = BASEFOLDERNAME;

            foreach (string arg in args)
            {
                if (arg.Equals("--attempt-highdpi"))
                {
                    Environment.SetEnvironmentVariable("FNA_GRAPHICS_ENABLE_HIGHDPI", "1");
                }

                if (arg.Equals("-mod") && args.Length > 1)
                {
                    baseFolder = args[1];
                }
            }

            Globals.baseFolder = baseFolder;

            if (string.Compare(baseFolder, BASEFOLDERNAME, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                string directoryCheck = Path.Combine(Environment.CurrentDirectory, baseFolder);
                if (!Directory.Exists(directoryCheck))
                {
                    Helpers.FatalPopup("Failed to find mod folder:\n'{0}'\n\nPlease make sure this folder exists.", directoryCheck);
                }
            }

            using (Globals.screenManager = new ScreenManager())
            {
#if DEBUG
                Globals.screenManager.Run(); //If debug build, then don't wrap it up in a try/catch block.
#else
                try
                {
                    Globals.screenManager.Run();
                }
                catch (Exception e)
                {
                    Helpers.ErrorPopup(e.ToString()); //If any crash happens, display it to the player.
                }
#endif
            }
        }
    }
}
