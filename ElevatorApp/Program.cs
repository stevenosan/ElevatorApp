// See https://aka.ms/new-console-template for more information

using ElevatorApp;

var elevator = new Elevator();

Console.WriteLine("The Ellevation Elevator Simulator has started.");

var command = "";
while (command != "X")
{
    Console.WriteLine($"Time: {elevator.Time}, Direction: {elevator.Direction}, floor: {elevator.CurrentFloor}");

    var passengersDisembarked = elevator.DisembarkPassengers();
    if(passengersDisembarked > 0)
    {
        Console.WriteLine($"{passengersDisembarked} passengers disembarked on Floor {elevator.CurrentFloor}. ");
    }

    Console.Write("Please enter the floor of a waiting passenger (1-10). Enter S to Skip, X to end: ");
    command = Console.ReadLine();
    if (command == "X")
    { break; }
    if(command != "S")
    {
        var result = int.TryParse(command, out int floor);
        while (!result || elevator.TopFloor > 10 || floor < 1)
        {
            Console.Write("That was not a valid floor number. Please enter 1-10: ");
            command = Console.ReadLine();
        }

        Console.Write("Please enter the direction the waiting passenger wants to go (U for Up, D for Down): ");
        var direction = (Direction)char.Parse(Console.ReadLine());

        elevator.RequestElevator(floor, direction);
    }

    elevator.EmbarkPassengers();

    elevator.Move();
    Console.WriteLine("----------------TICK----------------");
}

Console.WriteLine("The simulation has ended.");

