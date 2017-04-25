using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holmes
{
    public class Detective
    {
        public string Name { get; set; }
        public int TimeInEachRoom { get; set; }
        public bool IsClockwise { get; set; }
        public short Position { get; set; }
        public bool CanBeInSameRoomAsOtherDetectives { get; set; }
    }
}
