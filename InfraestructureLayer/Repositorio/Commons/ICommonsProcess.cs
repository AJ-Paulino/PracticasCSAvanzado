﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraestructureLayer.Repositorio.Commons
{
    public interface ICommonsProcess<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetIdAsync(int id);
        Task<(bool IsSuccess, string Message)> AddAsync(T entity);
        Task<(bool IsSuccess, string Message)> UpdateAsync(T entity);
        Task<(bool IsSuccess, string Message)> DeleteAsync(int id);
    }
}
