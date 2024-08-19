// See https://aka.ms/new-console-template for more information

using ElevatorApp;

var elevator = new Elevator();


Console.WriteLine($"The elevator is currently on floor {elevator.CurrentFloor}. Please enter the floor of the current passenger (1-10). Enter X to exit the simulation:");
var command = Console.ReadLine();
while (command != "X")
{
    var passengerFloor = int.Parse(command);

    //create pending request
    elevator.PendingRequests.Add(new PendingRequest { RequestedDirection = Direction.Up, Floor = passengerFloor });

    //disembark
    elevator.Passengers.RemoveAll(p => p.DesiredFloor == elevator.CurrentFloor);

    //embark
    Console.WriteLine("You have entered the elevator. Please enter your desired floor (1-10):");
    var desiredFloor = int.Parse(Console.ReadLine());

    var passenger = new Passenger(desiredFloor);
    elevator.Passengers.Add(passenger);

    //end time
    var nextDesiredFloor = elevator.Passengers.FirstOrDefault().DesiredFloor;
    var nextRequestedPickup = elevator.PendingRequests.FirstOrDefault().Floor;

    if(nextDesiredFloor > elevator.CurrentFloor)
    {
        Console.WriteLine("Going up!");
        elevator.Direction = Direction.Up;
    }

    Console.WriteLine($"Passenger has embarked. Time is currently {elevator.Time}. Direction: {elevator.Direction}. Current Floor: {elevator.CurrentFloor}");
}

Console.WriteLine("The simulation has ended.");

