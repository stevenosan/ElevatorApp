namespace ElevatorApp;
public class Elevator
{
    public int CurrentFloor = 1;
    public int TopFloor => 10;
    public List<int> Passengers = [];

    public List<Floor> Floors = Enumerable.Range(1, 10).Select(x => new Floor(x)).ToList();
}

public class WaitingPassenger(int desiredFloor)
{
    public int DesiredFloor;
}

public enum Direction
{
    Up,
    Down,
}

public class Floor
{
    public int FloorNumber;
    public Queue<WaitingPassenger> WaitingPassengers = [];

    public Floor(int floorNumber)
    {
        FloorNumber = floorNumber;
    }
}