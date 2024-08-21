namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public int Time = 0;
    public Direction Direction = Direction.Waiting;
    private Direction _direction;
    public List<Passenger> Passengers = [];
    private int _finalDestinationFloor;

    public List<PendingRequest> PendingRequests = new List<PendingRequest>();

    private Direction calculateDirection()
    {
        return _finalDestinationFloor > CurrentFloor ? Direction.Up : Direction.Down;
    }

    private int calculateNextFloor()
    {
        if (Direction == Direction.Up)
        {
            var destinationFloor = Passengers.Where(p => p.DesiredFloor > CurrentFloor).Select(p => p.DesiredFloor)
                .Concat(PendingRequests.Where(r => r.Floor > CurrentFloor).Select(r => r.Floor)).OrderBy(f => f).First();

            return destinationFloor;
        }
        else if(Direction == Direction.Down)
        {
            var destinationFloor = Passengers.Where(p => p.DesiredFloor > CurrentFloor).Select(p => p.DesiredFloor)
                .Concat(PendingRequests.Where(r => r.Floor > CurrentFloor).Select(r => r.Floor)).OrderByDescending(f => f).First();

            return destinationFloor;
        }
    }

    public int DisembarkPassengers()
    {
        var disembarkCount = Passengers.RemoveAll(p => p.DesiredFloor == CurrentFloor);

        if(!Passengers.Any())
        {
            Direction = Direction.Waiting;
        }

        return disembarkCount;
    }

    public void EmbarkPassenger(int destinationFloor)
    {
        var passenger = new Passenger(destinationFloor);
        Passengers.Add(passenger);
        var request = PendingRequests.First(r => r.Floor == CurrentFloor);
        PendingRequests.Remove(request);
    }

    public void EmbarkPassengers()
    {
        var waitingPassengers = PendingRequests.Where(r => r.Direction == Direction && r.Floor == CurrentFloor);
        Console.WriteLine($"There are {waitingPassengers.Count()} people waiting to go {Direction} at the current floor #{CurrentFloor}");

        if(!waitingPassengers.Any())
        {
            Console.WriteLine($"There are no passengers waiting at current floor #{CurrentFloor} to go {Direction}. Not taking any passengers.");
            return;
        }

        foreach(var newPassenger in waitingPassengers)
        {
            if(Direction == Direction.Up)
            {
                Console.Write($"Passenger going up. Please enter a floor between {CurrentFloor} and {TopFloor}: ");
                var result = int.TryParse(Console.ReadLine(), out int desiredFloor);
                
                while(desiredFloor < CurrentFloor || desiredFloor > TopFloor)
                {
                    Console.Write($"That was not a valid floor number. Please enter a floor between {CurrentFloor} and {TopFloor}: ");
                    result = int.TryParse(Console.ReadLine(), out desiredFloor);
                }

                var passenger = new Passenger(desiredFloor);
                if(desiredFloor > _finalDestinationFloor)
                {
                    _finalDestinationFloor = desiredFloor;
                }
            }
            else if(Direction == Direction.Down)
            {
                Console.Write($"Passenger going down. Please enter a floor between 1 and {CurrentFloor}: ");
                var result = int.TryParse(Console.ReadLine(), out int desiredFloor);

                while (desiredFloor > CurrentFloor || desiredFloor < TopFloor)
                {
                    Console.Write($"That was not a valid floor number. Please enter a floor between 1 and {CurrentFloor}: ");
                    result = int.TryParse(Console.ReadLine(), out desiredFloor);
                }

                var passenger = new Passenger(desiredFloor);
                if (desiredFloor < _finalDestinationFloor)
                {
                    _finalDestinationFloor = desiredFloor;
                }
            }

            PendingRequests.Remove(newPassenger);
        }

    }
    public void RequestElevator(int passengerFloor, Direction direction)
    {
        PendingRequests.Add(new PendingRequest { Direction = direction, Floor = passengerFloor });
    }

    public void Move()
    {
        if(Passengers.Any() || PendingRequests.Any())
        {
            if (Direction == Direction.Up)
            {
                CurrentFloor++;
            }
            else
            {
                CurrentFloor--;
            }
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