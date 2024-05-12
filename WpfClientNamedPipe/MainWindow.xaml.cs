using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfClientNamedPipe
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private NamedPipeClientStream _pipeClient;
    private Process _serverProcess;
    private readonly string pathExe = @"C:\Users\gabri\OneDrive\Documentos\workspaces\vs\NamedPipeWPF\WpfServerNamedPipe\bin\Debug\net6.0-windows\WpfServerNamedPipe.exe";

    public MainWindow()
    {
      InitializeComponent();
    }

    private async void Connection(object sender, RoutedEventArgs e)
    {
      try
      {
        //iniciar processo
        _serverProcess = new Process();
        _serverProcess.StartInfo.FileName = pathExe;
        _serverProcess.Start();

        //espera 5 segundos para conexão
        //await Task.Delay(5000);

        _pipeClient = new(".", "TestPipe", PipeDirection.InOut, PipeOptions.None);
        _pipeClient.Connect();
        StreamWriter writer = new(_pipeClient);
        writer.WriteLine("Gabriel");
        writer.Flush();

        StreamReader reader = new StreamReader(_pipeClient);
        string response = reader.ReadToEnd();
        //MessageBox.Show(response);

        StatusText.Text = response;

      }
      catch (Exception ex)
      {
        MessageBox.Show("Erro ao Conectar:" + ex.Message);
      }
    }
  }
}