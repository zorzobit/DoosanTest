using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static DoosanTest.Doosi;

namespace DoosanTest
{
    public class Doosi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct ROBOT_POSE
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] // NUM_JOINT is 6
            public float[] _fPosition;
        }
        const int NUMBER_OF_JOINT = 6;
        const int NUM_JOINT = 6;
        const int NUM_FLANGE_IO = 6;
        const int NUM_BUTTON = 5;
        const int MAX_FLANGE_AI = 4; 

        [StructLayout(LayoutKind.Sequential)]
    public struct MONITORING_DATA
    {
        public MONITORING_CONTROL _tCtrl;
        public MONITORING_MISC _tMisc;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORING_CONTROL
    {
        public ROBOT_MONITORING_STATE _tState;
        public ROBOT_MONITORING_JOINT _tJoint;
        public ROBOT_MONITORING_TASK _tTask;
        public ROBOT_MONITORING_TORQUE _tTorque;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MONITORING_MISC
    {
        public double _dSyncTime;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_FLANGE_IO)]
        public byte[] _iActualDI;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_FLANGE_IO)]
        public byte[] _iActualDO;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public byte[] _iActualBK;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_BUTTON)]
        public uint[] _iActualBT;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualMC;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualMT;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ROBOT_MONITORING_STATE
    {
        public byte _iActualMode;
        public byte _iActualSpace;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ROBOT_MONITORING_JOINT
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualAbs;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualErr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fTargetPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fTargetVel;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ROBOT_MONITORING_TASK
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * NUMBER_OF_JOINT)]
        public float[] _fActualPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualErr;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetVel;

        public byte _iSolutionSpace;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public float[] _fRotationMatrix;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ROBOT_MONITORING_TORQUE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fDynamicTor;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualJTS;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualEJT;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUM_JOINT)]
        public float[] _fActualETT;
    }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_DATA_EX
    {
        public MONITORING_CONTROL_EX _tCtrl;
        public MONITORING_MISC _tMisc;
        public MONITORING_MISC_EX _tMiscEx;
        public MONITORING_MODEL _tModel;
        public MONITORING_FLANGE_IO_CONFIG _tFlangeIo;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_MISC_EX
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 120)]
        public byte[] _szReserved1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_MODEL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] _szReserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_FLANGE_IO_CONFIG
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_FLANGE_AI)]
        public float[] _iActualAI;

        public byte _iX1Rs485FAIPinMux;
        public byte _iX2Rs485FAIPinMux;
        public byte _iX1DOBjtType;
        public byte _iX2DOBjtType;
        public byte _iVoutLevel;
        public byte _iFAI0Mode;
        public byte _iFAI1Mode;
        public byte _iFAI2Mode;
        public byte _iFAI3Mode;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] _szX1Baudrate;

        public byte _szX1DataLength;
        public byte _szX1Parity;
        public byte _szX1StopBit;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public byte[] _szX2Baudrate;

        public byte _szX2DataLength;
        public byte _szX2Parity;
        public byte _szX2StopBit;
        public byte _iServoSafetyMode;
        public byte _iInterruptSafetyMode;
    }

[StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ROBOT_MONITORING_WORLD
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualW2B;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * NUMBER_OF_JOINT)]
        public float[] _fActualPos; // Flattened 2D array

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualETT;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)] // 3x3 flattened
        public float[] _fRotationMatrix;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ROBOT_MONITORING_USER
    {
        public byte _iActualUCN;
        public byte _iParent;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 * NUMBER_OF_JOINT)]
        public float[] _fActualPos; // Flattened 2D array

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualETT;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetPos;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fTargetVel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)] // 3x3 flattened
        public float[] _fRotationMatrix;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_CONTROL_EX
    {
        public ROBOT_MONITORING_STATE _tState;
        public ROBOT_MONITORING_JOINT _tJoint;
        public ROBOT_MONITORING_TASK _tTask;
        public ROBOT_MONITORING_TORQUE _tTorque;
        public ROBOT_MONITORING_WORLD _tWorld;
        public ROBOT_MONITORING_USER _tUser;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_FORCECONTROL
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualHDT;

        public byte _iSingularHandlingMode;
        public byte _isMoving;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MONITORING_AMODEL
    {
        public ROBOT_MONITORING_SENSOR _tSensor;
        public float _fSingularity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ROBOT_MONITORING_SENSOR
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualFTS;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = NUMBER_OF_JOINT)]
        public float[] _fActualCS;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] _fActualACS;
    }

    public enum ROBOT_CONTROL
        {
            CONTROL_INIT_CONFIG,
            CONTROL_ENABLE_OPERATION,
            CONTROL_RESET_SAFET_STOP,
            CONTROL_RESET_SAFET_OFF,
            CONTROL_SERVO_ON = CONTROL_RESET_SAFET_OFF,
            CONTROL_RECOVERY_SAFE_STOP,
            CONTROL_RECOVERY_SAFE_OFF,
            CONTROL_RECOVERY_BACKDRIVE,
            CONTROL_RESET_RECOVERY
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "IsConnected")]
        public static extern bool IsConnected();
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "SetControlMode")]
        public static extern bool SetControlMode(ROBOT_CONTROL mode);
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "Connect")]
        public static extern bool Connect(string ip);
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "DisConnect")]
        public static extern void DisConnect();
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetSystemVersion", CallingConvention = CallingConvention.StdCall)]
        private static extern void getSystemVersion(out IntPtr result);
        public static string GetSystemVersion()
        {
            IntPtr resultPtr;
            getSystemVersion(out resultPtr);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            return result;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetRobotState", CallingConvention = CallingConvention.StdCall)]
        private static extern void getRobotState(out IntPtr result);
        public static string GetRobotState()
        {
            IntPtr resultPtr;
            getRobotState(out resultPtr);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            return result;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetSpeedMode", CallingConvention = CallingConvention.StdCall)]
        private static extern void getSpeedMode(out IntPtr result);
        public static string GetSpeedMode()
        {
            IntPtr resultPtr;
            getSpeedMode(out resultPtr);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            return result;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetProgramState", CallingConvention = CallingConvention.StdCall)]
        private static extern void getProgramState(out IntPtr result);
        public static string GetProgramState()
        {
            IntPtr resultPtr;
            getProgramState(out resultPtr);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            return result;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetLibraryVersion", CallingConvention = CallingConvention.StdCall)]
        private static extern void getLibraryVersion(out IntPtr result);
        public static string GetLibraryVersion()
        {
            IntPtr resultPtr;
            getLibraryVersion(out resultPtr);
            string result = Marshal.PtrToStringAnsi(resultPtr);
            return result;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetCurrentPose", CallingConvention = CallingConvention.StdCall)]
        private static extern void getCurrentPose(ref ROBOT_POSE pose);
        public static ROBOT_POSE GetCurrentPose()
        {
            ROBOT_POSE pose = new ROBOT_POSE { _fPosition = new float[6] }; // Initialize array
            getCurrentPose(ref pose);
            return pose;
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetCurrentPosj", CallingConvention = CallingConvention.StdCall)]
        private static extern void getCurrentPosj(ref ROBOT_POSE pose);
        public static ROBOT_POSE GetCurrentPosj()
        {
            ROBOT_POSE pose = new ROBOT_POSE { _fPosition = new float[6] }; // Initialize array
            getCurrentPosj(ref pose);
            return pose;
        }

        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "MoveJ", CallingConvention = CallingConvention.StdCall)]
        public static extern bool MoveJ(float[] posex, float vel, float acc);


        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "ProgTest")]
        public static extern bool ProgTest();
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetRobotMode")]
        public static extern bool GetRobotMode();
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "SwitchRobotMode")] 
        public static extern bool SwitchRobotMode();


        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetMonitoringData", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr getMonitoringData();
        public static MONITORING_DATA GetMonitoringData()
        {
            IntPtr ptr = getMonitoringData();
            if (ptr != IntPtr.Zero)
            {
                MONITORING_DATA data = Marshal.PtrToStructure<MONITORING_DATA>(ptr);
                return data;
            }
            return new MONITORING_DATA();
        }
        [DllImport("Lib\\DoosanInterface.dll", EntryPoint = "GetMonitoringDataEx", CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr getMonitoringDataEx();
        public static MONITORING_DATA_EX GetMonitoringDataEx()
        {
            IntPtr ptr = getMonitoringDataEx();
            if (ptr != IntPtr.Zero)
            {
                MONITORING_DATA_EX data = Marshal.PtrToStructure<MONITORING_DATA_EX>(ptr);
                return data;
            }
            return new MONITORING_DATA_EX();
        }
    }
}
