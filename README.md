# Airspace Project

## Airport Runway and Gate Simulator

We created this C# console application to simulate basic airport runway and gate scheduling. The program allows users to add arrival and departure flights, process flights through queues, assign available runways and gates, give emergency arrivals priority, and view the status of each flight.

The project focuses on runway and gate scheduling rather than trying to model a complete air traffic control system.

## Features

- Add commercial, cargo, and private aircraft
- Add arrival and departure flights
- Give emergency arrivals priority
- Keep normal flights in first-in, first-out order
- Assign available runways to flights
- Assign available gates to landed aircraft
- Place aircraft in a waiting queue when all gates are occupied
- Release a gate when a flight departs
- Prepare a landed arrival for departure
- View landing, departure, and gate-waiting queues
- View runway and gate availability
- View all flights and their current status
- View a chronological event log
- Load six sample flights for testing
- Reject invalid or duplicate flight information

## Object-Oriented Programming Concepts

### Abstraction

`Aircraft` is an abstract base class that contains information shared by all aircraft.

### Inheritance

The following classes inherit from `Aircraft`:

- `CommercialAircraft`
- `CargoAircraft`
- `PrivateAircraft`

Each derived class contains information for its specific aircraft type.

### Polymorphism

Each aircraft class overrides `AircraftType` and `GetDescription()`. The program can use an `Aircraft` reference while calling the correct method for the actual aircraft object.

### Encapsulation

The classes use private fields, properties, constructors, and methods to control how data is stored and changed. Input validation prevents invalid values from being added.

### Composition

The `Airport` class contains flights, runways, gates, a scheduler, and a conflict detector. A `Flight` object also contains an `Aircraft` object.

## Main Classes

| Class | Purpose |
|---|---|
| `Aircraft` | Abstract base class for common aircraft information |
| `CommercialAircraft` | Stores commercial aircraft passenger information |
| `CargoAircraft` | Stores cargo aircraft weight information |
| `PrivateAircraft` | Stores private aircraft owner information |
| `Flight` | Stores an aircraft, flight type, status, runway, and gate |
| `Airport` | Coordinates flights, runways, gates, and scheduling |
| `Scheduler` | Manages the landing, departure, and gate-waiting queues |
| `Runway` | Tracks runway availability and assigned flights |
| `Gate` | Tracks gate availability and assigned flights |
| `ConflictDetector` | Checks resource availability and scheduling conditions |
| `TestProgram` | Contains the console menu and user interaction |

## Requirements

- Visual Studio
- .NET 10 SDK
- C# console application support

No third-party libraries are required.

## Project Structure

```text
AirspaceProject/
|-- AirspaceProject.slnx
|-- README.md
|-- .gitignore
|-- .gitattributes
|
`-- AirspaceProject/
    |-- AirspaceProject.csproj
    |-- AircraftClass.cs
    |-- AirportClass.cs
    |-- CargoAircraftClass.cs
    |-- CommercialAircraftClass.cs
    |-- ConflictDetectorClass.cs
    |-- FlightClass.cs
    |-- FlightStatus.cs
    |-- FlightType.cs
    |-- GateClass.cs
    |-- PrivateAircraftClass.cs
    |-- RunwayClass.cs
    |-- Scheduler.cs
    `-- TestProgram.cs
```

## How to Build and Run

1. Clone or download the repository.
2. Open `AirspaceProject.slnx` in Visual Studio.
3. Select **Build > Rebuild Solution**.
4. Confirm that the build completes with zero errors.
5. Press **Ctrl + F5** to run the program.

The project can also be opened directly using:

```text
AirspaceProject/AirspaceProject.csproj
```

## Console Menu

```text
1. Add arrival flight
2. Add departure flight
3. Prepare landed flight for departure
4. Process next flight
5. View flight queues
6. View runways and gates
7. View all flights
8. View event log
9. Load sample flights
0. Exit
```

## Testing the Program

The built-in sample simulation is the easiest way to test the main features.

1. Run the program.
2. Enter `9` to load six sample flights.
3. Enter `5` to view the flight queues.
4. Confirm that emergency flight `PV303` is first in the landing queue.
5. Enter `4` to process the next flight.
6. Continue entering `4` to process the remaining flights.
7. Enter `6` to view runway and gate assignments.
8. Enter `7` to view all flight statuses.
9. Enter `8` to view the event log.

The sample flights are:

| Flight | Aircraft Type | Flight Type |
|---|---|---|
| DP505 | Commercial | Departure |
| DP606 | Cargo | Departure |
| CM101 | Commercial | Arrival |
| CG202 | Cargo | Arrival |
| PV303 | Private Emergency | Arrival |
| CM404 | Commercial | Arrival |

The expected landing queue order is:

```text
PV303
CM101
CG202
CM404
```

This shows that emergency flights receive priority while normal arrivals remain in first-in, first-out order.

## Team Members

- Rami
- Jaskaran
- Melissa
- Marcos

## Limitations

- The program uses a sequential simulation instead of real scheduled times.
- It prevents a runway or gate from being used while occupied, but it does not compare future time intervals.
- The airport starts with two runways and three gates.
- Simulation information is not saved after the program closes.
- The program uses a console interface instead of a graphical user interface.
