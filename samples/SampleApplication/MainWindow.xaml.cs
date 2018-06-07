using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlertBarWpfExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string LONG_TEXT = $"This is the first line of a very long text.{Environment.NewLine}This second line is supposed to be as long possible and therefore this text almost useless but therefore pretty long.{Environment.NewLine}I'm a third and last line in this text.";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnWarning_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            setIsIconVisible();

            var text = "This is a warning." + getText();

            if (chkUseOutlineTheme.IsChecked.HasValue && chkUseOutlineTheme.IsChecked != true)
            {
                msgbar.SetWarningAlert(text);
            }
            else
            {
                msgbarOutline.SetWarningAlert(text);
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            setIsIconVisible();

            var text = "This is an information." + getText();

            if (chkUseOutlineTheme.IsChecked.HasValue && chkUseOutlineTheme.IsChecked != true)
            {
                msgbar.SetInformationAlert(text);
            }
            else
            {
                msgbarOutline.SetInformationAlert(text);
            }
        }

        private void btnSuccess_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            setIsIconVisible();

            var text = "This is a success." + getText();

            if (chkUseOutlineTheme.IsChecked.HasValue && chkUseOutlineTheme.IsChecked != true)
            {
                msgbar.SetSuccessAlert(text);
            }
            else
            {
                msgbarOutline.SetSuccessAlert(text);
            }
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            clearAll();
            setIsIconVisible();

            var text = "This is an error." + getText();

            if (chkUseOutlineTheme.IsChecked.HasValue && chkUseOutlineTheme.IsChecked != true)
            {
                msgbar.SetDangerAlert(text);
            }
            else
            {
                msgbarOutline.SetDangerAlert(text);
            }
        }

        private void clearAll()
        {
            msgbar.Clear();
            msgbarOutline.Clear();
        }

        private void setIsIconVisible()
        {
            if (chkDisplayIcons.IsChecked.HasValue && chkDisplayIcons.IsChecked != true)
            {
                msgbar.IsIconVisible = false;
                msgbarOutline.IsIconVisible = false;
            }
            else
            {
                msgbar.IsIconVisible = true;
                msgbarOutline.IsIconVisible = true;
            }
        }

        private string getText()
        {
            var result = String.Empty;

            if (chkUseLongText.IsChecked.HasValue && chkUseLongText.IsChecked == true)
            {
                result = $"{Environment.NewLine}{LONG_TEXT}";
            }
            return result;
        }
    }
}
