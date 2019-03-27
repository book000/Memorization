using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorization
{
    public class Settings
    {
        private string _speaker;
        private int _volume;
        private int _speed;
        private int _pitch;
        private string _apikey;

        public string Speaker
        {
            get { return _speaker; }
            set { _speaker = value; }
        }

        public int Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public int Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }

        public string Apikey
        {
            get { return _apikey; }
            set { _apikey = value; }
        }

        public Settings()
        {
            _speaker = "show";
            _volume = 50;
            _speed = 120;
            _pitch = 120;
            _apikey = "ENTER YOUR APIKEY HERE";
        }
    }
}
