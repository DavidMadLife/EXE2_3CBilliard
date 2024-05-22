using System;
using System.Collections.Concurrent;

namespace EXE201_3CBilliard_Service.Service
{
    public class OtpManager
    {
        private readonly ConcurrentDictionary<string, (string Otp, DateTime Expiry)> _otps = new();
        private readonly TimeSpan _expiryDuration = TimeSpan.FromMinutes(3);

        public string GenerateOtp(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            var expiry = DateTime.UtcNow.Add(_expiryDuration);
            _otps[email] = (otp, expiry);
            return otp;
        }

        public bool ValidateOtp(string email, string otp)
        {
            if (_otps.TryGetValue(email, out var otpData))
            {
                if (otpData.Expiry > DateTime.UtcNow && otpData.Otp == otp)
                {
                    _otps.TryRemove(email, out _);
                    return true;
                }
            }
            return false;
        }

        public void RemoveOtp(string email)
        {
            _otps.TryRemove(email, out _);
        }
    }
}
