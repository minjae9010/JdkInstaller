using JavaInstaller.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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

namespace JavaInstaller
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private JdkInfoList _items;
        public JdkInfoList Items
        {
            get { return _items; }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public MainWindowViewModel()
        {
            Items = new JdkInfoList();
        }
    }

    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.DataContext = ViewModel;

            DbDAO dbDAO = new DbDAO();
            List<jdkInfo> result = dbDAO.SelectDataFromTable();

            if(result.Count > 0)
            {
                ViewModel.Items.Clear();
                foreach (var item in result)
                {
                    ViewModel.Items.Add(item);
                }
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 항목 가져오기
            jdkInfo selectedJdk = (sender as Button)?.DataContext as jdkInfo;

            if (selectedJdk != null && Directory.Exists(selectedJdk.path))
            {
                try
                {
                    // 환경 변수명과 값 설정
                    string environmentVariableName = "JdkInstaller";
                    string environmentVariableValue = selectedJdk.path;

                    // 시스템 변수의 Path 값 가져오기
                    string pathVariable = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine);
                    string pattern = $@"(?:^|;)([^;]*\\Jdk\\[^;]*\\bin)";

                    Regex regex = new Regex(pattern);
                    Match match = regex.Match(pathVariable);

                    // 중복 및 잔여 방지
                    if (match.Success)
                    {
                        String result = match.Value;
                        pathVariable = pathVariable.Replace(result, "");
                        Environment.SetEnvironmentVariable("Path", pathVariable, EnvironmentVariableTarget.Machine);

                    }

                    // 시스템 환경 변수에 등록
                    using (var regKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Environment", true))
                    {
                        if (regKey != null)
                        {
                            regKey.SetValue(environmentVariableName, environmentVariableValue);
                        }
                    }


                    // 새로운 Path 값 생성
                    string newPathValue = $"{pathVariable};\"%{environmentVariableName}%\\bin\"";

                    // 시스템 변수의 Path 값 업데이트
                    Environment.SetEnvironmentVariable("Path", newPathValue, EnvironmentVariableTarget.Machine);


                    // 완료 메시지 박스 표시
                    MessageBox.Show("환경 변수가 성공적으로 설정되었습니다.", "완료", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to set the environment variable: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWindow window = new AddWindow();
            window.ShowDialog();

            DbDAO dbDAO = new DbDAO();
            List<jdkInfo> result = dbDAO.SelectDataFromTable();

            if (result.Count > 0)
            {
                ViewModel.Items.Clear();
                foreach (var item in result)
                {
                    ViewModel.Items.Add(item);
                }
            }
        }

        private void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 항목 가져오기
            jdkInfo selectedJdk = (sender as Button)?.DataContext as jdkInfo;

            if (selectedJdk != null && Directory.Exists(selectedJdk.path))
            {
                try
                {
                    // 해당 폴더 열기
                    Process.Start(selectedJdk.path);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open the folder: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // 선택된 항목 가져오기
            jdkInfo selectedJdk = (sender as Button)?.DataContext as jdkInfo;

            if (selectedJdk != null)
            {
                // 폴더 삭제
                if (Directory.Exists(selectedJdk.path))
                {
                    Directory.Delete(selectedJdk.path, true);
                }

                // DB에서 삭제
                DbDAO dbDAO = new DbDAO();
                dbDAO.DeleteDataFromTable(selectedJdk);

                // 리스트에서 삭제
                ViewModel.Items.Remove(selectedJdk);
            }
        }
    }
}
