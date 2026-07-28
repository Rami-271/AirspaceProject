using System;
using System.Collections.Generic;
using AirSimulation;

class TestProgram
{
    static void Main()
    {
        Airport airport = new Airport("Airspace Training Airport");

        airport.AddRunway(new Runway("1"));
        airport.AddRunway(new Runway("2"));

        airport.AddGate(new Gate("A1"));
        airport.AddGate(new Gate("A2"));
        airport.AddGate(new Gate("A3"));

        bool exitProgram = false;

        while (!exitProgram)
        {
            DisplayMenu(airport);

            try
            {
                Console.Write("Enter an option: ");
                int option = int.Parse(Console.ReadLine());
                Console.WriteLine();

                switch (option)
                {
                    case 1:
                        AddArrivalFlight(airport);
                        break;
                    case 2:
                        AddDepartureFlight(airport);
                        break;
                    case 3:
                        PrepareFlightForDeparture(airport);
                        break;
                    case 4:
                        ProcessNextFlight(airport);
                        break;
                    case 5:
                        DisplayQueues(airport);
                        break;
                    case 6:
                        DisplayResources(airport);
                        break;
                    case 7:
                        DisplayFlights(airport);
                        break;
                    case 8:
                        DisplayEventLog(airport);
                        break;
                    case 9:
                        LoadSampleFlights(airport);
                        break;
                    case 0:
                        exitProgram = true;
                        Console.WriteLine("Simulation ended.");
                        break;
                    default:
                        Console.WriteLine("Enter a number from 0 to 9.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Enter a valid number.");
            }
            catch (ArgumentException error)
            {
                Console.WriteLine($"Input error: {error.Message}");
            }
            catch (InvalidOperationException error)
            {
                Console.WriteLine($"Scheduling error: {error.Message}");
            }

            Console.WriteLine();
        }
    }

    static void DisplayMenu(Airport airport)
    {
        Console.WriteLine("========================================");
        Console.WriteLine(airport.AirportName);
        Console.WriteLine("Airport Runway and Gate Simulator");
        Console.WriteLine("========================================");
        Console.WriteLine("1. Add arrival flight");
        Console.WriteLine("2. Add departure flight");
        Console.WriteLine("3. Prepare landed flight for departure");
        Console.WriteLine("4. Process next flight");
        Console.WriteLine("5. View flight queues");
        Console.WriteLine("6. View runways and gates");
        Console.WriteLine("7. View all flights");
        Console.WriteLine("8. View event log");
        Console.WriteLine("9. Load sample flights");
        Console.WriteLine("0. Exit");
        Console.WriteLine("========================================");
    }

    static void AddArrivalFlight(Airport airport)
    {
        Aircraft aircraft = CreateAircraft();

        if (airport.HasFlight(aircraft.FlightNumber))
        {
            throw new ArgumentException(
                "A flight with that flight number already exists.");
        }

        Flight flight = new Flight(aircraft, FlightType.Arrival);
        airport.AddFlight(flight);

        Console.WriteLine(
            $"Arrival flight {aircraft.FlightNumber} was added.");
    }

    static void AddDepartureFlight(Airport airport)
    {
        Aircraft aircraft = CreateAircraft();

        if (airport.HasFlight(aircraft.FlightNumber))
        {
            throw new ArgumentException(
                "A flight with that flight number already exists.");
        }

        Console.Write("Enter departure gate number: ");
        string gateNumber = Console.ReadLine();

        Flight flight = new Flight(aircraft, FlightType.Departure);

        airport.AssignDepartureGate(flight, gateNumber);
        airport.AddFlight(flight);

        Console.WriteLine(
            $"Departure flight {aircraft.FlightNumber} was added.");
    }

    static void PrepareFlightForDeparture(Airport airport)
    {
        Console.Write("Enter the landed flight number: ");
        string flightNumber = Console.ReadLine();

        Flight flight = airport.GetFlight(flightNumber);
        airport.PrepareFlightForDeparture(flight);

        Console.WriteLine(
            $"Flight {flight.Aircraft.FlightNumber} entered the " +
            "departure queue.");
    }

    static void ProcessNextFlight(Airport airport)
    {
        bool flightProcessed = airport.ProcessNextFlight();

        if (flightProcessed)
        {
            Console.WriteLine("The next flight was processed.");
        }
        else
        {
            Console.WriteLine("No flight could be processed.");
        }
    }

    static Aircraft CreateAircraft()
    {
        Console.WriteLine("Aircraft Types");
        Console.WriteLine("1. Commercial aircraft");
        Console.WriteLine("2. Cargo aircraft");
        Console.WriteLine("3. Private aircraft");
        Console.Write("Select an aircraft type: ");
        int aircraftType = int.Parse(Console.ReadLine());

        Console.Write("Enter flight number: ");
        string flightNumber = Console.ReadLine();

        Console.Write("Is this an emergency flight? (Y/N): ");
        string emergencyAnswer = Console.ReadLine().Trim().ToUpper();

        if (emergencyAnswer != "Y" && emergencyAnswer != "N")
        {
            throw new ArgumentException(
                "Emergency selection must be Y or N.");
        }

        bool isEmergency = emergencyAnswer == "Y";

        switch (aircraftType)
        {
            case 1:
                Console.Write("Enter passenger count: ");
                int passengerCount = int.Parse(Console.ReadLine());

                return new CommercialAircraft(flightNumber,
                    isEmergency, passengerCount);

            case 2:
                Console.Write("Enter cargo weight in pounds: ");
                double cargoWeight = double.Parse(Console.ReadLine());

                return new CargoAircraft(flightNumber,
                    isEmergency, cargoWeight);

            case 3:
                Console.Write("Enter owner name: ");
                string ownerName = Console.ReadLine();

                return new PrivateAircraft(flightNumber,
                    isEmergency, ownerName);

            default:
                throw new ArgumentException(
                    "Aircraft type must be 1, 2, or 3.");
        }
    }

    static void DisplayQueues(Airport airport)
    {
        DisplayQueue("Landing Queue",
            airport.Scheduler.GetLandingQueue());
        Console.WriteLine();

        DisplayQueue("Departure Queue",
            airport.Scheduler.GetDepartureQueue());
        Console.WriteLine();

        DisplayQueue("Waiting for Gate",
            airport.Scheduler.GetGateWaitingQueue());
    }

    static void DisplayQueue(string queueName, Flight[] flights)
    {
        Console.WriteLine(queueName);
        Console.WriteLine("------------------------------");

        if (flights.Length == 0)
        {
            Console.WriteLine("No flights in this queue.");
            return;
        }

        int position = 1;

        foreach (Flight flight in flights)
        {
            Console.WriteLine($"{position}. {flight.GetDescription()}");
            position++;
        }
    }

    static void DisplayResources(Airport airport)
    {
        Console.WriteLine("Runways");
        Console.WriteLine("------------------------------");

        foreach (Runway runway in airport.GetRunways())
        {
            Console.WriteLine(runway.GetDescription());
        }

        Console.WriteLine();
        Console.WriteLine("Gates");
        Console.WriteLine("------------------------------");

        foreach (Gate gate in airport.GetGates())
        {
            Console.WriteLine(gate.GetDescription());
        }
    }

    static void DisplayFlights(Airport airport)
    {
        Flight[] flights = airport.GetFlights();

        Console.WriteLine("All Flights");
        Console.WriteLine("------------------------------");

        if (flights.Length == 0)
        {
            Console.WriteLine("No flights have been added.");
            return;
        }

        foreach (Flight flight in flights)
        {
            Console.WriteLine(flight.GetDescription());
        }
    }

    static void DisplayEventLog(Airport airport)
    {
        string[] events = airport.Scheduler.GetEventLog();

        Console.WriteLine("Event Log");
        Console.WriteLine("------------------------------");

        if (events.Length == 0)
        {
            Console.WriteLine("No events have been recorded.");
            return;
        }

        int eventNumber = 1;

        foreach (string eventMessage in events)
        {
            Console.WriteLine($"{eventNumber}. {eventMessage}");
            eventNumber++;
        }
    }

    static void LoadSampleFlights(Airport airport)
    {
        if (airport.GetFlights().Length > 0)
        {
            throw new InvalidOperationException(
                "Sample flights can only be loaded into an empty simulation.");
        }

        Flight departure1 = new Flight(
            new CommercialAircraft("DP505", false, 120),
            FlightType.Departure);

        Flight departure2 = new Flight(
            new CargoAircraft("DP606", false, 5000),
            FlightType.Departure);

        airport.AssignDepartureGate(departure1, "A1");
        airport.AddFlight(departure1);

        airport.AssignDepartureGate(departure2, "A2");
        airport.AddFlight(departure2);

        airport.AddFlight(new Flight(
            new CommercialAircraft("CM101", false, 150),
            FlightType.Arrival));

        airport.AddFlight(new Flight(
            new CargoAircraft("CG202", false, 8000),
            FlightType.Arrival));

        airport.AddFlight(new Flight(
            new PrivateAircraft("PV303", true, "Taylor"),
            FlightType.Arrival));

        airport.AddFlight(new Flight(
            new CommercialAircraft("CM404", false, 90),
            FlightType.Arrival));

        Console.WriteLine("Six sample flights were loaded.");
        Console.WriteLine(
            "PV303 is an emergency arrival and should be first " +
            "in the landing queue.");
    }
}
