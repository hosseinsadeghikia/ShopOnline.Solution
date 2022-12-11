﻿using ShopOnline.Core.IRepository;
using ShopOnline.Data;

namespace ShopOnline.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        //private IGenericRepository<Country> _countries;
        //private IGenericRepository<Hotel> _hotels;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IGenericRepository<Country> Countries => _countries = new GenericRepository<Country>(_context);
        //public IGenericRepository<Hotel> Hotels => _hotels = new GenericRepository<Hotel>(_context);


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
