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
        private static List<Detective> _detectives = new List<Detective>();
        private static List<string> _whoMoved = new List<string>();

        static void Main(string[] args)
        {
            AddDetectives();
            Console.WriteLine("Current time is " + _time.ToShortTimeString() + ".");
            string _whatHappened = null;
            do
            {
                _whatHappened = "No-one has moved in the last 5 minutes.";
                _whoMoved.RemoveRange(0, _whoMoved.Count);
                IncrementTime();

                foreach (Detective detective in _detectives)
                {
                    if (_timeElapsed % detective.TimeInEachRoom == 0)
                    {
                        MoveDetective(detective);
                    }
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
            Console.WriteLine("Culprit detected! The culprit is... " + _rooms[_detectives.FirstOrDefault().Position] + ". Time Taken to detect Culprit: " + _timeElapsed + " minutes. Press Enter to exit.");
            Console.ReadLine();
        }

        private static void MoveDetective(Detective detective)
        {
            string position = _rooms[detective.Position] + " to ";

            MoveDetectivePosition(detective);

            List<Detective> otherDetectives = _detectives.Where(i => i.Name != detective.Name && _timeElapsed % i.TimeInEachRoom != 0).ToList();

            if (!detective.CanBeInSameRoomAsOtherDetectives && otherDetectives.Any(i => i.Position == detective.Position) && HolmesExtraCondition(detective))
                MoveDetectivePosition(detective);

            position += _rooms[detective.Position];
            _whoMoved.Add(detective.Name + " moved from " + position);

            if (detective.Name == "Holmes")
                CheckForCulprit(detective);
        }

        private static bool HolmesExtraCondition(Detective detective)
        {
            if (detective.Name == "Holmes" && _detectives.All(i => i.Position == detective.Position))
                return false;
            else
                return true;
        }

        private static void MoveDetectivePosition(Detective detective)
        {
            if (!detective.IsClockwise)
            {
                if (detective.Position == 0)
                    detective.Position = 5;
                else
                    detective.Position--;
            }
            else
            {
                if (detective.Position == 5)
                    detective.Position = 0;
                else
                    detective.Position++;
            }
        }

        private static void AddDetectives()
        {
            _detectives.Add(new Detective
            {
                Name = "Holmes",
                IsClockwise = true,
                Position = 0,
                TimeInEachRoom = 15,
                CanBeInSameRoomAsOtherDetectives = false
            });

            _detectives.Add(new Detective
            {
                Name = "Watson",
                IsClockwise = false,
                Position = 5,
                TimeInEachRoom = 20,
                CanBeInSameRoomAsOtherDetectives = false
            });

            _detectives.Add(new Detective
            {
                Name = "Wellington",
                IsClockwise = false,
                Position = 0,
                TimeInEachRoom = 30,
                CanBeInSameRoomAsOtherDetectives = true
            });
            
            _detectives = _detectives.OrderByDescending(i => i.TimeInEachRoom).ToList();
        }

        private static void IncrementTime(int minutesToIncrement = 5)
        {
            _timeElapsed += minutesToIncrement;
            _time = _time.AddMinutes(minutesToIncrement);
        }

        private static void CheckForCulprit(Detective detective)
        {
            _isCulpritDetected = _detectives.All(i => i.Position == detective.Position);
        }
    }
}
