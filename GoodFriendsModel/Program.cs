namespace GoodFriendsModel;
class Program
{
    static void Main(string[] args)
    {
        PopulateModel();

    }
    private static void PopulateModel()
    {
        //Create a list of friends
        var _goodfriends = new List<csFriend>();
        for (int c = 0; c < 20; c++)
        {
            _goodfriends.Add(csFriend.Factory.CreateRandom());
        }

        foreach (var friend in _goodfriends)
        {
            Console.WriteLine(friend);
        }

        Console.WriteLine($"NrOfFriends: {_goodfriends.Count()}");
        Console.WriteLine($"NrOfFriends without any pets: {_goodfriends.Count(f => f.Pets == null)}");
        Console.WriteLine($"NrOfFriends without an adress: {_goodfriends.Count(f => f.Adress == null)}");
    }
}

