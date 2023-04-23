namespace MusicModel;
class Program
{
    static void Main(string[] args)
    {
        var _modelList = SeedModel();
        WriteModel(_modelList);
    }

    #region Replaced by new model methods
    private static void WriteModel(List<csMusicGroup> _greatMusicBands)
    {
        foreach (var band in _greatMusicBands)
        {
            Console.WriteLine(band);
        }

        Console.WriteLine($"Nr of great music bands: {_greatMusicBands.Count()}");
        Console.WriteLine($"Total nr of albums produced: {_greatMusicBands.Sum(b => b.Albums.Count)}");
        Console.WriteLine($"Total nr of music band members: {_greatMusicBands.Sum(b => b.Members.Count)}");
    }

    private static List<csMusicGroup> SeedModel()
    {
        //Create a list of 20 great bands
        var _greatMusicBands = new List<csMusicGroup>();
        for (int c = 0; c < 20; c++)
        {
            _greatMusicBands.Add(csMusicGroup.Factory.CreateRandom());
        }

        return _greatMusicBands;
    }
    #endregion
}

