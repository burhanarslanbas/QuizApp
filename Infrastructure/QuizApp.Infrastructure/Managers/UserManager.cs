using AutoMapper;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Responses.User;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;
using QuizApp.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace QuizApp.Infrastructure.Managers;

public class UserManager : IUserService
{
    private readonly IUserReadRepository _userReadRepository;
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IMapper _mapper;

    public UserManager(IUserReadRepository userReadRepository, IUserWriteRepository userWriteRepository, IMapper mapper)
    {
        _userReadRepository = userReadRepository;
        _userWriteRepository = userWriteRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        var result = await _userWriteRepository.AddAsync(user);
        if (!result)
            throw new BusinessException("Failed to create user");

        return result;
    }

    public async Task<List<UserDTO>> CreateRangeAsync(List<CreateUserRequest> requests)
    {
        var users = _mapper.Map<List<User>>(requests);
        var result = await _userWriteRepository.AddRangeAsync(users);
        if (!result)
            throw new BusinessException("Failed to create users");

        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _userWriteRepository.RemoveById(id);
        if (!result)
            throw new NotFoundException($"User with ID {id} not found.");

        return result;
    }

    public bool DeleteRange(List<Guid> ids)
    {
        var users = _userReadRepository.GetWhere(x => ids.Contains(x.Id)).ToList();
        if (!users.Any())
            throw new NotFoundException("No users found with the provided IDs.");

        var result = _userWriteRepository.RemoveRange(users);
        if (!result)
            throw new BusinessException("Failed to delete users.");

        return result;
    }

    public List<UserDTO> GetAll()
    {
        var users = _userReadRepository.GetAll().ToList();
        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<UserDTO> GetByIdAsync(Guid id)
    {
        try
        {
            var user = await _userReadRepository.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User with ID {id} not found.");
        }
    }

    public bool Update(UpdateUserRequest request)
    {
        try
        {
            var user = _userReadRepository.GetByIdAsync(request.Id).Result;
            _mapper.Map(request, user);
            var result = _userWriteRepository.Update(user);
            if (!result)
                throw new BusinessException($"Failed to update user with ID {request.Id}");

            return result;
        }
        catch (InvalidOperationException)
        {
            throw new NotFoundException($"User with ID {request.Id} not found.");
        }
    }
}