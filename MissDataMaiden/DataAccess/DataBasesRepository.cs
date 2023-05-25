using MissBot.Abstractions.DataAccess;
using MissDataMaiden.Entities;

namespace MissDataMaiden.DataAccess
{
    internal class DataBasesRepository : IRepository<DataBase>
    {



        public Task<IEnumerable<DataBase>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DataBase> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : DataBase
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : DataBase
        {
            throw new NotImplementedException();
        }

        public Task<TEntityType> GetAsync<TEntityType>() where TEntityType : DataBase
        {
            throw new NotImplementedException();
        }
        // public override Task<DataBase> GetAsyncForAction<TAction>(TAction action)
        //{

        //    return Task.FromResult(new DataBase() { Id = Convert.ToInt32(action.Id) });
        //}
    }
}
