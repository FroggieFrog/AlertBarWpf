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

namespace AlertBarWpf
{
    public partial class AlertBarWpf : UserControl
    {
        private ThemeType theme = ThemeType.Standard;

        public enum ThemeType
        {
            Standard = 0,
            Outline = 1
        }

        public AlertBarWpf()
        {
            InitializeComponent();
        }

        #region Show event

        public static readonly RoutedEvent ShowEvent = EventManager.RegisterRoutedEvent(nameof(Show), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(AlertBarWpf));

        public event RoutedEventHandler Show
        {
            add { AddHandler(ShowEvent, value); }
            remove { RemoveHandler(ShowEvent, value); }
        }

        private void raiseShowEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(AlertBarWpf.ShowEvent);
            RaiseEvent(newEventArgs);
        }

        #endregion

        #region properties

        /// <summary>
        /// Hide or show icons in the messages.
        /// </summary>
        public bool IsIconVisible { get; set; } = true;

        public ThemeType Theme
        {
            get { return theme; }
            set
            {
                if (Enum.IsDefined(typeof(ThemeType), value))
                {
                    theme = value;
                }
                else
                {
                    theme = ThemeType.Standard;
                }
            }
        }

        public TextWrapping TextWrappingToUse { get; set; } = TextWrapping.NoWrap;

        #endregion       

        /// <summary>
        /// Shows a Danger Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetDangerAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#D9534F";
            string icon = "danger_16.png";
            transformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Shows a warning Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetWarningAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#F0AD4E";
            string icon = "warning_16.png";

            transformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Shows a Success Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetSuccessAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#5CB85C";
            string icon = "success_16.png";
            transformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Shows an Information Alert
        /// </summary>
        /// <param name="message">The message for the alert</param>
        /// <param name="timeoutInSeconds">Alert will auto-close in this amount of seconds</param>
        public void SetInformationAlert(string message, int timeoutInSeconds = 0)
        {
            string color = "#5BC0DE";
            string icon = "information_16.png";
            transformStage(message, timeoutInSeconds, color, icon);
        }

        /// <summary>
        /// Remove a message if one is currently being shown.
        /// </summary>
        public void Clear()
        {
            grdWrapper.Visibility = Visibility.Collapsed;
        }

        #region eventhandler

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Clear();

        }

        private void AnimationObject_Completed(object sender, EventArgs e)
        {
            if (grdWrapper.Opacity == 0)
            {
                //If you call msgbar.setErrorMessage("Whateva") in MainWindow() of your WPF the window is not rendered yet.  So opacity is 0.  If you have a timeout of 0 then it would call this immediately
                if (key1.KeyTime.TimeSpan.Seconds > 0)
                {
                    Clear();
                }
            }
        }

        #endregion

        #region helper methods

        private void transformStage(string messageToDisplay, int secs, string colorhex, string iconsrc)
        {
            SolidColorBrush backgroundBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom(colorhex));
            //alternative:
            //SolidColorBrush backgroundBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorhex));

            Grid parentGrid;
            switch (theme)
            {
                case ThemeType.Standard:
                    spStandard.Visibility = Visibility.Visible;
                    spOutline.Visibility = Visibility.Collapsed;

                    parentGrid = findVisualChildren<Grid>(spStandard).FirstOrDefault();
                    parentGrid.Background = backgroundBrush;
                    break;
                case ThemeType.Outline:
                default:
                    spStandard.Visibility = Visibility.Collapsed;
                    spOutline.Visibility = Visibility.Visible;

                    parentGrid = findVisualChildren<Grid>(spOutline).FirstOrDefault();
                    bdr.BorderBrush = backgroundBrush;
                    break;
            }

            TextBlock lblMessage = findVisualChildren<TextBlock>(parentGrid).FirstOrDefault();
            List<Image> imgs = findVisualChildren<Image>(parentGrid).ToList();
            Image imgStatusIcon = imgs[0];
            Image imgCloseIcon = imgs[1];

            if (!IsIconVisible)
            {
                imgStatusIcon.Visibility = Visibility.Collapsed;
                parentGrid.ColumnDefinitions[0].Width = GridLength.Auto;
                lblMessage.Margin = new Thickness(10, 4, 0, 4);
            }
            else
            {
                imgStatusIcon.Visibility = Visibility.Visible;
                parentGrid.ColumnDefinitions[0].Width = new GridLength(26, GridUnitType.Pixel);
                lblMessage.Margin = new Thickness(5, 0, 5, 0);

                imgStatusIcon.Source = new BitmapImage(new Uri("/AlertBarWpf;component/Resources/" + iconsrc, UriKind.Relative));
            }

            lblMessage.Text = messageToDisplay;
            lblMessage.TextWrapping = TextWrappingToUse;

            grdWrapper.Visibility = Visibility.Visible;
            key1.KeyTime = new TimeSpan(0, 0, (secs == 0 ? 0 : secs - 1));
            key2.KeyTime = new TimeSpan(0, 0, secs);
            raiseShowEvent();
        }

        //https://stackoverflow.com/a/978352
        private static IEnumerable<T> findVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }

                foreach (T childOfChild in findVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        //https://stackoverflow.com/a/32902458
        private static IEnumerable<T> findVisualChildrenRecursive<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                yield break;
            }

            if (dependencyObject is T)
            {
                yield return (T)dependencyObject;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                foreach (T childOfChild in findVisualChildrenRecursive<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }

        //https://stackoverflow.com/a/23836877
        private static IEnumerable<T> findLogicalChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                yield break;
            }

            foreach (object rawChild in LogicalTreeHelper.GetChildren(dependencyObject))
            {
                if (rawChild is DependencyObject)
                {
                    DependencyObject child = (DependencyObject)rawChild;
                    if (child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in findLogicalChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        #endregion

    }
}
