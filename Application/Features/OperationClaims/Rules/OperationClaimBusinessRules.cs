using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exceptions;
using Application.Services.Repositories;
using Domain.Concrete;
using Application.Features.OperationClaims.Constants;

namespace Application.Features.OperationClaims.Rules;

public class OperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IOperationClaimRepository _operationClaimDal;

    public OperationClaimBusinessRules(IOperationClaimRepository operationClaimDal)
    {
        _operationClaimDal = operationClaimDal;
    }

    public async Task OperationClaimIdShouldExistWhenSelected(int? id)
    {
        OperationClaim? result = await _operationClaimDal.GetAsync(b => b.Id == id);
        if (result == null) throw new BusinessException(OperationClaimMessages.OperationClaimNotFound);
    }
    public async Task OperationClaimNameShouldBeNotExists(string name)
    {
        OperationClaim? user = await _operationClaimDal.GetAsync(u => u.Name.ToLower() == name.ToLower());
        if (user != null) throw new BusinessException(OperationClaimMessages.OperationClaimNameAlreadyExists);
    }

    public async Task OperationMustBeAvailable()
    {
        List<OperationClaim>? results = _operationClaimDal.GetAll();
        if (results.Count <= 0) throw new BusinessException(OperationClaimMessages.OperationClaimNotFound);
    }
}
