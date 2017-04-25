using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holmes
{
    class Program
    {
        private static string[] _rooms = new string[] { "Mustard", "Plum", "Green", "Peacock", "Scarlett", "White" };
        private static int _timeElapsed = 0;
        private static DateTime _time = DateTime.Now;
        private static bool _isCulpritDetected = false;
        private static short _holmesPosition = 0;
        private static short _watsonPosition = 5;
        private static short _wellingtonPosition = 0;
        private static List<string> _whoMoved = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("Current time is " + _time.ToShortTimeString() + ".");
            string _whatHappened = null;
            do
            {
                _whatHappened = "No-one has moved in the last 5 minutes.";
                _whoMoved.RemoveRange(0, _whoMoved.Count);
                IncrementTime();

                if (_timeElapsed % 20 == 0)
                {
                    MoveWatson();
                }

                if (_timeElapsed % 30 == 0)
                {
                    MoveWellington();
                }

                if (_timeElapsed % 15 == 0)
                {
                    MoveHolmes();
                }
                if (_whoMoved.Count > 0)
                {
                    _whatHappened = string.Join(",", _whoMoved);
                    if (_whoMoved.Count > 1)
                        _whatHappened = _whatHappened.Remove(_whatHappened.LastIndexOf(","), 1).Insert(_whatHappened.LastIndexOf(","), " and ");
                }

                Console.WriteLine(_whatHappened);
                Console.WriteLine("Current time is " + _time.ToShortTimeString() + ". Time elapsed:" + _timeElapsed + " minutes.");
            } while (!_isCulpritDetected);
            Console.WriteLine("Culprit detected! The culprit is... " + _rooms[_holmesPosition] + ". Time Taken to detect Culprit: " + _timeElapsed + " minutes. Press Enter to exit.");
            Console.ReadLine();
        }

        private static void MoveWellington()
        {
            string position = _rooms[_wellingtonPosition] + " to ";

            if (_wellingtonPosition == 0)
                _wellingtonPosition = 5;
            else
                _wellingtonPosition--;

            position += _rooms[_wellingtonPosition];
            _whoMoved.Add("Wellington moved from " + position);
        }

        private static void MoveWatson()
        {
            string position = _rooms[_watsonPosition] + " to ";

            MoveWatsonsPosition();

            if (_watsonPosition == _holmesPosition && _timeElapsed % 15 != 0)
            {
                MoveWatsonsPosition();
            }

            position += _rooms[_watsonPosition];

            _whoMoved.Add("Watson moved from " + position);
        }

        private static void MoveWatsonsPosition()
        {
            if (_watsonPosition == 0)
                _watsonPosition = 5;
            else
                _watsonPosition--;
        }

        private static void MoveHolmes()
        {
            string position = _rooms[_holmesPosition] + " to ";

            MoveHolmesPosition();

            if (_watsonPosition == _holmesPosition && _timeElapsed % 20 != 0 && (_watsonPosition != _wellingtonPosition && _timeElapsed % 30 != 0))
            {
                MoveHolmesPosition();
            }

            position += _rooms[_holmesPosition];

            _whoMoved.Add("Holmes moved from " + position);

            CheckForCulprit();
        }

        private static void MoveHolmesPosition()
        {
            if (_holmesPosition == 5)
                _holmesPosition = 0;
            else
                _holmesPosition++;
        }

        private static void IncrementTime(int minutesToIncrement = 5)
        {
            _timeElapsed += minutesToIncrement;
            _time = _time.AddMinutes(_timeElapsed);
        }

        private static void CheckForCulprit()
        {
            _isCulpritDetected = (_holmesPosition == _watsonPosition && _holmesPosition == _wellingtonPosition);
        }
    }
}
