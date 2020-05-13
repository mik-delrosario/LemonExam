using LemonExam.Infrastructure;
using LemonExam.Model;
using System;
using Microsoft.AspNetCore.Http;
using LemonExam.Model.Master;
using System.Linq;
using System.Collections.Generic;

namespace LemonExam.Features.Category
{
    public class CategoryCommand : ICommandHandler<CategoryParam, CategoryResponse>
    {
        private LocalDbContext _dbContext;
        public CategoryCommand(LocalDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        
        public CategoryResponse Handle(CategoryParam command)
        {
            var categoryResponse = new CategoryResponse();
            CategoryEntry category;
            if(command.action == "read")
            {
                category = _dbContext.Categories.Where(c =>
                                c.ID == Int32.Parse(command.ID)).FirstOrDefault();
                if (category != null)
                {
                    categoryResponse.data = category;
                }
                
                categoryResponse.isSuccess = true;
                categoryResponse.message = "Success";
                categoryResponse.statusCode = StatusCodes.Status200OK;
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
                                category = command.convertToModel(false);
                                _dbContext.Categories.Add(category);
                                break;
                            case "update":
                                category = command.convertToModel(true);
                                _dbContext.Categories.Update(category);
                                break;
                            case "delete":
                                category = _dbContext.Categories.Where(c =>
                                    c.ID == Int64.Parse(command.ID)).FirstOrDefault();
                                _dbContext.Categories.Remove(category);
                                break;
                            default:

                                break;
                        }

                        if (_dbContext.SaveChanges() > 0)
                        {
                            categoryResponse.isSuccess = true;
                            categoryResponse.message = "Success";
                            categoryResponse.statusCode = StatusCodes.Status200OK;
                        }
                        else
                        {
                            categoryResponse.isSuccess = false;
                            categoryResponse.message = "Failed";
                            categoryResponse.statusCode = StatusCodes.Status500InternalServerError;
                        }

                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        categoryResponse.isSuccess = false;
                        categoryResponse.message = ex.StackTrace.ToString();
                        categoryResponse.statusCode = StatusCodes.Status500InternalServerError;
                    }
                }
            }
            

            return categoryResponse;
        }
    }
}
