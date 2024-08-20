// See https://aka.ms/new-console-template for more information

using ElevatorApp;

var elevator = new Elevator();

//var nextFloor = 999;

Console.WriteLine("The Ellevation Elevator Simulator has started.");

var command = "";
while (command != "X")
{
    Console.Write("Please enter the floor of the current passenger (1-10). Enter S to Skip, X to end: ");
    command = Console.ReadLine();
    if (command == "X")
    { break; }
    if(command != "S")
    {
        var result = int.TryParse(command, out int floor);
        while (!result || floor > 10 || floor < 1)
        {
            Console.Write("That was not a valid floor number. Please enter 1-10: ");
            command = Console.ReadLine();
        }

        Console.Write("Please enter the direction the current passenger wants to go (U for Up, D for Down): ");
        var direction = (Direction)char.Parse(Console.ReadLine());

        elevator.RequestElevator(floor, direction);
    }

    var passengersDisembarked = elevator.DisembarkPassengers();
    Console.WriteLine($"{passengersDisembarked} passengers disembarked on Floor {elevator.CurrentFloor}. ");

    var waitingPassengers = elevator.PendingRequests.Count(r => r.Floor == elevator.CurrentFloor);
    Console.WriteLine($"There are {waitingPassengers} passengers waiting at the current floor {elevator.CurrentFloor}");

    for (int i = 0; i < waitingPassengers; i++)
    {
        Console.Write("Enter the desired floor of the user (1-10): ");
        var result = int.TryParse(Console.ReadLine(), out int desiredFloor);

        while (!result || desiredFloor > 10 || desiredFloor < 1)
        {
            Console.Write("That was not a valid floor number. Please enter 1-10: ");
            result = int.TryParse(Console.ReadLine(), out desiredFloor);
        }

        elevator.EmbarkPassenger(desiredFloor);
    }

    Console.WriteLine($"{waitingPassengers} have embarked at floor {elevator.CurrentFloor}");

    Console.WriteLine($"TICK: Time: {elevator.Time}, Direction: {elevator.Direction}, floor: {elevator.CurrentFloor}");
    elevator.Move();
}

//while (command != "X")
//{
//    //go to next floor
//    if (elevator.Direction == Direction.Up)
//    {
//        var nextFloorVal = elevator.Passengers.FirstOrDefault(p => p.DesiredFloor > elevator.CurrentFloor)?.DesiredFloor;
        
//        var nextPickupVal = elevator.PendingRequests.FirstOrDefault()?.Floor;
//        var nextRequestedPickupFloor = nextPickupVal ??= 0;

//        nextFloor = nextFloorVal > nextRequestedPickupFloor && nextRequestedPickupFloor < elevator.CurrentFloor ? nextFloorVal.Value : nextRequestedPickupFloor;
//    }
//    else
//    {
//        var nextFloorVal = elevator.Passengers.FirstOrDefault(p => p.DesiredFloor > elevator.CurrentFloor)?.DesiredFloor;

//        var nextPickupVal = elevator.PendingRequests.FirstOrDefault()?.Floor;
//        var nextRequestedPickupFloor = nextPickupVal ??= 0;


//        nextFloor = nextFloorVal < nextRequestedPickupFloor && nextRequestedPickupFloor < elevator.CurrentFloor ? nextFloorVal.Value : nextRequestedPickupFloor;
//    }
//    elevator.Time += Math.Abs(nextFloor - elevator.CurrentFloor);

//    elevator.CurrentFloor = nextFloor == 0 ? 1 : nextFloor; //this is a poor solution for how to start this

//    Console.WriteLine($"The elevator went {elevator.Direction} to Floor {elevator.CurrentFloor}");

//    //disembark
//    elevator.Passengers.RemoveAll(p => p.DesiredFloor == elevator.CurrentFloor);

//    //embark
//    if(elevator.PendingRequests.Any(r => r.Floor == elevator.CurrentFloor))
//    {
//        Console.WriteLine($"The elevator is currently {elevator.CurrentFloor} at floor with a waiting passenger going {elevator.PendingRequests.First().RequestedDirection}. Please enter your desired floor (1-10) for this passenger:");
//        var desiredFloor = int.Parse(Console.ReadLine());

//        var passenger = new Passenger(desiredFloor);
//        elevator.Passengers.Add(passenger);

//        elevator.PendingRequests.RemoveAll(r => r.Floor == elevator.CurrentFloor);
//    }

//    //start next turn

//    //create pending request
//    Console.WriteLine("Please enter the requested passenger floor (Floors 1-10): ");
//    var passengerFloor = int.Parse(Console.ReadLine());
//    Console.WriteLine($"Please enter the desired direction of the requesting passenger(U for Up, D For Down) If you wish to exit the simulator, enter X: ");
//    command = Console.ReadLine();

//    var requestedDirection = char.Parse(command);
//    elevator.PendingRequests.Add(new PendingRequest { RequestedDirection = (Direction)requestedDirection, Floor = passengerFloor });

//    var nextDesiredFloorVal = elevator.Passengers.FirstOrDefault()?.DesiredFloor;
//    var nextRequestedWaitingFloorVal = elevator.PendingRequests.FirstOrDefault()?.Floor;

//    var nextDesiredFloor = nextDesiredFloorVal ??= 0;
//    var nextRequestedWaitingFloor = nextRequestedWaitingFloorVal ??= 0;

//    Console.WriteLine($"All passengers have embarked/disembarked. Time is currently {elevator.Time}. Direction: {elevator.Direction}. Current Floor: {elevator.CurrentFloor}");
//}

Console.WriteLine("The simulation has ended.");

