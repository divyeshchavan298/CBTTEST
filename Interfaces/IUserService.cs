using DemoTask.Models;

namespace DemoTask.Interfaces
{
    public interface IUserService
    {
        IResult CheckLogin(LoginReq loginReq);
        IResult RegisterUser(RegisterUserReq registerUser);
        IResult GenerateMobileOTP(GenerateMobileOtpReq objOtpGen);
        IResult GenerateEmailOTP(GenerateEmailOtpReq objOtpGen);
        IResult VerifyOtp(VerifyOtpReq objVerifyOtp);
        IResult UpdatePrivacyPolicy(UpdatePrivacyPolicyReq objUpdatePrivacy);
        IResult UpdateAccountPin(UpdateAccountPinReq objUpdateAccountPin);
        IResult UpdateBiometric(BiometricReq objUpdateAccountPin);
        IResult VerifyBiometric(BiometricReq objBiometric);
        IResult VerifyAccountPin(VerifyPinReq objVerifyPin);
    }
}
