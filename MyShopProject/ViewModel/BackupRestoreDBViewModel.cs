using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyShopProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MyShopProject.ViewModel
{
    public class BackupRestoreDBViewModel :BaseViewModel
    {

        private string _backupPath;
        public string BackupPath
        {
            get { return _backupPath; }
            set
            {
                _backupPath = value;
                OnPropertyChanged(nameof(BackupPath));
            }
        }

        private string _restorePath;

        public string RestorePath
        {
            get { return _restorePath; }
            set
            {
                _restorePath = value;
                OnPropertyChanged(nameof(RestorePath));
            }
        }

        private string _progressBackupText="0%";
        public string ProgressBackupText
        {
            get { return _progressBackupText; }
            set
            {
                _progressBackupText = value;
                OnPropertyChanged(nameof(ProgressBackupText));
            }
        }

        private string _progressRestoreText="0%";
        public string ProgressRestoreText
        {
            get { return _progressRestoreText; }
            set
            {
                _progressRestoreText = value;
                OnPropertyChanged(nameof(ProgressRestoreText));
            }
        }


        private int _progressBackupValue;
        public int ProgressBackupValue
        {
            get { return _progressBackupValue; }
            set
            {
                _progressBackupValue = value;
                OnPropertyChanged(nameof(ProgressBackupValue));
            }
        }

        private int _progressRestoreValue;
        public int ProgressRestoreValue
        {
            get { return _progressRestoreValue; }
            set
            {
                _progressRestoreValue = value;
                OnPropertyChanged(nameof(ProgressRestoreValue));
            }
        }

        public ICommand BackupCommand { get; set; }
        public ICommand RestoreCommand { get; set; }

        public ICommand BrowseBackupCommand { get; set; }
        public ICommand BrowseRestoreCommand { get; set; }
        public BackupRestoreDBViewModel()
        {
            BackupCommand = new RelayCommand<object>((p) => { return !BackupPath.IsNullOrEmpty(); }, (p) => { Backup(); });
            RestoreCommand = new RelayCommand<object>((p) => { return !RestorePath.IsNullOrEmpty(); }, (p) => { Restore(); });

            BrowseBackupCommand = new RelayCommand<object>((p) => { return true; }, (p) => { BrowseBackup(); });
            BrowseRestoreCommand = new RelayCommand<object>((p) => { return true; }, (p) => { BrowseRestore(); });
        }

        public void BrowseBackup()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog.SelectedPath;
                BackupPath = path;
            }

        }

        public void BrowseRestore()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Backup Files(*.bak)|*.bak";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog.FileName;
                RestorePath = path;
            }
        }

        public void Backup()
        {
            try
            {
                string Database = "MyShopContext.DatabaseName";
                if (BackupPath == null)
                {
                    MessageBox.Show("Vui lòng chọn đường dẫn để sao lưu dữ liệu");
                    return;
                }
                string sql = $"BACKUP DATABASE {Database} TO DISK = '{BackupPath}\\{Database}_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.bak'";
                MyShopContext context = new MyShopContext();
                context.Database.ExecuteSqlRaw(sql);
                MessageBox.Show("Sao lưu dữ liệu thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sao lưu dữ liệu thất bại");
            }
        }


        public void Restore()
        {
            try
            {
                string Database = MyShopContext.DatabaseName;
                if (RestorePath == null)
                {
                    MessageBox.Show("Vui lòng chọn đường dẫn để phục hồi dữ liệu");
                    return;
                }
                string str = $"ALTER DATABASE {Database} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                MyShopContext context = new MyShopContext();
                context.Database.ExecuteSqlRaw(str);

                string sql = $"USE MASTER RESTORE DATABASE {Database} FROM DISK = '{RestorePath}' WITH REPLACE";
                context.Database.ExecuteSqlRaw(sql);

                string str2 = $"ALTER DATABASE {Database} SET MULTI_USER";
                context.Database.ExecuteSqlRaw(str2);
                MessageBox.Show("Phục hồi dữ liệu thành công");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Phục hồi dữ liệu thất bại");
            }

        }
    }
}
