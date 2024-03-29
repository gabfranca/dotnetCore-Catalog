using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalog.Data;
using Catalog.Models;
using Catalog.ViewModels.ProductViewModels;
using Catalog.Repositories;

namespace Catalog.Controllers
{
    public class ProductController : Controller
    {
 
        private readonly ProductRepository repository; 

        public ProductController(ProductRepository _repository ){
            repository = _repository;
        }

        [Route("v1/products")]
        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return repository.Get();
        }

        [Route("v1/products/{id}")]
        [HttpGet]
        public Product Get(int id)
        {
            return repository.Get(id);
        }

        [Route("v1/products")]
        [HttpPost]
        public ResultViewModel Post([FromBody]EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "Não foi possível cadastrar o produto",
                    Data = model.Notifications
                };

            var product = new Product();
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.CreateDate = DateTime.Now; // Nunca recebe esta informação
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now; // Nunca recebe esta informação
            product.Price = model.Price;
            product.Quantity = model.Quantity;

       
            repository.Save(product);
            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso!",
                Data = product
            };
        }

        [Route("v1/products")]
        [HttpPut]
        public ResultViewModel Put([FromBody]EditorProductViewModel model)
        {
            model.Validate();
            if (model.Invalid)
                return new ResultViewModel
                {
                    Success = false,
                    Message = "teste",
                    Data = model.Notifications
                };

            var product = repository.Get(model.Id);
            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            // product.CreateDate = DateTime.Now; // Nunca altera a data de criação
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now; // Nunca recebe esta informação
            product.Price = model.Price;
            product.Quantity = model.Quantity;

  
            repository.Update(product);
            return new ResultViewModel
            {
                Success = true,
                Message = "Produto alterado com sucesso!",
                Data = product
            };
        }
    }
}