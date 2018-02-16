namespace TennisScore
{
    public interface IRepository<T>
    {
        Game GetGame(int gameId);
    }
}