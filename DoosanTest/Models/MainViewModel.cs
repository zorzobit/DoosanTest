using System.Runtime.ConstrainedExecution;
using System.Windows;
using System.Windows.Input;
using static DoosanTest.Doosi;

namespace DoosanTest
{
    public class MainViewModel : BaseViewModel
    {
        private MainWindow mainWindow;

        public MainViewModel(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.mainWindow.Closing += MainWindow_Closing;
            IpString = "192.168.56.101";
            SystemVerLabel = "Label";
            ConnectionButtonText = "Connect";
            IpBoxIsEnabled = true;
            IsConnectedText = "NOT CONNECTED";
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
        
        public string PosX { get; set; }
        public string PosY { get; set; }
        public string PosZ { get; set; }
        public string PosRx { get; set; }
        public string PosRy { get; set; }
        public string PosRz { get; set; }
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
                                        var gsgs = GetMonitoringData();
                                        bool rm = Doosi.GetRobotMode();
                                        string state = Doosi.GetRobotState();
                                        string speedmode = Doosi.GetSpeedMode();
                                        string progstate = Doosi.GetProgramState();
                                        Application.Current.Dispatcher.Invoke((Action)delegate
                                        {
                                            try
                                            {
                                                PosX = gsgs._tCtrl._tJoint._fActualPos[0].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[0].ToString();
                                                PosY = gsgs._tCtrl._tJoint._fActualPos[1].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[1].ToString();
                                                PosZ = gsgs._tCtrl._tJoint._fActualPos[2].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[2].ToString();
                                                PosRx = gsgs._tCtrl._tJoint._fActualPos[3].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[3].ToString();
                                                PosRy = gsgs._tCtrl._tJoint._fActualPos[4].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[4].ToString();
                                                PosRz = gsgs._tCtrl._tJoint._fActualPos[5].ToString() + "/" + gsgs._tCtrl._tJoint._fActualAbs[5].ToString();
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
    }
}
