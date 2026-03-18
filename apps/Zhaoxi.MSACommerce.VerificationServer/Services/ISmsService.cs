using Zhaoxi.MSACommerce.SharedKernel.Result;

namespace Zhaoxi.MSACommerce.VerificationServer.Services;

public interface ISmsService
{
    Task<Result> SendCodeAsync(string phoneNumber);
    
    Task<Result> VerifyCodeAsync(string phoneNumber, string inputCode);
}