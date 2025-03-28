# Doosan Robot Controller

A C# application for controlling and monitoring a Doosan robot using Modbus and TCP communication. The system includes a TCP server that interacts with clients, allowing them to read and write robot registers and positions.

## Features
- **TCP Server**: Listens for client connections to handle register read/write operations.
- **Modbus Communication**: Reads and writes register values to the robot.
- **Real-time Monitoring**: Continuously retrieves and updates robot state, position, and joint values.
- **Command Execution**: Sends movement commands (`MoveJ`), sets digital outputs, and switches robot modes.
- **Graphical User Interface**: Provides a WPF-based UI for interacting with the system.

---

## TCP Server Details

The application starts a **TCP server** on `192.168.56.1:20002` to communicate with clients.

### Supported Commands

| Command       | Description |
|--------------|------------|
| `GetR{index}` | Retrieves the float value of a specific register. |
| `SetR{index}={value}` | Updates a register with a new float value. |
| `GetPR{index}` | Retrieves a stored position (PR) as a JSON object. |

Example:
```
GetR10  →  Returns value of register 10
SetR15=5.23  →  Sets register 15 to 5.23
```

---

## Main Components

### `MainViewModel.cs`
Handles the core logic, including:
- **TCP Server** (`StartServer`, `HandleClient`)
- **Robot Connection** (`Connect`, `DisConnect`)
- **Register Management** (`UpdateRegister`, `SelectedRegister`)
- **Position Handling** (`GetPos`, `UpdatePR`)
- **Movement Commands** (`MoveJ`)
- **Mode Switching** (`SwitchMode`, `Reset`)

### `Doosi.cs`
Wrapper around the Doosan robot SDK:
- `Connect(ip)`: Connects to the robot.
- `DisConnect()`: Disconnects from the robot.
- `IsConnected()`: Returns the connection status.
- `MoveJ(pos, speed, acc)`: Executes a joint movement.
- `SwitchRobotMode()`: Toggles between manual and auto mode.
- `SetDigitalOutput(index, state)`: Controls digital outputs.

---

## Robot Monitoring

The system continuously updates:
- **Joint Positions** (`Joint1` - `Joint6`)
- **Task Positions** (`PosX`, `PosY`, `PosZ`, `PosRx`, `PosRy`, `PosRz`)
- **Register Values** (`Reg30` - `Reg41`)
- **Robot State** (`RobotMode`, `RobotState`, `SpeedMode`, `ProgramState`)

---

## Usage

1. Run the application.
2. Set the **IP Address** of the Doosan robot.
3. Click **Connect** to establish a connection.
4. Use available commands to read/write registers, move the robot, and control outputs.

---

## Dependencies
- **.NET 6+**
- **WPF** (for GUI)
- **ModbusClient** (for register communication)

---

## License
This project is licensed under the MIT License. See `LICENSE` for details.

