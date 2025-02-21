using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Input;
using static DoosanTest.Doosi;

namespace DoosanTest
{
    public class MainViewModel : BaseViewModel
    {
        private MainWindow mainWindow;
        ModbusClient modbusClient;

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
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Doosi.IsConnected())
                Doosi.DisConnect();
        }

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
        public int Reg30 { get; set; }
        public int Reg31 { get; set; }

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
                            short integerPart = (short)PosX;
                            short fractionalPart = (short)((PosX - integerPart) * 1000);
                            int[] vals = { integerPart, fractionalPart };
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
                                        bool rm = Doosi.GetRobotMode();
                                        string state = Doosi.GetRobotState();
                                        string speedmode = Doosi.GetSpeedMode();
                                        string progstate = Doosi.GetProgramState();
                                        var reg30 = modbusClient.ReadHoldingRegisters(30, 2);
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
