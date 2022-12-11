﻿namespace ShopOnline.Core.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        //IGenericRepository<Country> Countries { get; }
        //IGenericRepository<Hotel> Hotels { get; }
        Task SaveAsync();
    }
}
