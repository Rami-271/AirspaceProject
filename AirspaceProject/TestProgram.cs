using System;
using AirSimulation;
class TestProgram
{
    static void Main()
    {
        List<Aircraft> aircraftList = new List<Aircraft>();
        aircraftList.Add(new CommercialAircraft("1234", false, 180));
        aircraftList.Add(new CargoAircraft("4321", false, 12000));
        aircraftList.Add(new PrivateAircraft("21AB", true, "Jacob"));
        foreach (Aircraft aircraft in aircraftList)
        {
            Console.WriteLine(aircraft.GetDescription());
        }
    }
}
