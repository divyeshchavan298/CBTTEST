using DemoTask.DAL;
using DemoTask.Interfaces;
using DemoTask.Models;
using Microsoft.VisualBasic;
using System.Numerics;
using System.Text.Json;
using static DemoTask.Helper.Enums;

namespace DemoTask.Services
{
    public class UserService : IUserService
    {
        private readonly CbtDbContext _cbtDbContext;
        private readonly Env _env;
        public UserService(CbtDbContext cbtDbContext, Env env)
        {
            _cbtDbContext = cbtDbContext;
            _env = env;
        }

        public IResult CheckLogin(LoginReq objLoginReq)
        {
            try
            {
                if (objLoginReq == null)
                {
                    return Results.BadRequest("Invalid Input");
                }

                if (string.IsNullOrEmpty(objLoginReq.sIcNumber))
                {
                    return Results.BadRequest("Invalid Input");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objLoginReq.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.NotFound("IC Number not available.");
                }

                UserRes objUser = new UserRes()
                {
                    sCustomerName = objClient.sCustomerName,
                    sEmail = objClient.sEmail,
                    sIcNumber = objClient.sIcNumber,
                    sMobileNo = objClient.sMobileNo,
                    bPrivacyPolicy = objClient.bPrivacyPolicy,
                };
                //return Results.Ok(JsonSerializer.Serialize(objUser));
                return Results.Ok(objUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IResult RegisterUser(RegisterUserReq objRegisterUser)
        {
            try
            {
                if (objRegisterUser == null)
                {
                    return Results.BadRequest("Invalid Input");
                }

                if (string.IsNullOrEmpty(objRegisterUser.sIcNumber) || string.IsNullOrEmpty(objRegisterUser.sCustomerName) ||
                    string.IsNullOrEmpty(objRegisterUser.sEmail) || string.IsNullOrEmpty(objRegisterUser.sMobileNo))
                {
                    return Results.BadRequest("Invalid Input");
                }

                ClientMaster check = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objRegisterUser.sIcNumber).FirstOrDefault();

                if (check != null)
                {
                    return Results.BadRequest("Account Already Exits");
                }

                ClientMaster objClientMaster = new ClientMaster()
                {
                    sIcNumber = objRegisterUser.sIcNumber,
                    sMobileNo = objRegisterUser.sMobileNo,
                    bPrivacyPolicy = false,//at the time of registration
                    sCustomerName = objRegisterUser.sCustomerName,
                    sEmail = objRegisterUser.sEmail,
                    dtCreated = DateTime.Now,
                    dtLastUpdated = DateTime.Now,
                    sPin = "",
                };

                _cbtDbContext.clientMaster.Add(objClientMaster);
                _cbtDbContext.SaveChanges();

                UserRes objUser = new UserRes()
                {
                    sCustomerName = objRegisterUser.sCustomerName,
                    sEmail = objRegisterUser.sEmail,
                    sIcNumber = objRegisterUser.sIcNumber,
                    sMobileNo = objRegisterUser.sMobileNo,
                    bPrivacyPolicy = objClientMaster.bPrivacyPolicy,
                };
                return Results.Ok(objUser);
                //return Results.Ok(JsonSerializer.Serialize(objUser));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //We can merge this two Generate Otp functions into one by handling on Otp Type
        #region Generate Otp
        public IResult GenerateMobileOTP(GenerateMobileOtpReq objOtpGen)
        {
            try
            {
                if (objOtpGen == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objOtpGen.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (String.IsNullOrEmpty(objOtpGen.sMobileNo))
                {
                    return Results.BadRequest("Invalid Mobile Number");
                }

                int OtpLen = Convert.ToInt32(_env.OTP_Length);

                if (OtpLen <= 0)
                {
                    //We can write log here to check otp length
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
                string Otp = Helper.Helper.GenerateRandomInt(OtpLen);
                DateTime OtpGentnTime = DateTime.Now;

                OtpMaster objOtpMaster = _cbtDbContext.otpMasters.Where(x => x.sIcNumber == objOtpGen.sIcNumber
                    && x.sOtpFor == objOtpGen.sMobileNo && x.bOtpVerify == false
                    && x.nOtpType == (short)enumOtpType.Mobile).FirstOrDefault();


                if (objOtpMaster != null)
                {
                    var otpGrnTime = objOtpMaster.dtOtpGentnTime;
                    TimeSpan difference = (TimeSpan)(DateTime.Now - otpGrnTime);
                    int diff = Helper.Helper.OTPExceedTime - difference.Minutes;
                    if (Helper.Helper.OTPExceedTime > difference.Minutes)
                    {
                        if (objOtpMaster.nResendOTPCount >= Convert.ToInt32(_env.OTPRESEND_LIMIT))
                        {
                            return Results.Ok("You have exceeded limit. Please wait " + diff + " minute before trying again");
                        }
                    }
                    else
                    {
                        if (objOtpMaster.nResendOTPCount >= Convert.ToInt32(_env.OTPRESEND_LIMIT))
                        {
                            objOtpMaster.nResendOTPCount = 0;
                        }
                    }
                }
                else
                {
                    objOtpMaster = new OtpMaster();
                    objOtpMaster.sIcNumber = objOtpGen.sIcNumber;
                    objOtpMaster.sOtpFor = objOtpGen.sMobileNo;
                    objOtpMaster.bOtpVerify = false;
                    objOtpMaster.nOtpType = (short)enumOtpType.Mobile;
                }


                //
                //On here we will send otp using any third party services 
                //

                objOtpMaster.sOtp = Otp;
                objOtpMaster.dtOtpGentnTime = OtpGentnTime;
                if (objOtpMaster.nResendOTPCount == 0)
                {
                    objOtpMaster.nResendOTPCount++;
                }
                else
                {
                    objOtpMaster.nResendOTPCount++;
                }
                _cbtDbContext.otpMasters.Update(objOtpMaster);
                _cbtDbContext.SaveChanges();

                return Results.Ok("Otp Sent Successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult GenerateEmailOTP(GenerateEmailOtpReq objOtpGen)
        {
            try
            {
                if (objOtpGen == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objOtpGen.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (String.IsNullOrEmpty(objOtpGen.sEmail))
                {
                    return Results.BadRequest("Invalid Email");
                }

                int OtpLen = Convert.ToInt32(_env.OTP_Length);

                if (OtpLen <= 0)
                {
                    //We can write log here to check otp length
                    return Results.StatusCode(StatusCodes.Status500InternalServerError);
                }
                string Otp = Helper.Helper.GenerateRandomInt(OtpLen);
                DateTime OtpGentnTime = DateTime.Now;

                OtpMaster objOtpMaster = _cbtDbContext.otpMasters.Where(x => x.sIcNumber == objOtpGen.sIcNumber
                    && x.sOtpFor == objOtpGen.sEmail && x.bOtpVerify == false
                    && x.nOtpType == (short)enumOtpType.Email).FirstOrDefault();


                if (objOtpMaster != null)
                {
                    var otpGrnTime = objOtpMaster.dtOtpGentnTime;
                    TimeSpan difference = (TimeSpan)(DateTime.Now - otpGrnTime);
                    int diff = Helper.Helper.OTPExceedTime - difference.Minutes;
                    if (Helper.Helper.OTPExceedTime > difference.Minutes)
                    {
                        if (objOtpMaster.nResendOTPCount >= Convert.ToInt32(_env.OTPRESEND_LIMIT))
                        {
                            return Results.Ok("You have exceeded limit. Please wait " + diff + " minute before trying again");
                        }
                    }
                    else
                    {
                        if (objOtpMaster.nResendOTPCount >= Convert.ToInt32(_env.OTPRESEND_LIMIT))
                        {
                            objOtpMaster.nResendOTPCount = 0;
                        }
                    }
                }
                else
                {
                    objOtpMaster = new OtpMaster();
                    objOtpMaster.sIcNumber = objOtpGen.sIcNumber;
                    objOtpMaster.sOtpFor = objOtpGen.sEmail;
                    objOtpMaster.bOtpVerify = false;
                    objOtpMaster.nOtpType = (short)enumOtpType.Email;
                }


                //
                //On here we will send email otp using any third party services 
                //

                objOtpMaster.sOtp = Otp;
                objOtpMaster.dtOtpGentnTime = OtpGentnTime;
                if (objOtpMaster.nResendOTPCount == 0)
                {
                    objOtpMaster.nResendOTPCount++;
                }
                else
                {
                    objOtpMaster.nResendOTPCount++;
                }
                _cbtDbContext.otpMasters.Update(objOtpMaster);
                _cbtDbContext.SaveChanges();

                return Results.Ok("Otp Sent Successfully");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion

        public IResult VerifyOtp(VerifyOtpReq objVerifyOtp)
        {
            try
            {
                if (objVerifyOtp == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objVerifyOtp.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (!new short[] { (short)enumOtpType.Mobile, (short)enumOtpType.Email }.Contains(objVerifyOtp.nOtpType))
                {
                    return Results.BadRequest("Invalid Otp Type");
                }

                if (String.IsNullOrEmpty(objVerifyOtp.sOtpFor))
                {
                    if (objVerifyOtp.nOtpType == (short)enumOtpType.Email)
                    {
                        return Results.BadRequest("Invalid Email");
                    }
                    if (objVerifyOtp.nOtpType == (short)enumOtpType.Mobile)
                    {
                        return Results.BadRequest("Invalid Mobile Number");
                    }
                }

                OtpMaster objOtpMaster = _cbtDbContext.otpMasters.Where(x => x.sIcNumber == objVerifyOtp.sIcNumber
                    && x.sOtpFor == objVerifyOtp.sOtpFor && x.bOtpVerify == false
                    && x.nOtpType == objVerifyOtp.nOtpType).FirstOrDefault(); //nOtpType in Case Of Mobile = 0, Email = 1

                if (objOtpMaster == null)
                {
                    return Results.BadRequest("Invalid Otp");
                }

                string[] sExpiryTime = _env.OTPEXPIRE_TIME.Split(":");
                int nMinute = Convert.ToInt32(sExpiryTime[0]);
                int nSecond = Convert.ToInt32(sExpiryTime[1]);

                TimeSpan threshold = TimeSpan.FromMinutes(nMinute) + TimeSpan.FromSeconds(nSecond);

                var otpGrnTime = objOtpMaster.dtOtpGentnTime;
                TimeSpan difference = (TimeSpan)(DateTime.Now - otpGrnTime);
                if (difference < threshold)
                {
                    //"The time difference is less than specified time
                    if (objOtpMaster.sOtp == objVerifyOtp.sOtp)
                    {
                        objOtpMaster.bOtpVerify = true;
                        _cbtDbContext.otpMasters.Update(objOtpMaster);
                        _cbtDbContext.SaveChanges();

                        return Results.Ok("OTP verified successfully");
                    }
                    return Results.BadRequest("Invalid Otp");
                }
                return Results.BadRequest("Otp Expired");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult UpdatePrivacyPolicy(UpdatePrivacyPolicyReq objUpdatePrivacy)
        {
            try
            {

                if (objUpdatePrivacy == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objUpdatePrivacy.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objUpdatePrivacy.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.BadRequest("IC Number not available");
                }

                objClient.bPrivacyPolicy = objUpdatePrivacy.bPrivacyPolicy;
                _cbtDbContext.clientMaster.Update(objClient);
                _cbtDbContext.SaveChanges();

                return Results.Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult UpdateAccountPin(UpdateAccountPinReq objUpdateAccountPin)
        {
            try
            {

                if (objUpdateAccountPin == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objUpdateAccountPin.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (objUpdateAccountPin.sPin != objUpdateAccountPin.sConfirmPin)
                {
                    return Results.BadRequest("Unmatched PIN");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objUpdateAccountPin.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.BadRequest("IC Number not available");
                }

                objClient.sPin = objUpdateAccountPin.sPin; //We also can encrypt and store this pin
                _cbtDbContext.clientMaster.Update(objClient);
                _cbtDbContext.SaveChanges();

                return Results.Json("Pin Updated Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult UpdateBiometric(BiometricReq objBiometric)
        {
            try
            {

                if (objBiometric == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objBiometric.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (String.IsNullOrEmpty(objBiometric.sBiometric))
                {
                    return Results.BadRequest("Invalid Biometric");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objBiometric.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.BadRequest("IC Number not available");
                }

                objClient.sBiometric = objBiometric.sBiometric;
                _cbtDbContext.clientMaster.Update(objClient);
                _cbtDbContext.SaveChanges();

                return Results.Json("Biometric Updated Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult VerifyBiometric(BiometricReq objBiometric)
        {
            try
            {
                if (objBiometric == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objBiometric.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (String.IsNullOrEmpty(objBiometric.sBiometric))
                {
                    return Results.BadRequest("Invalid Biometric");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objBiometric.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.BadRequest("IC Number not available");
                }

                if (objClient.sBiometric.Trim() != objBiometric.sBiometric.Trim())
                {
                    return Results.BadRequest("Biometric Not Matched");
                }

                return Results.Json("Biometric Matched");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        public IResult VerifyAccountPin(VerifyPinReq objVerifyPin)
        {
            try
            {

                if (objVerifyPin == null)
                {
                    return Results.BadRequest("Invalid input");
                }

                if (String.IsNullOrEmpty(objVerifyPin.sIcNumber))
                {
                    return Results.BadRequest("Invalid IC Number");
                }

                if (String.IsNullOrEmpty(objVerifyPin.sPin))
                {
                    return Results.BadRequest("Unmatched PIN");
                }

                ClientMaster objClient = _cbtDbContext.clientMaster.Where(x => x.sIcNumber == objVerifyPin.sIcNumber).FirstOrDefault();
                if (objClient == null)
                {
                    return Results.BadRequest("IC Number not available");
                }

                if (objClient.sPin.Trim() != objVerifyPin.sPin.Trim())
                {
                    return Results.BadRequest("Pin Not Matched");
                }

                return Results.Json("Pin Matched");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginRequest() : Exception Message = {ex.Message}, StactTrace = {ex.StackTrace}");
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
