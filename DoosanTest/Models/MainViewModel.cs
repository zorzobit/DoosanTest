using System.Net.Sockets;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using static DoosanTest.Doosi;

namespace DoosanTest
{
    public class MainViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        ModbusClient modbusClient;
        private TcpListener _listener;
        private bool _isRunning = true;

        public MainViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.mainWindow.Closing += MainWindow_Closing;
            IpString = "192.168.56.101";
            SystemVerLabel = "Label";
            ConnectionButtonText = "Connect";
            IpBoxIsEnabled = true;
            IsConnectedText = "NOT CONNECTED";
            modbusClient = new ModbusClient(IpString, 502);
            NumberList = Enumerable.Range(1, 16).ToList();
            Task.Run(() => StartServer());
        }
        private void StartServer()
        {
            try
            {
                _listener = new TcpListener(IPAddress.Parse("192.168.56.1"), 20002);
                _listener.Start();
                //Dispatcher.Invoke(() => StatusText.Text = "Server started...");

                while (_isRunning)
                {
                    var client = _listener.AcceptTcpClient();
                    Task.Run(() => HandleClient(client));
                }
            }
            catch (Exception ex)
            {
                //Dispatcher.Invoke(() => StatusText.Text = $"Error: {ex.Message}");
            }
        }

        private void HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                //Dispatcher.Invoke(() => ReceivedText.Text = receivedData);

                // Send a response
                string response = "ACK";
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                stream.Write(responseData, 0, responseData.Length);
            }
            catch (Exception ex)
            {
                //Dispatcher.Invoke(() => StatusText.Text = $"Client Error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Doosi.IsConnected())
                Doosi.DisConnect();
        }
        bool isON = false;

        public string SystemVerLabel { get; set; }
        public string LibraryVerLabel { get; set; }
        public string ConnectionButtonText { get; set; }
        public string IsConnectedText { get; set; }
        public string IpString { get; set; }
        public string RobotMode { get; set; } = "NO_CONN";
        public string RobotState { get; set; }
        public string SpeedMode { get; set; }
        public string ProgramState { get; set; }


        public bool IpBoxIsEnabled { get; set; }
        public bool IsNotBusy { get; set; } = false;

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float PosRx { get; set; }
        public float PosRy { get; set; }
        public float PosRz { get; set; }
        public float Joint1 { get; set; }
        public float Joint2 { get; set; }
        public float Joint3 { get; set; }
        public float Joint4 { get; set; }
        public float Joint5 { get; set; }
        public float Joint6 { get; set; }
        public float posxval { get; set; } = 0;
        public float posyval { get; set; } = 0;
        public float poszval { get; set; } = 0;
        public float posrxval { get; set; } = 0;
        public float posryval { get; set; } = 0;
        public float posrzval { get; set; } = 0;
        public int Reg30 { get; set; }
        public int Reg31 { get; set; }
        public int Reg32 { get; set; }
        public int Reg33 { get; set; }
        public int Reg34 { get; set; }
        public int Reg35 { get; set; }
        public int Reg36 { get; set; }
        public int Reg37 { get; set; }
        public int Reg38 { get; set; }
        public int Reg39 { get; set; }
        public int Reg40 { get; set; }
        public int Reg41 { get; set; }
        public bool Output0 { get; set; }
        public bool Output1 { get; set; }
        public bool Output2 { get; set; }
        public bool Output3 { get; set; }
        public bool Output4 { get; set; }
        public bool Output5 { get; set; }
        public bool Output6 { get; set; }
        public bool Output7 { get; set; }
        public bool Output8 { get; set; }
        public bool Output9 { get; set; }
        public bool Output10 { get; set; }
        public bool Output11 { get; set; }
        public bool Output12 { get; set; }
        public bool Output13 { get; set; }
        public bool Output14 { get; set; }
        public bool Output15 { get; set; }
        public List<int> NumberList { get; set; }

        public string IOisON { get; set; }
        public int SelectedIONum { get; set; }
        public ICommand SetIO
        {
            get
            {
                return new RelayCommand(o =>
                {
                    IsNotBusy = false;
                    Task.Run(() =>
                    {
                        if (Doosi.IsConnected())
                        {
                            if (isON)
                            {
                                var res1 = Doosi.SetDigitalOutput(SelectedIONum, true);
                            }
                            else
                            {
                                var res2 = Doosi.SetDigitalOutput(SelectedIONum, false);
                            }
                        }
                        IsNotBusy = true;
                    });

                }, o => true);
            }
        }
        public ICommand SetReg
        {
            get
            {
                return new RelayCommand(o =>
                {
                    IsNotBusy = false;
                    Task.Run(() =>
                    {
                        if (Doosi.IsConnected())
                        {
                            int[] vals = {
                                (short)posxval, (short)((posxval - (short)posxval) * 1000),
                                (short)posyval, (short)((posyval - (short)posyval) * 1000),
                                (short)poszval, (short)((poszval - (short)poszval) * 1000),
                                (short)posrxval, (short)((posrxval - (short)posrxval) * 1000),
                                (short)posryval, (short)((posryval - (short)posryval) * 1000),
                                (short)posrzval, (short)((posrzval - (short)posrzval) * 1000),
                            };
                            modbusClient.WriteMultipleRegisters(30, vals);
                        }
                        IsNotBusy = true;
                    });

                }, o => true);
            }
        }
        public ICommand SwitchMode
        {
            get
            {
                return new RelayCommand(o =>
                {
                    IsNotBusy = false;
                    Task.Run(() =>
                    {
                        if (Doosi.IsConnected())
                        {
                            Doosi.SwitchRobotMode();
                        }
                        IsNotBusy = true;
                    });
                }, o => true);
            }
        }
        public ICommand Reset
        {
            get
            {
                return new RelayCommand(o =>
                {
                    IsNotBusy = false;
                    Task.Run(() =>
                    {
                        if (Doosi.IsConnected())
                        {
                            if (RobotState == "Safe Off")
                            {
                                bool res2 = Doosi.SetControlMode(ROBOT_CONTROL.CONTROL_SERVO_ON);
                            }
                        }
                        IsNotBusy = true;
                    });
                    
                }, o => true);
            }
        }
        public ICommand MoveJ
        {
            get
            {
                return new RelayCommand(o =>
                {
                    IsNotBusy = false;
                    Task.Run(() =>
                    {
                        if (!Doosi.GetRobotMode()) Doosi.SwitchRobotMode();
                        float[] pos = { 10, 10, 10, 0, 0, 0, };
                        bool res = Doosi.MoveJ(pos, 5, 1);
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            IsConnectedText = "pos 1";
                        });
                        pos[0] = 0;
                        pos[1] = 0;
                        pos[2] = 0;
                        bool res2 = Doosi.MoveJ(pos, 1, 1);
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            IsConnectedText = "pos 2";
                        });
                        IsNotBusy = true;
                    });
                }, o => true);
            }
        }
        public ICommand Connect
        {
            get
            {
                return new RelayCommand(o =>
                {
                    if (ConnectionButtonText == "Connect")
                    {
                        IpBoxIsEnabled = false;
                        ConnectionButtonText = "Disconnect";
                        SystemVerLabel = "Started";
                        Task.Run(() =>
                        {
                            var res = Doosi.Connect(IpString);
                            if (Doosi.IsConnected())
                            {
                                IsNotBusy = true;
                                var ver = Doosi.GetSystemVersion();
                                var lib = Doosi.GetLibraryVersion();
                                modbusClient.Connect();
                                Application.Current.Dispatcher.Invoke((Action)delegate
                                {
                                    IsConnectedText = "CONNECTED";
                                    SystemVerLabel = ver;
                                    LibraryVerLabel = lib;
                                });
                                Task.Run(() =>
                                {
                                    while (Doosi.IsConnected())
                                    {
                                        var gsgs = GetMonitoringDataEx();
                                        var gsgs2 = GetMonitoringData();
                                        bool rm = Doosi.GetRobotMode();
                                        string state = Doosi.GetRobotState();
                                        string speedmode = Doosi.GetSpeedMode();
                                        string progstate = Doosi.GetProgramState();
                                        var reg30 = modbusClient.ReadHoldingRegisters(30, 12);
                                        if(SelectedIONum>=0)
                                        isON = Doosi.GetDigitalOutput(SelectedIONum);
                                        Application.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            try
                                            {
                                                if (gsgs._tCtrl._tJoint._fActualPos != null)
                                                {
                                                    Joint1 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[0]);
                                                    Joint2 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[1]);
                                                    Joint3 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[2]);
                                                    Joint4 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[3]);
                                                    Joint5 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[4]);
                                                    Joint6 = ToCeiling(gsgs._tCtrl._tJoint._fActualPos[5]);
                                                    PosX =   ToCeiling(gsgs._tCtrl._tTask._fActualPos[0]);
                                                    PosY =   ToCeiling(gsgs._tCtrl._tTask._fActualPos[1]);
                                                    PosZ =   ToCeiling(gsgs._tCtrl._tTask._fActualPos[2]);
                                                    PosRx =  ToCeiling(gsgs._tCtrl._tTask._fActualPos[3]);
                                                    PosRy =  ToCeiling(gsgs._tCtrl._tTask._fActualPos[4]);
                                                    PosRz = ToCeiling(gsgs._tCtrl._tTask._fActualPos[5]);
                                                    Reg30 = reg30[0];
                                                    Reg31 = reg30[1];
                                                    Reg32 = reg30[2];
                                                    Reg33 = reg30[3];
                                                    Reg34 = reg30[4];
                                                    Reg35 = reg30[5];
                                                    Reg36 = reg30[6];
                                                    Reg37 = reg30[7];
                                                    Reg38 = reg30[8];
                                                    Reg39 = reg30[9];
                                                    Reg40 = reg30[10];
                                                    Reg41 = reg30[11];
                                                    this.IOisON = isON ? "OFF" : "ON";
                                                }
                                            }
                                            catch (Exception)
                                            {
                                            }
                                            RobotMode = rm ? "AUTO" : "MANUAL";
                                            RobotState = state;
                                            SpeedMode = speedmode;
                                            ProgramState = progstate;
                                        });
                                        Thread.Sleep(200);
                                    }
                                });
                            }
                        });
                    }
                    else
                    {
                        IpBoxIsEnabled = true;
                        Doosi.DisConnect();
                        ConnectionButtonText = "Connect";
                        IsConnectedText = "NOT CONNECTED";
                    }
                }, o => true);
            }
        }
        private float ToCeiling(float value)
        {
            return (float)Math.Ceiling(value * 1000) / 1000;
        }
    }
}
