using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lib.Common.Management
{
    public static class SoundMain
    {
        public static void PlayMessageBox()
        {
            new Thread(new ThreadStart(delegate {

                SoundPlayer MessageBoxSound = new SoundPlayer(Properties.Resources.Windows_Logon_Sound);
                MessageBoxSound.Play();

            })).Start();
            
        }
    }
}
