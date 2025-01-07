﻿using APICatalogo.Models;

namespace APICatalogo.Repositories.Interfaces
{
    public interface ICategoriaRepository
    {
        IEnumerable<Categoria> GetAll();
        Categoria GetById(int id);
        Categoria Create(Categoria categoria);
        Categoria Update(Categoria categoria);
        Categoria Delete(int id);  
    }
}
