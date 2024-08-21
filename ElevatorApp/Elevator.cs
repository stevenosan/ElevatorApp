namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction = Direction.Waiting;
    public List<Passenger> Passengers = [];
    private int _finalDestinationFloor;

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();

    public int DisembarkPassengers()
    {
        var disembarkCount = Passengers.RemoveAll(p => p.DesiredFloor == CurrentFloor);

        if (!Passengers.Any() && !PendingRequests.Any())
        {
            Direction = Direction.Waiting;
        }
        
        if (PendingRequests.Any() && !Passengers.Any())
        {
            _finalDestinationFloor = PendingRequests.First().Floor;
            Direction = PendingRequests.First().Direction;
        }

        return disembarkCount;
    }

    public void EmbarkPassengers()
    {
        var waitingPassengers = PendingRequests.Where(r => r.Direction == Direction && r.Floor == CurrentFloor).ToList();

        if (!waitingPassengers.Any())
        {
            Console.WriteLine($"There are no passengers waiting at current floor #{CurrentFloor} to go {Direction}. Not taking any passengers.");
            return;
        }

        Console.WriteLine($"There are {waitingPassengers.Count()} people waiting to go {Direction} at the current floor #{CurrentFloor}");

        foreach (var newPassenger in waitingPassengers)
        {
            if (Direction == Direction.Up)
            {
                Console.Write($"Passenger going up. Please enter a floor between {CurrentFloor} and {TopFloor}: ");
                var result = int.TryParse(Console.ReadLine(), out int desiredFloor);

                while (desiredFloor < CurrentFloor || desiredFloor > TopFloor)
                {
                    Console.Write($"That was not a valid floor number. Please enter a floor between {CurrentFloor} and {TopFloor}: ");
                    result = int.TryParse(Console.ReadLine(), out desiredFloor);
                }

                var passenger = new Passenger(desiredFloor);
                if (desiredFloor > _finalDestinationFloor)
                {
                    _finalDestinationFloor = desiredFloor;
                }

                Passengers.Add(passenger);
            }
            else if (Direction == Direction.Down)
            {
                Console.Write($"Passenger going down. Please enter a floor between 1 and {CurrentFloor}: ");
                var result = int.TryParse(Console.ReadLine(), out int desiredFloor);

                while (desiredFloor > CurrentFloor || desiredFloor < 1  )
                {
                    Console.Write($"That was not a valid floor number. Please enter a floor between 1 and {CurrentFloor}: ");
                    result = int.TryParse(Console.ReadLine(), out desiredFloor);
                }

                var passenger = new Passenger(desiredFloor);
                if (desiredFloor < _finalDestinationFloor)
                {
                    _finalDestinationFloor = desiredFloor;
                }
                Passengers.Add(passenger);
            }

            PendingRequests.Remove(newPassenger);
        }
    }
    public void RequestElevator(int passengerFloor, Direction direction)
    {
        if (Direction == Direction.Waiting)
        {
            Direction = direction;
        }

        PendingRequests.Add(new PendingRequest { Direction = direction, Floor = passengerFloor });
    }

    public void Move()
    {
        if (Direction == Direction.Up)
        {
            CurrentFloor++;
        }
        else if (Direction == Direction.Down)
        {
            CurrentFloor--;
        }

        Time++;
    }
}

public class PendingRequest
{
    public int Floor;
    public Direction Direction;
}

public class Passenger(int desiredFloor)
{
    public int DesiredFloor = desiredFloor;
}

public enum Direction
{
    Up = 'U',
    Down = 'D',
    Waiting = 'W'
}