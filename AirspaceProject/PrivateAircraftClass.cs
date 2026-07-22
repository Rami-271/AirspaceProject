using System;

namespace AirSimulation
{
    public class PrivateAircraft : Aircraft
    {
        public string OwnerName { get; set; }

        public PrivateAircraft(string flightNumber, bool isEmergency, string ownerName)
            : base(flightNumber, isEmergency)
        {
            OwnerName = ownerName;
        }

        public override string AircraftType => "Private";

        public override string GetDescription()
        {
            return $"{FlightNumber} - Private Aircraft, Owner: {OwnerName}";
        }
    }
}
