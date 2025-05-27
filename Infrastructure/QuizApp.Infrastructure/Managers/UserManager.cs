using AutoMapper;
using QuizApp.Application.DTOs.Requests.User;
using QuizApp.Application.DTOs.Requests.User.Read;
using QuizApp.Application.Repositories;
using QuizApp.Application.Services;
using QuizApp.Domain.Entities;

namespace QuizApp.Infrastructure.Managers;

public class UserManager : IUserService
{
    private readonly IUserWriteRepository _userWriteRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IMapper _mapper;

    public UserManager(
        IUserWriteRepository userWriteRepository,
        IUserReadRepository userReadRepository,
        IMapper mapper)
    {
        _userWriteRepository = userWriteRepository;
        _userReadRepository = userReadRepository;
        _mapper = mapper;
    }

    public async Task<bool> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<Domain.Entities.User>(request);
        // TODO: Add password hashing
        return await _userWriteRepository.AddAsync(user);
    }

    public Task<List<UserDTO>> CreateRangeAsync(List<CreateUserRequest> requests)
    {
        var users = _mapper.Map<List<Domain.Entities.User>>(requests);
        // TODO: Add password hashing for each user
        return _userWriteRepository.AddRangeAsync(users)
            .ContinueWith(task => task.Result ? _mapper.Map<List<UserDTO>>(users) : null);
    }

    public Task<bool> Delete(Guid id)
    {
        return _userWriteRepository.RemoveById(id);
    }

    public Task<bool> DeleteRange(List<Guid> ids)
    {
        var users = _userReadRepository.GetWhere(u => ids.Contains(u.Id)).ToList();
        if (users == null || !users.Any())
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(_userWriteRepository.RemoveRange(users));
    }

    public Task<List<UserDTO>> GetAll()
    {
        var users = _userReadRepository.GetAll();
        if (users == null || !users.Any())
        {
            return Task.FromResult(new List<UserDTO>());
        }
        return Task.FromResult(_mapper.Map<List<UserDTO>>(users));
    }

    public Task<UserDTO> GetById(Guid id)
    {
        var user = _userReadRepository.GetByIdAsync(id).Result;
        if (user == null)
        {
            return Task.FromResult<UserDTO>(null);
        }
        return Task.FromResult(_mapper.Map<UserDTO>(user));
    }

    public Task<UserDTO> Update(UpdateUserRequest request)
    {
        var user = _mapper.Map<Domain.Entities.User>(request);
        if (_userWriteRepository.Update(user))
        {
            return Task.FromResult(_mapper.Map<UserDTO>(user));
        }
        throw new Exception("User update failed.");
    }

    public async Task<bool> UpdateRange(List<UpdateUserRequest> requests)
    {
        var users = _mapper.Map<List<Domain.Entities.User>>(requests);
        return _userWriteRepository.UpdateRange(users);
    }

    public async Task<UserDTO> GetByEmail(string email)
    {
        var user = await _userReadRepository.GetWhere(u => u.Email == email).FirstOrDefaultAsync();
        return user != null ? _mapper.Map<UserDTO>(user) : null;
    }

    public async Task<List<UserDTO>> GetByRole(UserRole role)
    {
        var users = await _userReadRepository.GetWhere(u => u.Role == role).ToListAsync();
        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<bool> ChangePassword(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userReadRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        // TODO: Implement password validation and hashing
        // if (!ValidatePassword(currentPassword, user.PasswordHash))
        //     return false;

        // user.PasswordHash = HashPassword(newPassword);
        return _userWriteRepository.Update(user);
    }

    public async Task<bool> ResetPassword(Guid userId, string newPassword)
    {
        var user = await _userReadRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        // TODO: Implement password hashing
        // user.PasswordHash = HashPassword(newPassword);
        return _userWriteRepository.Update(user);
    }

    public async Task<bool> ActivateUser(Guid userId)
    {
        var user = await _userReadRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        user.IsActive = true;
        return _userWriteRepository.Update(user);
    }

    public async Task<bool> DeactivateUser(Guid userId)
    {
        var user = await _userReadRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        user.IsActive = false;
        return _userWriteRepository.Update(user);
    }

    public async Task<bool> UpdateProfile(Guid userId, string firstName, string lastName, string phoneNumber, string address)
    {
        var user = await _userReadRepository.GetByIdAsync(userId);
        if (user == null)
            return false;

        user.FirstName = firstName;
        user.LastName = lastName;
        user.PhoneNumber = phoneNumber;
        user.Address = address;

        return _userWriteRepository.Update(user);
    }

    public async Task<bool> ValidateUser(string email, string password)
    {
        var user = await _userReadRepository.GetWhere(u => u.Email == email).FirstOrDefaultAsync();
        if (user == null)
            return false;

        // TODO: Implement password validation
        // return ValidatePassword(password, user.PasswordHash);
        return true;
    }

    public async Task<List<UserDTO>> GetActiveUsers()
    {
        var users = await _userReadRepository.GetWhere(u => u.IsActive).ToListAsync();
        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<List<UserDTO>> GetInactiveUsers()
    {
        var users = await _userReadRepository.GetWhere(u => !u.IsActive).ToListAsync();
        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<int> GetUserCount()
    {
        return await _userReadRepository.GetAll().CountAsync();
    }

    public async Task<Dictionary<UserRole, int>> GetUserCountByRole()
    {
        var users = await _userReadRepository.GetAll().ToListAsync();
        return users.GroupBy(u => u.Role)
                   .ToDictionary(g => g.Key, g => g.Count());
    }
} 