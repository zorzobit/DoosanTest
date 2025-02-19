
#include <windows.h>
#include <iostream>
#include <conio.h>

#ifndef DRCF_VERSION
#define DRCF_VERSION 2
#endif
#include <DRFLEx.h>
using namespace DRAFramework;

#undef NDEBUG
#include <assert.h>

CDRFLEx drfl;
bool g_bHasControlAuthority = FALSE;
bool g_TpInitailizingComplted = FALSE;
bool g_mStat = FALSE;
bool g_Stop = FALSE;
bool isConnected = FALSE;
const char* StateToString(ROBOT_STATE state) {
    switch (state) {
    case STATE_INITIALIZING: return "Initializing";
    case STATE_STANDBY: return "Standby";
    case STATE_MOVING: return "Moving";
    case STATE_SAFE_OFF: return "Safe Off";
    case STATE_TEACHING: return "Teaching";
    case STATE_SAFE_STOP: return "Safe Stop";
    case STATE_EMERGENCY_STOP: return "Emergency Stop";
    case STATE_HOMMING: return "Homing";
    case STATE_RECOVERY: return "Recovery";
    case STATE_SAFE_STOP2: return "Safe Stop 2";
    case STATE_SAFE_OFF2: return "Safe Off 2";
    case STATE_RESERVED1: return "Reserved 1";
    case STATE_RESERVED2: return "Reserved 2";
    case STATE_RESERVED3: return "Reserved 3";
    case STATE_RESERVED4: return "Reserved 4";
    case STATE_NOT_READY: return "Not Ready";
    case STATE_LAST: return "Last";
    default: return "Unknown State";
    }
}const char* SpeedModeToString(SPEED_MODE speed_mode) {
    switch (speed_mode) {
    case SPEED_NORMAL_MODE: return "Normal";
    case SPEED_REDUCED_MODE: return "Reduced";
    default: return "Unknown State";
    }
}
const char* ProgramStateToString(DRL_PROGRAM_STATE prg_state) {
    switch (prg_state) {
    case DRL_PROGRAM_STATE_PLAY: return "PLAY";
    case DRL_PROGRAM_STATE_STOP: return "STOP";
    case DRL_PROGRAM_STATE_HOLD: return "HOLD";
    case DRL_PROGRAM_STATE_LAST: return "LAST";
    default: return "Unknown State";
    }
}

int mCnt = 0;
std::string IP = "192.168.137.100";

void OnTpInitializingCompleted()
{
    // Tp
    g_TpInitailizingComplted = TRUE;
    drfl.ManageAccessControl(MANAGE_ACCESS_CONTROL_FORCE_REQUEST);
}

void OnHommingCompleted()
{
    // 50msec
    cout << "homming completed" << endl;
}

void OnProgramStopped(const PROGRAM_STOP_CAUSE)
{
    assert(drfl.PlayDrlStop(STOP_TYPE_SLOW));
    // 50msec
    //assert(drfl.SetRobotMode(ROBOT_MODE_MANUAL));
    cout << "program stopped" << endl;
}

LPMONITORING_DATA LPMonData;

void OnMonitoringDataCB(const LPMONITORING_DATA pData)
{
    LPMonData = pData;
    return;
}

void OnMonitoringDataExCB(const LPMONITORING_DATA_EX pData)
{
    return;
    cout << "# monitoring 1 data "
        << pData->_tCtrl._tJoint._fActualPos[1]
        << pData->_tCtrl._tJoint._fActualPos[1]
        << pData->_tCtrl._tJoint._fActualPos[2]
        << pData->_tCtrl._tJoint._fActualPos[3]
        << pData->_tCtrl._tJoint._fActualPos[4]
        << pData->_tCtrl._tJoint._fActualPos[5] << endl;
}

void OnMonitoringCtrlIOCB(const LPMONITORING_CTRLIO pData)
{
    return;
    cout << "# monitoring ctrl 0 data" << endl;
    for (int i = 0; i < 16; i++)
    {
        cout << (int)pData->_tInput._iActualDI[i] << endl;
    }
}

void OnMonitoringCtrlIOExCB(const LPMONITORING_CTRLIO_EX pData)
{
    return;
    cout << "# monitoring ctrl 1 data" << endl;
    for (int i = 0; i < 16; i++)
    {
        cout << (int)pData->_tInput._iActualDI[i] << endl;
    }
    for (int i = 0; i < 16; i++)
    {
        cout << (int)pData->_tOutput._iTargetDO[i] << endl;
    }
}

void OnMonitoringStateCB(const ROBOT_STATE eState)
{
    // 50msec 
    switch ((unsigned char)eState)
    {
#if 0  // TP
    case STATE_NOT_READY:
        if (g_bHasControlAuthority) drfl.SetRobotControl(CONTROL_INIT_CONFIG);
        break;
    case STATE_INITIALIZING:
        // add initalizing logic
        if (g_bHasControlAuthority) drfl.SetRobotControl(CONTROL_ENABLE_OPERATION);
        break;
#endif
    case STATE_EMERGENCY_STOP:
        // popup
        break;
    case STATE_STANDBY:
    case STATE_MOVING:
    case STATE_TEACHING:
        break;
    case STATE_SAFE_STOP:
        if (g_bHasControlAuthority) {
            drfl.SetSafeStopResetType(SAFE_STOP_RESET_TYPE_DEFAULT);
            drfl.SetRobotControl(CONTROL_RESET_SAFET_STOP);
        }
        break;
    case STATE_SAFE_OFF:
        //cout << "STATE_SAFE_OFF1" << endl;
        if (g_bHasControlAuthority) {
            //cout << "STATE_SAFE_OFF2" << endl;
            drfl.SetRobotControl(CONTROL_SERVO_ON);
        }
        break;
    case STATE_SAFE_STOP2:
        if (g_bHasControlAuthority) drfl.SetRobotControl(CONTROL_RECOVERY_SAFE_STOP);
        break;
    case STATE_SAFE_OFF2:
        if (g_bHasControlAuthority) {
            drfl.SetRobotControl(CONTROL_RECOVERY_SAFE_OFF);
        }
        break;
    case STATE_RECOVERY:
        //drfl.SetRobotControl(CONTROL_RESET_RECOVERY);
        break;
    default:
        break;
    }
    return;
    cout << "current state: " << (int)eState << endl;
}

void OnMonitroingAccessControlCB(const MONITORING_ACCESS_CONTROL eTrasnsitControl)
{
    // 50msec 

    switch (eTrasnsitControl)
    {
    case MONITORING_ACCESS_CONTROL_REQUEST:
        assert(drfl.ManageAccessControl(MANAGE_ACCESS_CONTROL_RESPONSE_NO));
        //drfl.ManageAccessControl(MANAGE_ACCESS_CONTROL_RESPONSE_YES);
        break;
    case MONITORING_ACCESS_CONTROL_GRANT:
        g_bHasControlAuthority = TRUE;
        //cout << "GRANT1" << endl;
        //cout << "MONITORINGCB : " << (int)drfl.GetRobotState() << endl;
        OnMonitoringStateCB(drfl.GetRobotState());
        //cout << "GRANT2" << endl;
        break;
    case MONITORING_ACCESS_CONTROL_DENY:
    case MONITORING_ACCESS_CONTROL_LOSS:
        g_bHasControlAuthority = FALSE;
        if (g_TpInitailizingComplted) {
            //assert(drfl.ManageAccessControl(MANAGE_ACCESS_CONTROL_REQUEST));
            drfl.ManageAccessControl(MANAGE_ACCESS_CONTROL_FORCE_REQUEST);
        }
        break;
    default:
        break;
    }
}

void OnLogAlarm(LPLOG_ALARM tLog)
{
    g_mStat = true;
    cout << "Alarm Info: " << "group(" << (unsigned int)tLog->_iGroup << "), index(" << tLog->_iIndex
        << "), param(" << tLog->_szParam[0] << "), param(" << tLog->_szParam[1] << "), param(" << tLog->_szParam[2] << ")" << endl;
}

void OnTpPopup(LPMESSAGE_POPUP tPopup)
{
    cout << "Popup Message: " << tPopup->_szText << endl;
    cout << "Message Level: " << tPopup->_iLevel << endl;
    cout << "Button Type: " << tPopup->_iBtnType << endl;
}

void OnTpLog(const char* strLog)
{
    cout << "Log Message: " << strLog << endl;
}

void OnTpProgress(LPMESSAGE_PROGRESS tProgress)
{
    cout << "Progress cnt : " << (int)tProgress->_iTotalCount << endl;
    cout << "Current cnt : " << (int)tProgress->_iCurrentCount << endl;
}

void OnTpGetuserInput(LPMESSAGE_INPUT tInput)
{
    cout << "User Input : " << tInput->_szText << endl;
    cout << "Data Type : " << (int)tInput->_iType << endl;
}

void OnRTMonitoringData(LPRT_OUTPUT_DATA_LIST tData)
{
    return;
    if (mCnt == 1000)
    {

        printf("timestamp : %.3f\n", tData->time_stamp);
        printf("joint : %f %f %f %f %f %f\n", tData->actual_joint_position[0], tData->actual_joint_position[1], tData->actual_joint_position[2], tData->actual_joint_position[3], tData->actual_joint_position[4], tData->actual_joint_position[5]);
        mCnt = 0;
    }
    else {
        mCnt++;
    }
}

void OnMonitoringSafetyStateCB(SAFETY_STATE iState)
{
    return;
    cout << iState << endl;
}

void OnMonitoringRobotSystemCB(ROBOT_SYSTEM iSystem)
{
    return;
    cout << iSystem << endl;
}

void OnMonitoringSafetyStopTypeCB(const unsigned char iSafetyStopType)
{
    return;
}

void OnDisConnected()
{
    while (!drfl.open_connection(IP)) {
        Sleep(1000);
    }
}
extern "C" __declspec(dllexport) bool WINAPI IsConnected() {
    return isConnected;
}

extern "C" __declspec(dllexport) bool WINAPI Connect(const char* ip) {
    IP = ip;
    drfl.set_on_homming_completed(OnHommingCompleted);
    drfl.set_on_monitoring_data(OnMonitoringDataCB);
    drfl.set_on_monitoring_data_ex(OnMonitoringDataExCB);
    drfl.set_on_monitoring_ctrl_io(OnMonitoringCtrlIOCB);
    drfl.set_on_monitoring_ctrl_io_ex(OnMonitoringCtrlIOExCB);
    drfl.set_on_monitoring_state(OnMonitoringStateCB);
    drfl.set_on_monitoring_access_control(OnMonitroingAccessControlCB);
    drfl.set_on_tp_initializing_completed(OnTpInitializingCompleted);
    drfl.set_on_log_alarm(OnLogAlarm);
    drfl.set_on_tp_popup(OnTpPopup);
    drfl.set_on_tp_log(OnTpLog);
    drfl.set_on_tp_progress(OnTpProgress);
    drfl.set_on_tp_get_user_input(OnTpGetuserInput);
    drfl.set_on_rt_monitoring_data(OnRTMonitoringData);

    drfl.set_on_monitoring_robot_system(OnMonitoringRobotSystemCB);
    drfl.set_on_monitoring_safety_state(OnMonitoringSafetyStateCB);
    drfl.set_on_monitoring_safety_stop_type(OnMonitoringSafetyStopTypeCB);

    drfl.set_on_program_stopped(OnProgramStopped);
    drfl.set_on_disconnected(OnDisConnected);
    isConnected = drfl.open_connection(ip);
    return isConnected;
}
extern "C" __declspec(dllexport) void WINAPI DisConnect(const char* ip) {
    drfl.CloseConnection();
    isConnected = FALSE;
}
extern "C" __declspec(dllexport) void WINAPI GetSystemVersion(const char** result) {
    SYSTEM_VERSION tSysVerion = { '\0', };
    drfl.get_system_version(&tSysVerion);
    *result = _strdup(tSysVerion._szController);
}
extern "C" __declspec(dllexport) void WINAPI GetLibraryVersion(const char** result) {
    *result = _strdup(drfl.get_library_version());
}
extern "C" __declspec(dllexport) void WINAPI GetRobotState(const char** result) {
    ROBOT_STATE state = drfl.get_robot_state();
    *result = StateToString(state);
}
extern "C" __declspec(dllexport) void WINAPI GetSpeedMode(const char** result) {
    SPEED_MODE speed_mode = drfl.get_robot_speed_mode();
    *result = SpeedModeToString(speed_mode);
}
extern "C" __declspec(dllexport) bool WINAPI SetControlMode(ROBOT_CONTROL mode) {
    return drfl.SetRobotControl(mode);
}
extern "C" __declspec(dllexport) void WINAPI GetProgramState(const char** result) {
    DRL_PROGRAM_STATE prg_state = drfl.get_program_state();
    *result = ProgramStateToString(prg_state);
}
extern "C" __declspec(dllexport) void WINAPI GetCurrentPose(ROBOT_POSE* pPose) {
    if (pPose) {
        pPose = drfl.get_current_pose(ROBOT_SPACE_JOINT);
    }
}
extern "C" __declspec(dllexport) void WINAPI GetCurrentPosj(LPROBOT_POSE* pPose) {
    if (pPose) {
        *pPose = drfl.get_desired_posj();
    }
}
extern "C" __declspec(dllexport) bool WINAPI MoveJ(float* posx, float vel, float acc) {
    ROBOT_STATE state = drfl.get_robot_state();
    float posf[6] = { 0 };
    std::copy(posx, posx + 6, posf);
    bool result = drfl.MoveJ(posf, vel, acc);
    return result;
}
#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

extern "C" {
    EXPORT LPMONITORING_DATA GetMonitoringData() {
        return LPMonData;
    }
}

extern "C" __declspec(dllexport) void WINAPI ProgTest() {
    string strDrlProgram = "\r\n\
loop = 0\r\n\
while loop < 3:\r\n\
 movej(posj(10,10.10,10,10.10), vel=60, acc=60)\r\n\
 movej(posj(00,00.00,00,00.00), vel=60, acc=60)\r\n\
 loop+=1\r\n\
movej(posj(10,10.10,10,10.10), vel=60, acc=60)\r\n";
    auto setmode = drfl.set_robot_mode(ROBOT_MODE_AUTONOMOUS);
    if (drfl.get_robot_state() == STATE_STANDBY) {
        if (drfl.get_robot_mode() == ROBOT_MODE_AUTONOMOUS) {
            // Automatic Mode
            ROBOT_SYSTEM eTargetSystem = ROBOT_SYSTEM_VIRTUAL;
            drfl.drl_start(eTargetSystem, strDrlProgram);
        }
    }
}

extern "C" __declspec(dllexport) bool WINAPI GetRobotMode() {
    return drfl.get_robot_mode() == ROBOT_MODE_AUTONOMOUS;
}
extern "C" __declspec(dllexport) bool WINAPI SwitchRobotMode() {
    if (drfl.get_robot_mode() == ROBOT_MODE_AUTONOMOUS) {
        return drfl.set_robot_mode(ROBOT_MODE_MANUAL);
    }
    else {
        return drfl.set_robot_mode(ROBOT_MODE_AUTONOMOUS);
    }
}

int main() {
	
    int key;
    cin >> key;
    drfl.CloseConnection();
	return 0;
}