using DigitalBanking.Services.Contracts;

namespace DigitalBanking.Services.Implementation
{
    public class OneTimePinService : IOTPService
    {
        public void GenerateOTP()
        {
            throw new NotImplementedException();
        }

        public bool ValidateOTP()
        {
            return false;
        }
    }
}
