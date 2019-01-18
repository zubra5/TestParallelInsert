
namespace TestParallelInsert.Repositories
{
    public interface IRepository
    {
        string InsertData(string untranslated, string translated);
    }
}
