namespace DigitalBanking.Services.Contracts
{
    public interface IOTPService
    {
        public void GenerateOTP();

        public bool ValidateOTP();

    }
}
