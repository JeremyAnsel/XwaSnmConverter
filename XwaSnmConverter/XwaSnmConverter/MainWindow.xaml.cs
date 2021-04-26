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

        private void smOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";

            if (dialog.ShowDialog(this) == true)
            {
                this.smSnmFileName.Text = dialog.FileName;
                this.smMp4FileName.Text = System.IO.Path.ChangeExtension(dialog.FileName, "mp4");
            }
        }

        private void smOpenMp4_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.smSnmFileName.Text))
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "mp4";
            dialog.Filter = "mp4 files (*.mp4)|*.mp4";
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.smSnmFileName.Text);
            dialog.FileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(this.smSnmFileName.Text), "mp4");

            if (dialog.ShowDialog(this) == true)
            {
                this.smMp4FileName.Text = dialog.FileName;
            }
        }

        private void smConvert_Click(object sender, RoutedEventArgs e)
        {
            string snm = this.smSnmFileName.Text;
            string mp4 = this.smMp4FileName.Text;

            if (string.IsNullOrEmpty(snm) || string.IsNullOrEmpty(mp4))
            {
                return;
            }

            this.RunBusyAction(disp =>
            {
                try
                {
                    JeremyAnsel.Xwa.Snm.SnmFile
                        .FromFile(snm)
                        .SaveAsMp4(mp4);

                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, string.Concat(mp4, " created."), this.Title, MessageBoxButton.OK));
                }
                catch (Exception ex)
                {
                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error));
                }
            });
        }

        private void msOpenMf_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "mp4 files|*.mp4|avi files|*.avi|video files|*.*";

            if (dialog.ShowDialog(this) == true)
            {
                this.msMfFileName.Text = dialog.FileName;
                this.msSnmFileName.Text = System.IO.Path.ChangeExtension(dialog.FileName, "snm");
            }
        }

        private void msOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.msMfFileName.Text))
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "snm";
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.msMfFileName.Text);
            dialog.FileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(this.msMfFileName.Text), "snm");

            if (dialog.ShowDialog(this) == true)
            {
                this.msSnmFileName.Text = dialog.FileName;
            }
        }

        private void msConvert_Click(object sender, RoutedEventArgs e)
        {
            string mf = this.msMfFileName.Text;
            string snm = this.msSnmFileName.Text;

            if (string.IsNullOrEmpty(mf) || string.IsNullOrEmpty(snm))
            {
                return;
            }

            this.RunBusyAction(disp =>
            {
                try
                {
                    JeremyAnsel.Xwa.Snm.SnmFile
                        .FromMFFile(mf)
                        .Save(snm);

                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, string.Concat(snm, " created."), this.Title, MessageBoxButton.OK));
                }
                catch (Exception ex)
                {
                    disp(() => Xceed.Wpf.Toolkit.MessageBox.Show(this, ex.ToString(), this.Title, MessageBoxButton.OK, MessageBoxImage.Error));
                }
            });
        }

        private void saOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.CheckFileExists = true;
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";

            if (dialog.ShowDialog(this) == true)
            {
                this.saSnmFileName.Text = dialog.FileName;
                this.saAviFileName.Text = System.IO.Path.ChangeExtension(dialog.FileName, "avi");
            }
        }

        private void saOpenAvi_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.saSnmFileName.Text))
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "avi";
            dialog.Filter = "avi files (*.avi)|*.avi";
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.saSnmFileName.Text);
            dialog.FileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(this.saSnmFileName.Text), "avi");

            if (dialog.ShowDialog(this) == true)
            {
                this.saAviFileName.Text = dialog.FileName;
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
                        .SaveAsAviMpeg4(avi);

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
                this.asSnmFileName.Text = System.IO.Path.ChangeExtension(dialog.FileName, "snm");
            }
        }

        private void asOpenSnm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.asAviFileName.Text))
            {
                return;
            }

            var dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "snm";
            dialog.Filter = "snm files (*.snm, *.znm)|*.snm;*.znm";
            dialog.InitialDirectory = System.IO.Path.GetDirectoryName(this.asAviFileName.Text);
            dialog.FileName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(this.asAviFileName.Text), "snm");

            if (dialog.ShowDialog(this) == true)
            {
                this.asSnmFileName.Text = dialog.FileName;
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
