using System;
using TraceSourceLogger;

namespace TradeHubGui.Common.ApplicationSecurity
{
    /// <summary>
    /// Provides Component License Validation
    /// </summary>
    public static class TradeSharpLicenseManager
    {
        private static TradeSharpLicense _tradeSharpLicense;

        /// <summary>
        /// Gets a license for an instance or type of component.
        /// </summary>
        public static TradeSharpLicense GetLicense()
        {
            try
            {
                // Create TradeSharp licene or return existing one
                return _tradeSharpLicense ?? (_tradeSharpLicense = TradeSharpLicense.CreateLicense());
            }
            catch (Exception exception)
            {
                Logger.Error("Unable to create License", "", "");
                return TradeSharpLicense.CreateInvalidLicense();
            }
        }

        /// <summary>
        /// Updates the license for an instance or type of component.
        /// </summary>
        public static TradeSharpLicense UpdateLicense()
        {
            try
            {
                // Create TradeSharp licene or return existing one
                return _tradeSharpLicense = TradeSharpLicense.CreateLicense();
            }
            catch (Exception exception)
            {
                Logger.Error("Unable to create License", "", "");
                return TradeSharpLicense.CreateInvalidLicense();
            }
        }
    }
}
