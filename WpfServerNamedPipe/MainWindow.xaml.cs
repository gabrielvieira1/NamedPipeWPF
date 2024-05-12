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

namespace WpfServerNamedPipe
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private NamedPipeServerStream _pipeServer;

    public MainWindow()
    {
      InitializeComponent();
      StartServer();
    }

    private void StartServer()
    {
      Task.Run(() =>
      {
        try
        {
          _pipeServer = new NamedPipeServerStream("TestPipe", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
          _pipeServer.WaitForConnection();

          StreamReader reader = new StreamReader(_pipeServer);
          string? name = reader.ReadLine();

          if (name == "Protocol" || !String.IsNullOrWhiteSpace(name))
          {
            Dispatcher.Invoke(() => StatusText.Text = name);
            StreamWriter writer = new StreamWriter(_pipeServer);
            writer.WriteLine($"Olá {name}! sou o Servidor WPF!");
            writer.Flush();
          }
          _pipeServer.Disconnect();
          Application.Current.Dispatcher.Invoke(() => Application.Current.Shutdown());
        }
        catch (Exception ex)
        {
          MessageBox.Show("Erro ao Conectar:" + ex.Message);
        }
        finally
        {


        }
      });
    }
  }
}