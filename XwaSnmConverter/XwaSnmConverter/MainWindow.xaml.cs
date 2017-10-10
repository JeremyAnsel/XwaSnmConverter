using Microsoft.Win32;
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

namespace XwaSnmConverter
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunBusyAction(Action action)
        {
            this.RunBusyAction(dispatcher => action());
        }

        private void RunBusyAction(Action<Action<Action>> action)
        {
            this.BusyIndicator.IsBusy = true;

            Action<Action> dispatcherAction = a =>
            {
                this.Dispatcher.Invoke(a);
            };

            Task.Factory.StartNew(state =>
            {
                var disp = (Action<Action>)state;
                disp(() => { this.BusyIndicator.IsBusy = true; });

                try
                {
                    action(disp);
                }
                catch (Exception ex)
                {
                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error));
                }

                disp(() => { this.BusyIndicator.IsBusy = false; });
            }, dispatcherAction);
        }

        private void saOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";

            if (dialog.ShowDialog(this) == true)
            {
                this.saSnmFileName.Text = dialog.FileName;
            }
            else
            {
                this.saSnmFileName.Text = null;
            }
        }

        private void saOpenAvi_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "avi";
            dialog.Filter = "avi files (*.avi)|*.avi";

            if (dialog.ShowDialog(this) == true)
            {
                this.saAviFileName.Text = dialog.FileName;
            }
            else
            {
                this.saAviFileName.Text = null;
            }
        }

        private void saConvert_Click(object sender, RoutedEventArgs e)
        {
            string snm = this.saSnmFileName.Text;
            string avi = this.saAviFileName.Text;

            if (string.IsNullOrEmpty(snm) || string.IsNullOrEmpty(avi))
            {
                return;
            }

            this.RunBusyAction(disp =>
            {
                try
                {
                    JeremyAnsel.Xwa.Snm.SnmFile
                        .FromFile(snm)
                        .SaveAsAvi(avi);

                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, string.Concat(avi, " created."), this.Title, MessageBoxButton.OK));
                }
                catch (Exception ex)
                {
                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error));
                }
            });
        }

        private void asOpenAvi_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "avi files (*.avi)|*.avi";

            if (dialog.ShowDialog(this) == true)
            {
                this.asAviFileName.Text = dialog.FileName;
            }
            else
            {
                this.asAviFileName.Text = null;
            }
        }

        private void asOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "snm";
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";

            if (dialog.ShowDialog(this) == true)
            {
                this.asSnmFileName.Text = dialog.FileName;
            }
            else
            {
                this.asSnmFileName.Text = null;
            }
        }

        private void asConvert_Click(object sender, RoutedEventArgs e)
        {
            string avi = this.asAviFileName.Text;
            string snm = this.asSnmFileName.Text;

            if (string.IsNullOrEmpty(avi) || string.IsNullOrEmpty(snm))
            {
                return;
            }

            this.RunBusyAction(disp =>
            {
                try
                {
                    JeremyAnsel.Xwa.Snm.SnmFile
                        .FromAviFile(avi)
                        .Save(snm);

                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, string.Concat(snm, " created."), this.Title, MessageBoxButton.OK));
                }
                catch (Exception ex)
                {
                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error));
                }
            });
        }
    }
}
