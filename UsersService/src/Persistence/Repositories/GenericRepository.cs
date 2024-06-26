﻿using Application.Interfaces.Repositories;
using ArchitectureCommonDomainLib.Common;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        public IQueryable<T> Entities => _context.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            T exist = await _context.Set<T>().FirstOrDefaultAsync(x => x.Guid == entity.Guid);
            _context.Entry(exist).CurrentValues.SetValues(entity);
            await Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByGuidAsync(string guid)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Guid == guid);
        }
    }
}
