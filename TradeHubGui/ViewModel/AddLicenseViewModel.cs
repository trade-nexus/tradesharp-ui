using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MessageBoxUtils;
using TradeHub.NotificationEngine.Common.Utility;
using TradeHubGui.Common;
using TradeHubGui.Common.ApplicationSecurity;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.ViewModel
{
    public class AddLicenseViewModel : BaseViewModel
    {
        private static Type _type = typeof (AddLicenseViewModel);

        #region Fields
        
        private RelayCommand _addLicense;

        #endregion

        #region Commands
        /// <summary>
        /// Command used with 'Save' button
        /// </summary>
        public ICommand AddLicenseCommand
        {
            get
            {
                return _addLicense ??
                       (_addLicense = new RelayCommand(param => AddLicenseExecute()));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// License Expiration Date
        /// </summary>
        public string ExpirationDate
        {
            get { return TradeSharpLicenseManager.GetLicense().ExpirationDate.Date.ToString("D"); }
        }

        /// <summary>
        /// License Subscription Type
        /// </summary>
        public string LicenseType
        {
            get { return TradeSharpLicenseManager.GetLicense().LicenseSubscriptionType; }
        }

        /// <summary>
        /// Client Details
        /// </summary>
        public string ClientDetails
        {
            get { return TradeSharpLicenseManager.GetLicense().ClientDetails; }
        }

        #endregion
        
        #region Command Trigger Methods

        /// <summary>
        /// Called when 'Select License File' button is clicked
        /// </summary>
        private void AddLicenseExecute()
        {
            AddLicense();
        }

        #endregion

        /// <summary>
        /// Open dialog to select license and then add it to the application
        /// </summary>
        private void AddLicense()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "Import License";
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "OBJ Files (.obj)|*.obj|All Files (*.*)|*.*";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                File.Copy(openFileDialog.FileName, "TradeSharpLicense.obj", true);
                var newLicense = TradeSharpLicenseManager.UpdateLicense();

                if (newLicense.LicenseSubscriptionType.Equals(Common.ApplicationSecurity.LicenseType.Invalid.ToString()))
                {
                    WPFMessageBox.Show(MainWindow, "The license file is invalid.\n\nPlease contact TradeSharp support at: tradesharp@aurorasolutions.io", "TRADESHARP",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    WPFMessageBox.Show(MainWindow, "New License added.\n\nPlease restart the application", "TRADESHARP",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
