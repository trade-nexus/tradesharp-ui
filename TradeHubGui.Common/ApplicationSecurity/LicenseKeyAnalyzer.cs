using System;
using System.Globalization;

namespace TradeHubGui.Common.ApplicationSecurity
{
    internal class LicenseKeyAnalyzer
    {
        /// <summary>
        /// default constructor
        /// </summary>
        internal LicenseKeyAnalyzer()
        {

        }

        /// <summary>
        /// Retrieves License details
        /// </summary>
        /// <returns></returns>
        internal Tuple<LicenseType, string, DateTime> GetLicenseInformation()
        {
            Decryptor decryptor = new Decryptor();
            var licenseInformation = decryptor.DecryptLicense(ReadLicenseFile());

            return new Tuple<LicenseType, string, DateTime>(
                FindLicenseType(licenseInformation.Item1),
                licenseInformation.Item2.Trim(),
                ExtracetExpirationDate(licenseInformation.Item3));
        }

        /// <summary>
        /// Reads data from given license file
        /// </summary>
        /// <returns></returns>
        private byte[] ReadLicenseFile()
        {
            BinaryFileReader binaryFileReader = new BinaryFileReader();

            return binaryFileReader.Read();
        }

        /// <summary>
        /// Extracts license type
        /// </summary>
        /// <param name="licenseType"></param>
        /// <returns></returns>
        private LicenseType FindLicenseType(string licenseType)
        {
            switch (licenseType.Trim().ToLower())
            {
                case "monthly":
                    return LicenseType.Monthly;
                case "annual":
                    return LicenseType.Annual;
                case "lifetime":
                    return LicenseType.LifeTime;
                default:
                    return LicenseType.Demo;
            }
        }

        /// <summary>
        /// Extracts expiration date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime ExtracetExpirationDate(string date)
        {
            return DateTime.ParseExact(date.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture);
        }
    }
}
