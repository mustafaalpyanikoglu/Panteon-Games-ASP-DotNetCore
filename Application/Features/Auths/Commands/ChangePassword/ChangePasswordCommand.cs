using Application.Features.Auths.Rules;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Core.Security.Hashing;
using Application.Services.Repositories;
using Domain.Concrete;
using MediatR;
using static Core.Security.Constants.GeneralOperationClaims;
using Application.Features.Auths.Dtos;

namespace Application.Features.Auths.Commands.ChangePassword;

public class ChangePasswordCommand:IRequest<UserForChangePasswordDto>,ISecuredRequest
{
    public string Email { get; set; }
    public string NewPassword { get; set; }
    public string RepeatPassword { get; set; }

    public string[] Roles => new[] { ADMIN, GAMER, VIP };

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, UserForChangePasswordDto>
    {
        private readonly IMapper _mapper;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserRepository _userDal;

        public ChangePasswordCommandHandler(IMapper mapper, AuthBusinessRules authBusinessRules, IUserRepository userDal)
        {
            _mapper = mapper;
            _authBusinessRules = authBusinessRules;
            _userDal = userDal;
        }

        public async Task<UserForChangePasswordDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await _authBusinessRules.PasswordsEnteredMustBeTheSame(request.NewPassword,request.RepeatPassword);
            await _authBusinessRules.UserEmailMustBeAvailable(request.Email);

            byte[] passwordHash, passwordSalt;

            User? user = await _userDal.GetAsync(u => u.Email == request.Email);

            HashingHelper.CreatePasswordHash(request.NewPassword, out passwordHash, out passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            User updatedUser= await _userDal.UpdateAsync(user);
            UserForChangePasswordDto updatedUserDto = _mapper.Map<UserForChangePasswordDto>(updatedUser);
            updatedUserDto.Password = request.NewPassword;

            return updatedUserDto;

        }
    }
}
