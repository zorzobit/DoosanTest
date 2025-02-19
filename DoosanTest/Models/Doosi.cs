using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
    }
}
