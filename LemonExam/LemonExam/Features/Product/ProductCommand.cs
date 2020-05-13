using LemonExam.Infrastructure;
using LemonExam.Model;
using LemonExam.Model.Master;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LemonExam.Features.Product
{
    public class ProductCommand : ICommandHandler<ProductParam, ProductResponse>
    {
        private LocalDbContext _dbContext;
        public ProductCommand(LocalDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public ProductResponse Handle(ProductParam command)
        {
            var productResponse = new ProductResponse();
            ProductEntry product;
            if (command.action == "read")
            {
                product = _dbContext.Products.Where(p =>
                                p.ID == Int32.Parse(command.ID)).FirstOrDefault();
                if (product != null)
                {
                    productResponse.data = product;
                }

                productResponse.isSuccess = true;
                productResponse.message = "Success";
                productResponse.statusCode = StatusCodes.Status200OK;
            }
            else
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        switch (command.action.Trim())
                        {
                            case "create":
                                product = command.convertToModel(false);
                                _dbContext.Products.Add(product);
                                break;
                            case "update":
                                product = command.convertToModel(true);
                                _dbContext.Products.Update(product);
                                break;
                            case "delete":
                                product = _dbContext.Products.Where(p =>
                                    p.ID == Int64.Parse(command.ID)).FirstOrDefault();
                                _dbContext.Products.Remove(product);
                                break;
                            default:

                                break;
                        }

                        if (_dbContext.SaveChanges() > 0)
                        {
                            productResponse.isSuccess = true;
                            productResponse.message = "Success";
                            productResponse.statusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            productResponse.isSuccess = false;
                            productResponse.message = "Failed";
                            productResponse.statusCode = StatusCodes.Status500InternalServerError;
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        productResponse.isSuccess = false;
                        productResponse.message = ex.StackTrace.ToString();
                        productResponse.statusCode = StatusCodes.Status500InternalServerError;
                    }
                }
            }


            return productResponse;
        }
        
    }
}
