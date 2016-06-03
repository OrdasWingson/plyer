using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MassEffectPlyer
{
    public static class Sounds
    {
        public static MediaPlayer clickSound = new MediaPlayer();

        public static void clikSoundField()
        {
            Sounds.clickSound.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "music\\buttClick.mp3"));
            Sounds.clickSound.Play();
        }

        
    }
}
