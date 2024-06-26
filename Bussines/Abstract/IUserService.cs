﻿using Core.Utilities.Results.Abstract;
using Entities.Concrete;
using Entities.DTOs.UserDTOs;
using System.Linq.Expressions;
using Core.Utilities.Results.Abstract;
namespace Bussines.Abstract;

public interface IUserService

{
    public IDataResult<List<User>> GetUsers();
   
    public IDataResult<User> GetUser(Expression<Func<User, bool>> expression);
    public Task <IDataResult<User>> GetUserAsync(Expression<Func<User, bool>> expression);
     public Task<IResult> UpdateUserAsync(UserUpdateDTO userUpdateDTO,string url);
    public  Task<IDataResult<User>> AddUserAsync(UserRegisterDTO userRegisterDTO);
   
   



}
