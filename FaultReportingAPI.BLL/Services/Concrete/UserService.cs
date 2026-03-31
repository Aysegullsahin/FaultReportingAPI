using FaultReportingAPI.BLL.Mapper.Abstract;
using FaultReportingAPI.BLL.Services.Abstract;
using FaultReportingAPI.BLL.Services.Concrete.Base;
using FaultReportingAPI.Core.Dto;
using FaultReportingAPI.Core.Enums;
using FaultReportingAPI.Core.Models;
using FaultReportingAPI.Core.Models.Base;
using FaultReportingAPI.DAL.UnitOfWork.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FaultReportingAPI.BLL.Services.Concrete
{
    public class UserService(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor, IUserMapper mapper) : BaseService<User>(uow: uow, httpContextAccessor: httpContextAccessor), IUserService
    {
        private readonly string _secretKey = "684309524012-*4glhkyu65sdrw344567yjfs434";
        private readonly IUserMapper _mapper = mapper;

        #region Login
        public async Task<DataResult<UserDto_Token>> LoginUserAsync(string email, string password)
        {
            DataResult<CoreBaseEntity> dataResult = await _uow.userRepository.Login<UserDto_LoginResponse>(filter: x => x.Email == email && !x.IsDeleted);

            if (dataResult.Data == null)
            {
                return new DataResult<UserDto_Token>()
                {
                    Message = "The user was not found.",
                    StatusCode = HttpStatusCode.NotFound
                };
            }
            var user = dataResult.Data as UserDto_LoginResponse;

            if (!BCrypt.Net.BCrypt.Verify(password, user!.HashPassword))
            {
                return new DataResult<UserDto_Token>()
                {
                    Message = "The password is incorrect.",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            return GenerateToken(user.Email, user.Role, user.Id, _secretKey);
        }
        #endregion
        #region Crud Operations
        public async Task<DataResult<UserDto_Token>> RegisterAsync(UserDto_Request entityDto)
        {
            var entity = _mapper.ToEntity(entityDto);

            entity.Role = RoleEnum.User;
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);


            var dataResult = await _uow.userRepository.CrudAsync(entity, EntityStateEnum.Insert);

            if (dataResult.Success)
                return GenerateToken(dataResult.Data!.Email!, dataResult.Data?.Role, dataResult.Data!.Id!, _secretKey);

            return new DataResult<UserDto_Token>()
            {
                Message = "Registration failed: " + dataResult.Message,
                StatusCode = dataResult.StatusCode
            };
        }

        public async Task<DataResult<CoreBaseEntity>> UpdateAsync(UserDto_Request entityDto, Dictionary<string, object>? changedFields = null)
        {
            var entity = _mapper.ToEntity(entityDto);
            changedFields = new()
            {
                ["Name"] = entityDto.Name,
                ["Surname"] = entityDto.Surname,
                ["Email"] = entityDto.Email,
            };
            var dataResult = await _uow.userRepository.CrudAsync(entity: entity, entityState: EntityStateEnum.Update, changedFields: changedFields);

            if (!dataResult.Success || dataResult.Data == null)
            {
                return new DataResult<CoreBaseEntity>
                {
                    Success = false,
                    Message = dataResult.Message,
                    StatusCode = dataResult.StatusCode
                };
            }
            return new DataResult<CoreBaseEntity>
            {
                Success = true,
                Data = _mapper.ToDto_S(dataResult.Data),
                StatusCode = dataResult.StatusCode
            };
        }
        #endregion

        #region Single
        public async override Task<DataResult<CoreBaseEntity>> GetAsync(long id)
        {
            return await _uow.faultReportingRepository.GetAsync<UserDto_S>(filter: x => x.CreatedById == userId && x.Id == id && !x.IsDeleted);
        }
        #endregion
        #region List
        public async override Task<DataResult<CoreBaseEntity>> GetListAsync<TResult>(Expression<Func<User, bool>>? filter = null, Expression<Func<User, TResult>>? selector = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, int pageNumber = 1, int pageSize = 10)
        {
            return await _uow.userRepository.GetListAsync(filter, selector, orderBy: q => q.OrderBy(x => x.Id), pageSize, pageNumber);
        }
        #endregion
        #region GenerateToken
        private static DataResult<UserDto_Token> GenerateToken(string email, RoleEnum? role, long userId, string secretKey)
        {
            DataResult<UserDto_Token> response = new();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                new Claim(ClaimTypes.Email, email),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, role.ToString()?? ""),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            ]),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            response.Data = new UserDto_Token()
            {
                JWTToken = tokenHandler.WriteToken(token),
                Id = userId
            };
            response.StatusCode = HttpStatusCode.OK;
            response.Success = true;
            return response;
        }
        #endregion
    }
}
