// See https://aka.ms/new-console-template for more information

using ElevatorApp;

var elevator = new Elevator();

string command = "";
var nextFloor = 999;

while (command != "X")
{
    //go to next floor
    if (elevator.Direction == Direction.Up)
    {
        var nextFloorVal = elevator.Passengers.FirstOrDefault(p => p.DesiredFloor > elevator.CurrentFloor)?.DesiredFloor;
        
        var nextPickupVal = elevator.PendingRequests.FirstOrDefault()?.Floor;
        var nextRequestedPickupFloor = nextPickupVal ??= 0;

        nextFloor = nextFloorVal > nextRequestedPickupFloor && nextRequestedPickupFloor < elevator.CurrentFloor ? nextFloorVal.Value : nextRequestedPickupFloor;
    }
    else
    {
        var nextFloorVal = elevator.Passengers.FirstOrDefault(p => p.DesiredFloor > elevator.CurrentFloor)?.DesiredFloor;

        var nextPickupVal = elevator.PendingRequests.FirstOrDefault()?.Floor;
        var nextRequestedPickupFloor = nextPickupVal ??= 0;


        nextFloor = nextFloorVal < nextRequestedPickupFloor && nextRequestedPickupFloor < elevator.CurrentFloor ? nextFloorVal.Value : nextRequestedPickupFloor;
    }
    elevator.Time += Math.Abs(nextFloor - elevator.CurrentFloor);

    elevator.CurrentFloor = nextFloor == 0 ? 1 : nextFloor; //this is a poor solution for how to start this

    Console.WriteLine($"The elevator went {elevator.Direction} to Floor {elevator.CurrentFloor}");

    //disembark
    elevator.Passengers.RemoveAll(p => p.DesiredFloor == elevator.CurrentFloor);

    //embark
    if(elevator.PendingRequests.Any(r => r.Floor == elevator.CurrentFloor))
    {
        Console.WriteLine($"The elevator is currently {elevator.CurrentFloor} at floor with a waiting passenger going {elevator.PendingRequests.First().RequestedDirection}. Please enter your desired floor (1-10) for this passenger:");
        var desiredFloor = int.Parse(Console.ReadLine());

        var passenger = new Passenger(desiredFloor);
        elevator.Passengers.Add(passenger);

        elevator.PendingRequests.RemoveAll(r => r.Floor == elevator.CurrentFloor);
    }

    //start next turn

    //create pending request
    Console.WriteLine("Please enter the requested passenger floor (Floors 1-10): ");
    var passengerFloor = int.Parse(Console.ReadLine());
    Console.WriteLine($"Please enter the desired direction of the requesting passenger(U for Up, D For Down) If you wish to exit the simulator, enter X: ");
    command = Console.ReadLine();

    var requestedDirection = char.Parse(command);
    elevator.PendingRequests.Add(new PendingRequest { RequestedDirection = (Direction)requestedDirection, Floor = passengerFloor });

    var nextDesiredFloorVal = elevator.Passengers.FirstOrDefault()?.DesiredFloor;
    var nextRequestedWaitingFloorVal = elevator.PendingRequests.FirstOrDefault()?.Floor;

    var nextDesiredFloor = nextDesiredFloorVal ??= 0;
    var nextRequestedWaitingFloor = nextRequestedWaitingFloorVal ??= 0;

    Console.WriteLine($"All passengers have embarked/disembarked. Time is currently {elevator.Time}. Direction: {elevator.Direction}. Current Floor: {elevator.CurrentFloor}");
}

Console.WriteLine("The simulation has ended.");

